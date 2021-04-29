
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
{
    public abstract class RobotKalman : KalmanBase
    {
        protected bool visionProblem;
        public bool VisionProblem
        {
            get { return visionProblem; }
            set { visionProblem = value; }
        }

        protected RobotKalman(int _stateN, int _obsN, int propNum, double _stepSize) : base(_stateN, _obsN, propNum, _stepSize)
        {
            if (MergerTrackerConfig.Default.PrintRobotKalmaError)
                predictionLookahead = MergerTrackerConfig.Default.Latency;
            visionProblem = false;
        }

        public abstract VectorF2D Position(double time);
        public abstract VectorF2D Velocity(double time);
        // return the velocity un-rotated
        public abstract VectorF2D RawVelocity(double time);
        public abstract float Direction(double time);
        public abstract float AngularVelocity(double time);

    }
}