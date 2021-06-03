using System;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.Utils;

namespace MRL.SSL.Ai.MotionPlanner
{
    public class AdaptiveTunner
    {
        VelocityCoefCalculaterBase[] vcc;
        PID[] pid;
        PIDCoef[] basePidCoefs;

        public AdaptiveTunner()
        {
            var config = ControlConfig.Default;
            var count = config.PIDModuleCount;
            pid = new PID[count];
            vcc = new VelocityCoefCalculaterBase[count];

            vcc[0] = new VelocityCoefCalculatorPos(config.MinPIDDistanceTresh, config.PosMinVelocityTresh,
                                                   config.PosCoefResetValue, config.PosResetCoefTresh, config.MinPosCoef);
            vcc[1] = new VelocityCoefCalculatorPos(config.MinPIDDistanceTresh, config.PosMinVelocityTresh,
                                                   config.PosCoefResetValue, config.PosResetCoefTresh, config.MinPosCoef);
            vcc[2] = new VelocityCoefCalculatorAngle(config.MinPIDAngleTresh, config.AngleMinVelocityTresh,
                                                     config.AngleCoefResetValue, config.AngleResetCoefTresh,
                                                     config.MinAngleCoef);
            for (int i = 0; i < count; i++)
                pid[i] = new PID();

            basePidCoefs = new PIDCoef[count];
            basePidCoefs[0] = new PIDCoef(config.Kp, config.Ki, config.Kd);
            basePidCoefs[1] = new PIDCoef(config.Kp, config.Ki, config.Kd);
            basePidCoefs[2] = new PIDCoef(config.AngleKp, config.AngleKi, config.AngleKd);
        }

        public void UpdateCoefs(SingleObjectState init, SingleObjectState target, float targetAngle)
        {
            vcc[0].UpdateVelocityCoefs(init.Location.X, init.Speed.X, target.Location.X);
            vcc[1].UpdateVelocityCoefs(init.Location.Y, init.Speed.Y, target.Location.Y);
            vcc[2].UpdateVelocityCoefs(init.Angle, init.AngularSpeed, targetAngle);
        }
        public float Tune(float d, PIDType type)
        {
            int idx = (int)type;

            pid[idx].Coef = basePidCoefs[idx] / vcc[idx].Coef;

            return -pid[idx].Calculate(d, 0);
        }
        public void Reset(PIDType type)
        {
            pid[(int)type].Reset();
        }

        public void Check4CollisionReset(PIDType type)
        {
            vcc[(int)type].Check4Reset();
        }
    }

}