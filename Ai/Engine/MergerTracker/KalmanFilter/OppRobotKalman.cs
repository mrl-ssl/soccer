using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;

namespace MRL.SSL.Ai.MergerTracker
{
    public class OppRobotKalman : RobotKalman
    {
        public OppRobotKalman() : base()
        {
        }

        public override MatrixF f(bool visionProblem, MatrixF x, ref MatrixF I, bool checkCollision)
        {
            throw new System.NotImplementedException();
        }

        public override void Observe(double timestamp, bool visionProblem, bool checkCollision)
        {
            throw new System.NotImplementedException();
        }


    }
}