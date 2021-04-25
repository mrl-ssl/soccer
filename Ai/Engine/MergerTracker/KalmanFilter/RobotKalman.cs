using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;

namespace MRL.SSL.Ai.MergerTracker
{
    public class RobotKalman : KalmanBase
    {
        public RobotKalman() : base(7, 3, MergerTrackerConfig.Default.FramePeriod)
        {
        }

        public override MatrixF A(bool visionProblem, MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF f(bool visionProblem, MatrixF x, ref MatrixF I, bool checkCollision)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF h(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF H(MatrixF x)
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

        public override MatrixF V(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF W(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override void Observe(double timestamp, bool visionProblem, bool checkCollision)
        {
            throw new System.NotImplementedException();
        }
        public override void Update(MatrixF z, bool visionProblem, bool checkCollision)
        {
            throw new System.NotImplementedException();
        }

    }
}