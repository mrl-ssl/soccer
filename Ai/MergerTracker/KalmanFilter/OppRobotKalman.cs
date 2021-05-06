using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;
using SMath = System.Math;
using System.Linq;
using System;
using MRL.SSL.Ai.Utils;

namespace MRL.SSL.Ai.MergerTracker
{
    public class OppRobotKalman : RobotKalman
    {
        public OppRobotKalman() : base(6, 3, 3, MergerTrackerConfig.Default.FramePeriod)
        {
        }

        public override void Observe(Observation obs, double timestamp)
        {
            var fx = (xs.Count > 0 && xs.First().Rows > 2) ? xs.First() : null;

            if (SMath.Abs(timestamp - time) > MergerTrackerConfig.Default.MaxPredictionTime
                || (fx != null && (float.IsNaN(fx[0, 0]) || float.IsNaN(fx[1, 0]) || float.IsNaN(fx[2, 0]))))
                Reset();


            if (_reset)
            {
                if (obs.Confidence <= 0.0) return;

                var _p = matrixBuilder.DenseZero(stateNum);
                var _x = matrixBuilder.DenseZero(stateNum, 1);

                _p[0, 0] = MergerTrackerConfig.Default.OpponentPositionVariance;
                _p[1, 1] = MergerTrackerConfig.Default.OpponentPositionVariance;
                _p[2, 2] = MergerTrackerConfig.Default.OpponentAngleVariance;
                _p[3, 3] = 0f; // 0m/s
                _p[4, 4] = 0f; // 0m/s
                _p[5, 5] = 0f;

                _x[0, 0] = obs.Location.X;
                _x[1, 0] = obs.Location.Y;
                _x[2, 0] = obs.Angle - MathF.PI / 2f;

                Initial(timestamp, _x, _p);

                _reset = false;
            }
            else if (timestamp > time)
            {
                Tick(timestamp - time);

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

            float cos_theta = MathF.Cos(theta), sin_theta = MathF.Sin(theta);
            float step = (float)stepSize;

            _A[0, 2] = step * (vpar * -sin_theta + vperp * -cos_theta);
            _A[0, 3] = cos_theta * step;
            _A[0, 4] = -sin_theta * step;
            _A[0, 6] = -step * (vpar * cos_theta + vperp * -sin_theta);
            _A[1, 2] = step * (vpar * cos_theta + vperp * -sin_theta);
            _A[1, 3] = sin_theta * step;
            _A[1, 4] = cos_theta * step;
            _A[1, 6] = -step * (vpar * sin_theta + vperp * cos_theta);
            _A[2, 5] = step;
            _A[2, 6] = -step * vtheta;
            _A[3, 3] = MergerTrackerConfig.Default.OpponentVelocityNextStepCovariance;
            _A[4, 4] = MergerTrackerConfig.Default.OpponentVelocityNextStepCovariance;
            _A[5, 5] = MergerTrackerConfig.Default.OpponentVelocityNextStepCovariance;

            return _A;
        }

        public override MatrixF f(in MatrixF x, ref MatrixF I)
        {
            float _vpar = x[3, 0], _vperp = x[4, 0], _vtheta = x[5, 0];

            float avgVpar, avgVperp, avgVtheta, avgTheta;
            float avgWeight = 0.5f;
            float step = (float)stepSize;

            if (MergerTrackerConfig.Default.RobotUseAverageInPropagation)
            {
                avgVpar = avgWeight * _vpar + (1f - avgWeight) * x[3, 0];
                avgVperp = avgWeight * _vperp + (1f - avgWeight) * x[4, 0];
                avgVtheta = avgWeight * _vtheta + (1f - avgWeight) * x[5, 0];
                avgTheta = avgWeight * x[2, 0];

                x[2, 0] += step * avgVtheta;

                avgTheta += (1f - avgWeight) * x[2, 0];

                x[0, 0] += step * (avgVpar * MathF.Cos(avgTheta) - avgVperp * MathF.Sin(avgTheta));
                x[1, 0] += step * (avgVpar * MathF.Sin(avgTheta) + avgVperp * MathF.Cos(avgTheta));
            }
            else
            {
                x[0, 0] += step * (x[3, 0] * MathF.Cos(x[2, 0]) - x[4, 0] * MathF.Sin(x[2, 0]));
                x[1, 0] += step * (x[3, 0] * MathF.Sin(x[2, 0]) + x[4, 0] * MathF.Cos(x[2, 0]));
                x[2, 0] += step * x[5, 0];
            }
            return x;
        }

        public override MatrixF Q(MatrixF x)
        {

            _Q[0, 0] = MergerTrackerConfig.Default.OpponentVelocityVariance;
            _Q[1, 1] = MergerTrackerConfig.Default.OpponentVelocityVariance;
            _Q[2, 2] = MergerTrackerConfig.Default.OpponentAngularVelocityVariance;

            return _Q;
        }

        public override MatrixF R(MatrixF x)
        {
            _R[0, 0] = MergerTrackerConfig.Default.OpponentPositionVariance;
            _R[1, 1] = MergerTrackerConfig.Default.OpponentPositionVariance;
            _R[2, 2] = MergerTrackerConfig.Default.OpponentAngleVariance;

            return _R;
        }

        public override VectorF2D Position(double time)
        {
            var x = Predict(time);

            return new VectorF2D(x[0, 0], x[1, 0]);
        }

        public override float Direction(double time)
        {
            var x = Predict(time);
            return x[2, 0];
        }

        public override VectorF2D RawVelocity(double time)
        {
            var x = Predict(time);

            return new VectorF2D(x[3, 0], x[4, 0]);
        }

        public override VectorF2D Velocity(double time)
        {
            var x = Predict(time);

            float a = x[2, 0];
            float c = MathF.Cos(a);
            float s = MathF.Sin(a);

            float vx = x[3, 0];
            float vy = x[4, 0];

            return new VectorF2D(c * vx - s * vy, s * vx + c * vy);
        }
        public override float AngularVelocity(double time)
        {
            var x = Predict(time);
            return x[5, 0];
        }

    }
}