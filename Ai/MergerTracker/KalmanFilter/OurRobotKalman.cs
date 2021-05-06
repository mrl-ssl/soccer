using System.Collections.Generic;
using System.Linq;
using MRL.SSL.Common.Utils.Extensions;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using System;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;
using SMath = System.Math;
using MRL.SSL.Ai.Utils;

namespace MRL.SSL.Ai.MergerTracker
{
    public class OurRobotKalman : RobotKalman
    {
        protected struct RCommand
        {
            public RCommand(double _t, VectorF3D _vs)
            {
                timestamp = _t;
                vs = _vs;
            }
            public double timestamp;
            public VectorF3D vs;
        };
        protected double latency;
        protected Queue<RCommand> cs; // Velocity commands 

        public OurRobotKalman(double _latency) : base(7, 3, 4, MergerTrackerConfig.Default.FramePeriod)
        {
            cs = new();
            latency = _latency;
        }

        protected override void Initial(double t, MatrixF x, MatrixF p)
        {
            cs.Clear();
            cs.Enqueue(new RCommand(t, new VectorF3D()));
            base.Initial(t, x, p);
        }
        public override void Observe(Observation obs)
        {
            var fx = (xs.Count > 0 && xs.First().Rows > 2) ? xs.First() : null;

            if (SMath.Abs(obs.Time - time) > MergerTrackerConfig.Default.MaxPredictionTime
                || (fx != null && (float.IsNaN(fx[0, 0]) || float.IsNaN(fx[1, 0]) || float.IsNaN(fx[2, 0]))))
                Reset();

            if (_reset)
            {
                if (obs.Confidence <= 0.0) return;

                var _p = matrixBuilder.DenseZero(stateNum);
                var _x = matrixBuilder.DenseZero(stateNum, 1);

                _p[0, 0] = MergerTrackerConfig.Default.RobotPositionVariance;
                _p[1, 1] = MergerTrackerConfig.Default.RobotPositionVariance;
                _p[2, 2] = MergerTrackerConfig.Default.RobotAngleVariance;
                _p[3, 3] = 0f; // 0m/s
                _p[4, 4] = 0f; // 0m/s
                _p[5, 5] = 0f;
                _p[6, 6] = 0f;

                _x[0, 0] = obs.Location.X;
                _x[1, 0] = obs.Location.Y;
                _x[2, 0] = obs.Angle - MathF.PI / 2f;

                Initial(obs.Time, _x, _p);

                _reset = false;
            }
            else if (obs.Time > time)
            {

                Tick(obs.Time - time);

                var xtheta = xs.First()[2, 0];

                _z[0, 0] = obs.Location.X;
                _z[1, 0] = obs.Location.Y;
                _z[2, 0] = MathHelper.AngleMod((obs.Angle - MathF.PI / 2f) - xtheta) + xtheta;

                Update(_z);

                if (GetTimeElapsedError() > 10.0f)
                    ResetError();
            }
        }

        public override MatrixF A(MatrixF x)
        {
            float theta = x[2, 0];
            float vpar = x[3, 0], vperp = x[4, 0], vtheta = x[5, 0];
            float stuck = x[6, 0];
            float cos_theta = MathF.Cos(theta), sin_theta = MathF.Sin(theta);
            float step = (float)stepSize;

            _A[0, 2] = (1f - stuck) * step * (vpar * -sin_theta + vperp * -cos_theta);
            _A[0, 3] = cos_theta * (1f - stuck) * step;
            _A[0, 4] = -sin_theta * (1f - stuck) * step;
            _A[0, 6] = -step * (vpar * cos_theta + vperp * -sin_theta);
            _A[1, 2] = (1f - stuck) * step * (vpar * cos_theta + vperp * -sin_theta);
            _A[1, 3] = sin_theta * (1f - stuck) * step;
            _A[1, 4] = cos_theta * (1f - stuck) * step;
            _A[1, 6] = -step * (vpar * sin_theta + vperp * cos_theta);
            _A[2, 5] = (1f - stuck) * step;
            _A[2, 6] = -step * vtheta;
            _A[3, 3] = MergerTrackerConfig.Default.RobotVelocityNextStepCovariance;
            _A[4, 4] = MergerTrackerConfig.Default.RobotVelocityNextStepCovariance;
            _A[5, 5] = MergerTrackerConfig.Default.RobotVelocityNextStepCovariance;
            _A[6, 6] = (visionProblem) ? 1 : MergerTrackerConfig.Default.RobotStuckDecay;

            return _A;
        }

        public override MatrixF f(in MatrixF x, ref MatrixF I)
        {
            var c = GetCommand(steppedTime);

            float _vpar = x[3, 0], _vperp = x[4, 0], _vtheta = x[5, 0];

            x[6, 0] = MathHelper.BoundF(x[6, 0], 0, 1) * ((visionProblem) ? 1 : MergerTrackerConfig.Default.RobotStuckDecay);

            x[3, 0] = c.vs.X;
            x[4, 0] = c.vs.Y;
            x[5, 0] = c.vs.Z;

            float avgVpar, avgVperp, avgVtheta, avgTheta;
            float avgWeight = 0.5f;
            float step = (float)stepSize;

            if (MergerTrackerConfig.Default.RobotUseAverageInPropagation)
            {
                avgVpar = avgWeight * _vpar + (1f - avgWeight) * x[3, 0];
                avgVperp = avgWeight * _vperp + (1f - avgWeight) * x[4, 0];
                avgVtheta = avgWeight * _vtheta + (1f - avgWeight) * x[5, 0];
                avgTheta = avgWeight * x[2, 0];

                x[2, 0] += (1f - x[6, 0]) * step * avgVtheta;

                avgTheta += (1f - avgWeight) * x[2, 0];

                x[0, 0] += (1f - x[6, 0]) * step * (avgVpar * MathF.Cos(avgTheta) - avgVperp * MathF.Sin(avgTheta));
                x[1, 0] += (1f - x[6, 0]) * step * (avgVpar * MathF.Sin(avgTheta) + avgVperp * MathF.Cos(avgTheta));
            }
            else
            {
                x[0, 0] += (1f - x[6, 0]) * step * (x[3, 0] * MathF.Cos(x[2, 0]) - x[4, 0] * MathF.Sin(x[2, 0]));
                x[1, 0] += (1f - x[6, 0]) * step * (x[3, 0] * MathF.Sin(x[2, 0]) + x[4, 0] * MathF.Cos(x[2, 0]));
                x[2, 0] += (1f - x[6, 0]) * step * x[5, 0];
            }
            return x;
        }

        public override MatrixF Q(MatrixF x)
        {
            _Q[0, 0] = MergerTrackerConfig.Default.RobotVelocityVariance;
            _Q[1, 1] = MergerTrackerConfig.Default.RobotVelocityVariance;
            _Q[2, 2] = MergerTrackerConfig.Default.RobotAngularVelocityVariance;
            _Q[3, 3] = MergerTrackerConfig.Default.RobotStuckVariance;

            return _Q;
        }

        public override MatrixF R(MatrixF x)
        {
            _R[0, 0] = MergerTrackerConfig.Default.RobotPositionVariance;
            _R[1, 1] = MergerTrackerConfig.Default.RobotPositionVariance;
            _R[2, 2] = MergerTrackerConfig.Default.RobotAngleVariance;

            return _R;
        }
        public void PushCommand(VectorF3D v, double timestamp)
        {
            RCommand c = new(timestamp + latency - (MergerTrackerConfig.Default.FramePeriod / 2.0), v);

            while (cs.Count > 1 && cs.ElementAt(0).timestamp < time - stepSize)
            {
                cs.Dequeue();
            }
            cs.ReverseQ();
            while (cs.Count != 0 && cs.First().timestamp == c.timestamp)
            {
                cs.Dequeue();
            }
            cs.ReverseQ();
        }
        private RCommand GetCommand(double time)
        {
            if (cs.Count == 0 || cs.ElementAt(0).timestamp > time)
                return new RCommand(0.0, new VectorF3D(0f, 0f, 0f));
            int i;
            for (i = 1; i < cs.Count; i++)
            {
                if (cs.ElementAt(i).timestamp > time) break;
            }
            return cs.ElementAt(i - 1);//Export Last command in time
        }

        public override VectorF2D Position(double time)
        {
            if (MergerTrackerConfig.Default.RobotFastPredict)
            {
                if (time > latency)
                {
                    var x = Predict(latency);
                    return new VectorF2D(x[0, 0] + x[3, 0], x[1, 0] + x[4, 0] * (float)(time - latency));
                }
                else
                {
                    var x = Predict(time);
                    return new VectorF2D(x[0, 0], x[1, 0]);
                }
            }
            else
            {
                var x = Predict(time);

                return new VectorF2D(x[0, 0], x[1, 0]);
            }
        }
        public override VectorF2D Velocity(double time)
        {
            MatrixF x;

            if (MergerTrackerConfig.Default.RobotFastPredict)
            {
                if (time > latency)
                {
                    x = Predict(latency);
                }
                else
                {
                    x = Predict(time);
                }
            }
            else
                x = Predict(time);

            float a = x[2, 0];
            float c = MathF.Cos(a);
            float s = MathF.Sin(a);

            float vx = x[3, 0];
            float vy = x[4, 0];


            if (x[6, 0] > MergerTrackerConfig.Default.RobotStuckThreshold)
                return VectorF2D.Zero;

            return new VectorF2D(c * vx - s * vy, s * vx + c * vy);
        }
        // return the velocity un-rotated
        public override VectorF2D RawVelocity(double time)
        {
            MatrixF x;
            if (MergerTrackerConfig.Default.RobotFastPredict)
            {
                if (time > latency)
                {
                    x = Predict(latency);
                }
                else
                {
                    x = Predict(time);
                }
            }
            else
                x = Predict(time);

            if (x[6, 0] > MergerTrackerConfig.Default.RobotStuckThreshold)
                return VectorF2D.Zero;

            return new VectorF2D(x[3, 0], x[4, 0]);
        }
        public override float Direction(double time)
        {
            var x = Predict(time);
            return x[2, 0];
        }
        public override float AngularVelocity(double time)
        {
            var x = Predict(time);
            return x[5, 0];
        }
        public virtual float Stuck(double time)
        {
            var x = Predict(time);
            return MathHelper.BoundF(x[6, 0], 0f, 1f);
        }

    }
}