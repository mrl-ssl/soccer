using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;

namespace MRL.SSL.Common
{
    public class OppRobotKalman : RobotKalman
    {
        public OppRobotKalman() : base(6, 3, 3, MergerTrackerConfig.Default.FramePeriod)
        {
        }

        public override void Observe(Observation obs)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF A(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF f(MatrixF x, ref MatrixF I)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF Q(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF R(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override VectorF2D Position(double time)
        {
            throw new System.NotImplementedException();
        }

        public override float Direction(double time)
        {
            throw new System.NotImplementedException();
        }

        public override VectorF2D RawVelocity(double time)
        {
            throw new System.NotImplementedException();
        }

        public override VectorF2D Velocity(double time)
        {
            throw new System.NotImplementedException();
        }
        public override float AngularVelocity(double time)
        {
            throw new System.NotImplementedException();
        }

    }
}