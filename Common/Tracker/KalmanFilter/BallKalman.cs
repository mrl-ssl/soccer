using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using System.Linq;
using System;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;
using SMath = System.Math;
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public enum OccludeType { Visible, MaybeOccluded, Occluded };

    public class BallKalman : KalmanBase
    {
        // Pointer up to the complete set of trackers... for collisions,
        // occlusions, and such.
        public Tracker Tracker { get; set; }

        // Occlusion Information
        public OccludeType Occluded { get; set; }
        public int OccludingTeam { get; set; }
        public int OccludingRobot { get; set; }
        public VectorF2D OccludingOffset { get; set; }
        public double OccludedLastObsTime { get; set; }
        protected bool checkCollision;
        public bool CheckCollision
        {
            get { return checkCollision; }
            set { checkCollision = value; }
        }
        protected uint obsCamId;

        public BallKalman() : base(4, 2, 2, MergerTrackerConfig.Default.FramePeriod)
        {
            Occluded = OccludeType.Visible;
            Tracker = null;
            if (MergerTrackerConfig.Default.PrintRobotKalmaError)
                predictionLookahead = MergerTrackerConfig.Default.Latency;
        }
        protected override void Propagate()
        {
            MatrixF x = xs.Last();
            MatrixF P = Ps.Last();
            MatrixF __A = A(x);
            MatrixF I = matrixBuilder.DenseZero(1, 1);

            x = f(x, ref I);
            var __W = W(x);
            var __Q = Q(x);
            tmpC = __W * __Q * __W.Transpose();
            P = __A * P * __A.Transpose() + tmpC;
            xs.Enqueue(x);
            Ps.Enqueue(P);
            Is.Enqueue(I);
            steppedTime += stepSize;
        }
        public override void Observe(Observation obs)
        {
            if (SMath.Abs(obs.Time - time) > MergerTrackerConfig.Default.MaxPredictionTime
                || (xs.Count > 0 && float.IsNaN(xs.ElementAt(0)[0, 0])))
                Reset();

            obsCamId = obs.Camera;

            if (_reset && obs.Confidence >= MergerTrackerConfig.Default.BallConfidenceThreshold)
            {
                var _p = matrixBuilder.DenseZero(stateNum);
                var _x = matrixBuilder.DenseZero(stateNum, 1);

                _p[0, 0] = MergerTrackerConfig.Default.BallPositionVariance;
                _p[1, 1] = MergerTrackerConfig.Default.BallPositionVariance;
                _p[2, 2] = 250000f; // 500m/s
                _p[3, 3] = 250000f; // 500m/s

                _x[0, 0] = obs.Location.X;
                _x[1, 0] = obs.Location.Y;
                _x[2, 0] = 0f;
                _x[3, 0] = 0f;

                Initial(obs.Time, _x, _p);
                Occluded = OccludeType.Visible;
                _reset = false;
            }
            else if (obs.Time > time)
            {
                if (_reset && Occluded != OccludeType.Occluded) return;

                // Tick to current time.
                if (Occluded == OccludeType.Occluded)
                {
                    TickOcclusion(obs.Time - time);
                }
                else
                {
                    Tick(obs.Time - time);
                }
                _z[0, 0] = obs.Location.X;
                _z[1, 0] = obs.Location.Y;
                if (MergerTrackerConfig.Default.BallImprobabilityFiltering)
                {
                    // Check for improbable observations (i.e. noise)
                    if (GetObsLikelihood(0.0, _z) <= MergerTrackerConfig.Default.BallLikelihoodThreshold)
                        obs.Confidence = -1f;
                }
                // Make observation
                if (obs.Confidence >= MergerTrackerConfig.Default.BallConfidenceThreshold)
                {
                    Update(_z);
                    Occluded = OccludeType.Visible;
                    OccludedLastObsTime = obs.Time;
                }
                else
                {

                    if (Occluded == OccludeType.Visible)
                        CheckOcclusion();

                    if (Occluded == OccludeType.MaybeOccluded &&
                        obs.Time - OccludedLastObsTime > MergerTrackerConfig.Default.BallOccludeTime)
                    {

                        Occluded = OccludeType.Occluded;
                        _reset = true;
                    }
                }
                if (GetTimeElapsedError() > 10.0f)
                    ResetError();
            }
        }
        public override MatrixF A(MatrixF x)
        {
            _A[0, 2] = (float)stepSize;
            _A[1, 3] = (float)stepSize;
            return _A;
        }

        public override MatrixF f(in MatrixF x, ref MatrixF I)
        {
            float _x = x[0, 0], _y = x[1, 0], _vx = x[2, 0], _vy = x[3, 0];
            float _v = MathF.Sqrt(_vx * _vx + _vy * _vy);

            float _a = MathF.Min(MergerTrackerConfig.Default.BallFriction * MergerTrackerConfig.Default.Gravity,
                                 _v / (float)stepSize);
            float _ax = MathHelper.EqualFloat(_v, 0f) ? 0f : -_a * _vx / _v;
            float _ay = MathHelper.EqualFloat(_v, 0f) ? 0f : -_a * _vy / _v;

            bool walls = false;

            if (MergerTrackerConfig.Default.BallWalls)
            {
                if ((MathF.Abs(_x) > FieldConfig.Default.OurGoalCenter.X + MergerTrackerConfig.Default.WallWidth &&
                 MathF.Abs(_y) > FieldConfig.Default.GoalWidth / 2f) ||
                (MathF.Abs(_y) > FieldConfig.Default.OurGoalLeft.X / 2f + MergerTrackerConfig.Default.WallWidth))
                {
                    _vx = 0f;
                    _vy = 0f;
                    _ax = 0f;
                    _ay = 0f;
                    walls = true;
                }
            }

            // Update Position
            _x += (float)(_vx * stepSize + 0.5 * _ax * stepSize * stepSize);
            _y += (float)(_vy * stepSize + 0.5 * _ay * stepSize * stepSize);

            // If there's a collision... then set ball's velocity to the colliding
            //  object's velocity.
            VectorF2D cv = VectorF2D.Zero, cp = VectorF2D.Zero;
            int team = 0, robot = 0;

            x[0, 0] = _x;
            x[1, 0] = _y;
            x[2, 0] = _vx;
            x[3, 0] = _vy;

            bool col = false;// (checkCollision) ? check_for_collision(_f, ref cp, ref cv, ref team, ref robot) : false;
            if (!walls && col)
            {

                _vx = cv.X; _vy = cv.Y;
                I = matrixBuilder.DenseZero(2, 1);
                I[0, 0] = team;
                I[1, 0] = robot;
            }
            else
            {
                _vx += (float)(_ax * stepSize);
                _vy += (float)(_ay * stepSize);
            }

            x[0, 0] = _x;
            x[1, 0] = _y;
            x[2, 0] = _vx;
            x[3, 0] = _vy;

            return x;
        }

        public override MatrixF Q(MatrixF x)
        {
            _Q[0, 0] = _Q[1, 1] = VelocityVariance(x);
            return _Q;
        }

        public override MatrixF R(MatrixF x)
        {
            _R[0, 0] = MergerTrackerConfig.Default.BallPositionVariance;
            _R[1, 1] = MergerTrackerConfig.Default.BallPositionVariance;

            return _R;
        }
        public virtual VectorF2D Position(double time)
        {
            if (Occluded == OccludeType.Occluded && checkCollision) return OccludedPosition(time);
            var x = Predict(time);
            return new VectorF2D(x[0, 0], x[1, 0]);
        }

        public virtual VectorF2D Velocity(double time)
        {
            if (Occluded == OccludeType.Occluded && checkCollision) return OccludedVelocity(time);

            var x = Predict(time);
            return new VectorF2D(x[2, 0], x[3, 0]);
        }

        public virtual MatrixF Covariances(double time)
        {
            return PredictCov(time);
        }
        public virtual bool Collision(double time, ref int team, ref int robot)
        {
            var I = PredictInfo(time);

            if (I.Rows <= 1) return false;

            team = (int)SMath.Round(I[1, 0]);
            robot = (int)SMath.Round(I[1, 0]);

            return true;
        }

        protected virtual float VelocityVariance(MatrixF x)
        {
            if (Tracker == null) return MergerTrackerConfig.Default.BallVelocityVarianceNearRobot;

            var ball = new VectorF2D(x[0, 0], x[1, 0]);
            float dist = float.MaxValue;
            float rad = 0;

            for (int i = 0; i < MergerTrackerConfig.Default.TeamsCount; i++)
            {
                for (int j = 0; j < MergerTrackerConfig.Default.MaxTeamRobots; j++)
                {
                    if (!Tracker.Exists(i, j)) continue;

                    float d = (Tracker.Robots[i, j].Position(0.0) - ball).Length();
                    if (d < dist)
                    {
                        dist = d;
                        rad = Tracker.GetRadius(i);
                    }
                }
            }

            float r = MathHelper.BoundF((dist - rad) / rad, 0f, 1f);

            return r * MergerTrackerConfig.Default.BallVelocityVarianceNoRobot +
              (1 - r) * MergerTrackerConfig.Default.BallVelocityVarianceNearRobot;
        }
        protected virtual bool CheckForCollision(MatrixF x, ref Vector2D<float> cp, ref Vector2D<float> cv, ref int team, ref int robot)
        {
            if (Tracker == null) return false;
            var bp = new VectorF2D(x[0, 0], x[1, 0]);
            var bv = new VectorF2D(x[2, 0], x[3, 0]);

            float dist = 5000f;
            bool rv = false;

            for (int i = 0; i < MergerTrackerConfig.Default.TeamsCount; i++)
            {
                float radius = 0;
                if (i == 0)
                    radius = MergerTrackerConfig.Default.BallTeammateCollisionRadius;
                else radius = MergerTrackerConfig.Default.BallOpponentCollisionRadius;
                if (radius <= 0) continue;

                for (int j = 0; j < MergerTrackerConfig.Default.MaxTeamRobots; j++)
                {
                    if (!Tracker.Exists(i, j)) continue;

                    var p = Tracker.Robots[i, j].Position(steppedTime - time);
                    var v = Tracker.Robots[i, j].Velocity(steppedTime - time);
                    var d = MathF.Min((p - bp).Length(),
                           (p + v * (float)stepSize - bp + bv * (float)stepSize).Length());
                    // Ball is within radius, nothing else is closest, and ball is
                    //  moving towards the robot...  Count as collision.
                    if (d <= radius && d < dist && (bv - v).Dot(p - bp) > 0f)
                    {
                        cp = p + (bp - p).GetNormTo(radius);

                        cv = v; rv = true; dist = d;
                        team = i; robot = j;
                    }
                }
            }
            return rv;
        }

        protected virtual bool CheckOcclusion()
        {
            if (Tracker == null || Tracker.Cameras == null || !Tracker.Cameras.ContainsKey(obsCamId)) return false;

            if (Occluded != OccludeType.Visible) return true;

            var camera = new VectorF2D(Tracker.Cameras[obsCamId].Tx, Tracker.Cameras[obsCamId].Ty);
            float cameraHeight = Tracker.Cameras[obsCamId].Tz;
            var ball = Position(0) - camera;

            float occludingPct = 0.5f;

            for (int i = 0; i < MergerTrackerConfig.Default.TeamsCount; i++)
            {
                for (int j = 0; j < MergerTrackerConfig.Default.MaxTeamRobots; j++)
                {
                    if (!Tracker.Exists(i, j)) continue;

                    float radius = Tracker.GetRadius(i);
                    float height = Tracker.GetHeight(i);

                    var p = Tracker.Robots[i, j].Position(0f) - camera;
                    float from = VectorF2D.OffsetToLine(VectorF2D.Zero, ball, p);
                    float along = VectorF2D.OffsetAlongLine(VectorF2D.Zero, ball, p);
                    float ballAlong = ball.Length();

                    if (MathF.Abs(from) > radius) continue;
                    if (ballAlong < along) continue;


                    along += MathF.Sqrt(radius * radius - from * from);

                    float x = (along * height) / (cameraHeight - height);
                    float pct = (x - (ballAlong - along) + MergerTrackerConfig.Default.BallRadi)
                                / (2f * MergerTrackerConfig.Default.BallRadi);

                    pct = MathHelper.BoundF(pct, 0, 1);

                    if (pct > occludingPct)
                    {
                        Occluded = OccludeType.MaybeOccluded;
                        OccludingTeam = i;
                        OccludingRobot = j;
                        OccludingOffset = (VectorF2D)(ball - p).GetRotate(-(p - camera).AngleInRadians());
                        occludingPct = pct;
                    }
                }

            }

            return (Occluded != OccludeType.Occluded);
        }

        protected virtual void TickOcclusion(double dt)
        {
            var camera = new VectorF2D(Tracker.Cameras[obsCamId].Tx, Tracker.Cameras[obsCamId].Ty);
            float cameraHeight = Tracker.Cameras[obsCamId].Tz;

            var p = Tracker.Robots[OccludingTeam, OccludingRobot].Position(0f);
            var v = Tracker.Robots[OccludingTeam, OccludingRobot].Velocity(0f);
            var b = OccludingOffset.GetRotate((p - camera).AngleInRadians());

            float bdelta = MathF.Max(v.Dot(b.GetNorm()) * (float)dt, 0.0f);
            float radius = Tracker.GetRadius(OccludingTeam);

            if (b.Length() - bdelta < radius) b.NormTo(radius);
            else b.NormTo(b.Length() - bdelta);

            OccludingOffset = b.GetRotate(-(p - camera).AngleInRadians());

            // Update the x and P queue.

            var xp = OccludedPosition(dt);
            var xv = OccludedVelocity(dt);

            var _p = matrixBuilder.DenseZero(stateNum);
            var _x = matrixBuilder.DenseZero(stateNum, 1);

            _x[0, 0] = xp.X;
            _x[1, 0] = xp.Y;
            _x[2, 0] = xv.X;
            _x[3, 0] = xv.Y;

            _p[0, 0] = MergerTrackerConfig.Default.BallPositionVariance;
            _p[1, 1] = MergerTrackerConfig.Default.BallPositionVariance;
            _p[2, 2] = 250000.0f; // 500m/s
            _p[3, 3] = 250000.0f; // 500m/s

            xs.Clear(); xs.Enqueue(_x);
            Ps.Clear(); Ps.Enqueue(_p);
            time += dt;
        }

        public VectorF2D OccludedPosition(double time)
        {
            if (Tracker == null || Tracker.Cameras == null || !Tracker.Cameras.ContainsKey(obsCamId)) return VectorF2D.Zero;

            var camera = new VectorF2D(Tracker.Cameras[obsCamId].Tx, Tracker.Cameras[obsCamId].Ty);

            Vector2D<float> b = Tracker.Robots[OccludingTeam, OccludingRobot].Position(time);
            b = (b + OccludingOffset.GetRotate((b - camera).AngleInRadians()));

            return (VectorF2D)b;
        }

        public VectorF2D OccludedVelocity(double time)
        {
            if (Tracker == null) return VectorF2D.Zero;
            return Tracker.Robots[OccludingTeam, OccludingRobot].Velocity(time);
        }
    }
}