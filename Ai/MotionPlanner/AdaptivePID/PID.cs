using MRL.SSL.Common.Configuration;

namespace MRL.SSL.Ai.MotionPlanner
{
    public enum PIDType
    {
        X = 0,
        Y = 1,
        W = 2
    }
    public struct PIDCoef
    {
        float kp, ki, kd, lambda;
        public PIDCoef(float _kp, float _ki, float _kd, float _lambda)
        {
            kp = _kp;
            ki = _ki;
            kd = _kd;
            lambda = _lambda;
        }
        public PIDCoef(float _kp, float _ki, float _kd)
        {
            kp = _kp;
            ki = _ki;
            kd = _kd;
            lambda = 1;
        }
        public float Kp
        {
            get { return kp; }
            set { kp = value; }
        }

        public float Ki
        {
            get { return ki; }
            set { ki = value; }
        }

        public float Kd
        {
            get { return kd; }
            set { kd = value; }
        }

        public float Lambda
        {
            get { return lambda; }
            set { lambda = value; }
        }
        public static PIDCoef operator /(PIDCoef c, float d)
        {
            return new PIDCoef(c.kp / d, c.ki / d, c.kd / d, c.lambda);
        }
        public static PIDCoef operator *(PIDCoef c, float d)
        {
            return new PIDCoef(c.kp * d, c.ki * d, c.kd * d, c.lambda);
        }
        public static PIDCoef operator *(float d, PIDCoef c)
        {
            return new PIDCoef(c.kp * d, c.ki * d, c.kd * d, c.lambda); ;
        }
    }

    public class PID
    {
        private float _lastError, _integral, _difrential;
        private float dt;
        private PIDCoef coef;

        public PIDCoef Coef
        {
            get { return coef; }
            set { coef = value; }
        }

        bool first = true;

        private float err;

        public float Error
        {
            get { return err; }
        }
        public PID()
        {
            dt = MergerTrackerConfig.Default.FrameRate;
        }
        public float Calculate(float current, float desierd)
        {
            float error = desierd - current;
            if (first)
                _lastError = error;
            err = error;
            _difrential = (error - _lastError) * dt;
            _lastError = error;
            _integral *= coef.Lambda;
            _integral += error * dt;


            first = false;
            return error * coef.Kp + _integral * coef.Ki + _difrential * coef.Kd;
        }
        public void Reset()
        {
            _integral = 0;
            first = true;
        }
    }
}