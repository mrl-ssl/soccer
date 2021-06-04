using System.Collections.Generic;
using MRL.SSL.Common.SSLWrapperCommunication;
using MRL.SSL.Common.Utils;
using System.Linq;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.Configuration;
using System;

namespace MRL.SSL.Ai.MotionPlanner
{
    public class Controller
    {
        protected AdaptiveTunner aTunner;
        private bool inFineTuneX;
        private bool inFineTuneY;
        private bool inFineTuneW;

        public Controller()
        {
            aTunner = new AdaptiveTunner();
        }

        public SingleWirelessCommand CalculatePathSpeed(List<SingleObjectState> path, float targetAngle,
                                                        VectorF2D maxAccel = null, VectorF2D maxSpeed = null,
                                                        float maxW = -1, float maxAlfa = -1)
        {
            if (path.Count < 2) return new SingleWirelessCommand();

            var config = ControlConfig.Default;
            var framePeriod = MergerTrackerConfig.Default.FramePeriod;

            if (maxAccel == null) maxAccel = config.MaxAccel;
            if (maxSpeed == null) maxSpeed = config.MaxSpeed;
            if (maxW <= 0f) maxW = config.MaxW;
            if (maxW <= 0f) maxAlfa = config.MaxAlfa;

            var init = path[path.Count - 1];
            var goal = path[path.Count - 2];

            float pathLength = 0f;
            if (path.Count > 2)
                goal = FindTarget(path, targetAngle, ref pathLength);

            var d = goal.Location.Sub(init.Location);
            var v0 = init.Speed;
            var w0 = init.AngularSpeed;
            var vf = goal.Speed != null ? goal.Speed : VectorF2D.Zero;
            var dAng = MathHelper.AngleMod(targetAngle - init.Angle);

            aTunner.UpdateCoefs(init, goal, targetAngle);

            if (MathF.Abs(d.X) < config.MinDistanceTresh)
                d.X = 0f;
            if (MathF.Abs(d.Y) < config.MinDistanceTresh)
                d.Y = 0f;
            if (MathF.Abs(dAng) < config.MinAngleTresh)
                dAng = 0f;

            float time = 0;
            float timeAng = 0f, alfa = 0f, factor = 0f;
            int counter = 0;

            var vFinal = ComputeMotion2D(d, v0, vf, maxSpeed, maxAccel, ref time);

            while (counter < config.MaxSearchAngleIterations)
            {
                factor += 0.1f;
                ComputeMotion1D(config.WAccuercy, -dAng, w0, 0, maxAlfa * factor, maxW, config.AlfaFactor, ref alfa,
                                ref timeAng);
                if (timeAng < time)
                    break;
                counter++;
            }
            var wFinal = MathHelper.BoundAbsF(w0 + alfa * framePeriod, maxW);
            FineTune(d, v0, dAng, w0, path.Count > 2, pathLength, ref vFinal, ref wFinal);

            vFinal = vFinal.ToRobotCoordinate(init.Angle);

            return new() { Vx = vFinal.X, Vy = vFinal.Y, W = wFinal };
        }

        private VectorF2D ComputeMotion2D(VectorF2D d, VectorF2D v0, VectorF2D vf, VectorF2D maxSpeed, VectorF2D maxA,
                                    ref float time)
        {
            int counter = 0;
            float u = MathF.PI / 2f, du = -MathF.PI / 2f;
            float vxMax = 0f, vyMax = 0f, timeX = 0f, timeY = 0f, ax = 0f, ay = 0f;
            var config = ControlConfig.Default;
            float framePeriod = MergerTrackerConfig.Default.FramePeriod;

            while (counter < config.MaxSearchPosIterations)
            {
                du *= 0.5f;
                var alpha = u + du;
                var axMax = MathF.Sin(alpha) * maxA.X;
                var ayMax = MathF.Cos(alpha) * maxA.Y;

                vxMax = MathF.Sin(alpha) * maxSpeed.X;
                vyMax = MathF.Cos(alpha) * maxSpeed.Y;
                ComputeMotion1D(config.Accuercy, -d.X, v0.X, vf.X, axMax, vxMax, config.AccelFactor, ref ax, ref timeX);
                ComputeMotion1D(config.Accuercy, -d.Y, v0.Y, vf.Y, ayMax, vyMax, config.AccelFactor, ref ay, ref timeY);

                if (timeX - timeY <= 0)
                    u = alpha;

                counter++;
            }
            time = MathF.Max(timeX, timeY);

            var aFinal = new VectorF2D(ax, ay);
            var vFinal = v0.Add(aFinal.Scale(framePeriod));
            vFinal.X = MathHelper.BoundAbsF(vFinal.X, vxMax);
            vFinal.Y = MathHelper.BoundAbsF(vFinal.Y, vyMax);

            return vFinal;
        }
        private void ComputeMotion1D(float accuercyCoef, float x0, float v0, float v1,
                               float aMax, float vMax, float aFactor,
                               ref float trajAccel, ref float trajTime)
        {
            // First check to see if nothing needs to be done...
            if (MathHelper.EqualFloat(x0, 0) && MathHelper.EqualFloat(v0, v1))
            {
                trajAccel = 0; trajTime = 0;
                return;
            }
            var framePeriod = MergerTrackerConfig.Default.FramePeriod;

            // Need to do some motion.
            aMax /= aFactor;

            var timeToV1 = MathF.Abs(v0 - v1) / aMax;  // Por que?
            var xToV1 = MathF.Abs((v0 + v1) / 2.0f) * timeToV1; //

            var period = accuercyCoef * framePeriod; // Minimum time that vision feedback can be used for precise motion

            v1 = MathHelper.CopySign(v1, -x0);
            // state 0
            if (v0 * x0 > 0 || (MathF.Abs(v0) > MathF.Abs(v1) && xToV1 > MathF.Abs(x0)))
            {
                // Time to reach goal after stopping + Time to stop.
                float timeToStop = MathF.Abs(v0) / aMax;
                float xToStop = v0 * v0 / (2f * aMax);

                ComputeMotion1D(accuercyCoef, x0 + MathHelper.CopySign(xToStop, v0), 0, v1, aMax * aFactor,
                                vMax, aFactor, ref trajAccel, ref trajTime);
                trajTime += timeToStop;

                // Decelerate
                if (trajTime < period)
                    trajAccel = ComputeStop(v0, aMax * aFactor, framePeriod);
                else if (timeToStop < period)
                    trajAccel = timeToStop / period * -MathHelper.CopySign(aMax * aFactor, v0) +
                  (1f - timeToStop / period) * trajAccel;
                else trajAccel = -MathHelper.CopySign(aMax * aFactor, v0);
                return;
            }
            // At this point we have two options.  We can maximally accelerate
            // and then maximally decelerate to hit the target.  Or we could
            // find a single acceleration that would reach the target with zero
            // velocity.  The later is useful when we are close to the target
            // where the former is less stable.

            // OPTION 1
            // This computes the amount of time to accelerate before decelerating.
            float tA, tAccel, tDecel;
            if (MathF.Abs(v0) > MathF.Abs(v1))
            {
                tA = (MathF.Sqrt((3f * v1 * v1 + v0 * v0) / 2.0f - MathF.Abs(v0 * v1) + MathF.Abs(x0) * aMax)
                      - MathF.Abs(v0)) / aMax;
                //t_a = (Math.Sqrt((v0 * v0 + v1 * v1) / 2.0 + Math.Abs(x0) * a_max)
                //   - Math.Abs(v0)) / a_max;

                if (tA < 0.0f) tA = 0;
                tAccel = tA;
                tDecel = tA + timeToV1;
            }
            else if (xToV1 > MathF.Abs(x0))
            {
                tA = (MathF.Sqrt(v0 * v0 + 2f * aMax * MathF.Abs(x0)) - MathF.Abs(v0)) / aMax;
                tAccel = tA;
                tDecel = 0f;
            }
            else
            {
                //    t_a = (sqrt((3*v0*v0 + v1*v1) / 2.0 - fabs(v0 * v1) + fabs(x0) * a_max) 
                //  - fabs(v1)) / a_max;
                tA = (MathF.Sqrt((v0 * v0 + v1 * v1) / 2f + MathF.Abs(x0) * aMax) - MathF.Abs(v1)) / aMax;

                if (tA < 0f) tA = 0;

                tAccel = tA + timeToV1;
                tDecel = tA;
            }
            // OPTION 2
            float aToV1AtX0 = (v0 * v0 - v1 * v1) / (2f * MathF.Abs(x0));
            float tToV1AtX0 = (-MathF.Abs(v0) + MathF.Sqrt(v0 * v0 + 2f * MathF.Abs(aToV1AtX0) * MathF.Abs(x0)))
                              / MathF.Abs(aToV1AtX0);
            // We follow OPTION 2 if t_a is less than a FRAME_PERIOD making it
            // difficult to transition to decelerating and stopping exactly.

            if (ControlConfig.Default.EnableMotion1DOp2 && aToV1AtX0 < aMax && aToV1AtX0 > 0f && tToV1AtX0 < 2f * framePeriod)
            {
                // OPTION 2
                // Use option 1 time, even though we're not following it.
                trajTime = tAccel + tDecel; ;
                // Target acceleration to stop at x0.
                trajAccel = -MathHelper.CopySign(aToV1AtX0, v0);
                return;
            }
            else
            {
                // OPTION 1
                // Time to accelerate and decelerate.
                trajTime = tAccel + tDecel;
                // timeAcc = 0.6;
                // If the acceleration time would get the speed above v_max, then
                //  we need to add time to account for cruising at max speed.
                if (tAccel * aMax + Math.Abs(v0) > vMax)
                {
                    var t = vMax - (aMax * tAccel + MathF.Abs(v0));
                    trajTime += ((t * t) / (aMax * vMax));
                }
                // Accelerate (unless t_accel is less than FRAME_PERIOD, then set
                // acceleration to average acceleration over the period.)
                if (tAccel < period && MathHelper.EqualFloat(tDecel, 0f))
                    trajAccel = MathHelper.CopySign(aMax * aFactor, -x0);
                else if (tAccel < period && tDecel > 0.0)
                    trajAccel = ComputeStop(v0, aMax * aFactor, framePeriod);
                else if (tAccel < period)
                    trajAccel = MathHelper.CopySign((2f * tAccel / (period) - 1) * aMax * aFactor, v0);
                else
                    trajAccel = MathHelper.CopySign(aMax * aFactor, -x0);

            }

        }
        public float ComputeStop(float v, float maxA, float framePeriod)
        {
            if (MathF.Abs(v) > maxA * framePeriod) return MathHelper.CopySign(maxA, -v);// Math.Abs(max_a) * Math.Sign(-v);
            else return -v / framePeriod;
        }
        private void FineTune(VectorF2D d, VectorF2D v0, float dAng, float w0, bool hasPath, float pathLen,
                                    ref VectorF2D v, ref float w)
        {
            var config = ControlConfig.Default;
            var framePeriod = MergerTrackerConfig.Default.FramePeriod;
            if ((d.X < config.TunningDist && !hasPath) || (hasPath && pathLen < config.TunningDist))
            {
                var _vx = aTunner.Tune(d.X, PIDType.X);
                var _ax = MathHelper.BoundAbsF((_vx - v0.X) / framePeriod, config.MaxPosTunningAccel);
                inFineTuneX = true;
                v.X = v0.X + _ax * framePeriod;
            }
            else
            {
                if (inFineTuneX)
                    aTunner.Check4CollisionReset(PIDType.X);
                aTunner.Reset(PIDType.X);
                inFineTuneX = false;
            }

            if ((d.Y < config.TunningDist && !hasPath) || (hasPath && pathLen < config.TunningDist))
            {
                var _vy = aTunner.Tune(d.Y, PIDType.Y);
                var _ay = MathHelper.BoundAbsF((_vy - v0.X) / framePeriod, config.MaxPosTunningAccel);
                inFineTuneY = true;
                v.Y = v0.Y + _ay * framePeriod;
            }
            else
            {
                if (inFineTuneY)
                    aTunner.Check4CollisionReset(PIDType.Y);
                aTunner.Reset(PIDType.Y);
                inFineTuneY = false;
            }
            if (dAng < config.TunningAngle)
            {
                var _w = aTunner.Tune(dAng, PIDType.W);
                var _alfa = MathHelper.BoundAbsF((_w - w0) / framePeriod, config.MaxAngleTunningAccel);
                inFineTuneW = true;
                w = w0 + _alfa * framePeriod;
            }
            else
            {
                if (inFineTuneW)
                    aTunner.Check4CollisionReset(PIDType.W);
                aTunner.Reset(PIDType.W);
                inFineTuneW = false;
            }
        }
        private VectorF2D PathTrajectorySpeed(List<SingleObjectState> path, VectorF2D target, float v0, float vMax,
                                              float aMax, float pl, int idx)
        {
            float dAccel = 0, dCruiz = 0, dDecel = 0, vSize = 0;
            float dc = pl - ((2f * vMax * vMax - v0 * v0) / (2f * aMax));

            if (dc < 0)
            {
                var v = MathF.Sqrt(aMax * pl + v0 * v0 / 2f);
                dAccel = (v * v - v0 * v0) / (2f * aMax);
                dDecel = v * v / (2f * aMax);
                dCruiz = 0;
            }
            else
            {
                dAccel = (vMax * vMax - v0 * v0) / (2f * aMax);
                dDecel = vMax * vMax / (2f * aMax);
                dCruiz = MathF.Max(pl - dAccel - dDecel, 0f);
            }
            var dr = path[path.Count - 1].Location.Distance(target);
            if (dr < dAccel)
                vSize = MathF.Sqrt(v0 * v0 + 2f * aMax * dr);
            else if (dr < dAccel + dCruiz)
                vSize = vMax;
            else
            {
                var tmpd = dAccel + dCruiz + dDecel - dr;
                vSize = MathF.Sqrt(2f * aMax * tmpd);
            }

            int i = idx + 1;
            int endIdx = Math.Max(i - ControlConfig.Default.PathTrajectorySpeedLength, 0);
            float s = 0f;
            float d0 = 0f;
            for (int k = i + 1; k < path.Count - 1; k++)
                d0 += path[k].Location.Distance(path[k + 1].Location);

            for (int j = i; j > endIdx; j--)
            {
                var v1 = path[j].Location.Sub(path[j + 1].Location);
                var v2 = path[j - 1].Location.Sub(path[j].Location);
                var theta = v1.SmallestAngleBetweenInRadians(v2);
                d0 += path[j].Location.Distance(path[j + 1].Location);
                s += (theta / d0 / 2f);
            }
            var init2target = target - path[path.Count - 1].Location;
            var alfa = init2target.AngleInRadians();
            var cos = MathF.Abs(MathF.Cos(s));
            return VectorF2D.FromAngleSize(alfa, cos * vSize);

        }
        private SingleObjectState FindTarget(List<SingleObjectState> path, float angle, ref float pathLength)
        {
            var config = ControlConfig.Default;
            float maxPathLength = ControlConfig.Default.MaxPathLength;
            pathLength = 0;
            for (int j = path.Count - 2; j >= 0; j--)
            {
                pathLength += path[j].Location.Distance(path[j + 1].Location);
            }
            var distInterpol = MathHelper.BoundF(pathLength / maxPathLength,
                                                 config.DisInterpolationMin,
                                                 config.DisInterpolationMax)
                                                 * config.PathLengthInterpolationCoef;

            float sumD = 0f;
            int i = path.Count - 2;
            for (; i >= 0; i--)
            {
                var d = path[i].Location.Distance(path[i + 1].Location);
                if (d + sumD <= distInterpol)
                    sumD += d;
                else
                    break;
            }
            i = Math.Max(i, 0);
            var idx = Math.Max(i - 1, 0);

            SingleObjectState goal = new SingleObjectState();
            goal.Location = (VectorF2D)VectorF2D.Interpolate(path[path.Count - 2].Location,
                                                             path[i].Location,
                                                             config.DistanceInterpolationCoef);
            goal.Speed = PathTrajectorySpeed(path, goal.Location, path[path.Count - 1].Speed.Length(),
                                             config.MaxSpeed.X, config.MaxAccel.X / config.AccelFactor,
                                             pathLength, idx);
            return goal;
        }
    }
}