using System.Collections.Generic;

namespace MRL.SSL.Common
{
    public class ObservationModel
    {

        public IDictionary<int, RobotObservationMeta> OurRobots { get; set; }
        public IDictionary<int, RobotObservationMeta> Opponents { get; set; }
        public BallObservationMeta Ball { get; set; }
        public Dictionary<int, Observation> OtherBalls { get; set; }

        public double TimeOfCapture { get; set; }
        public ObservationModel()
        {
            OurRobots = new Dictionary<int, RobotObservationMeta>();
            Opponents = new Dictionary<int, RobotObservationMeta>();
            Ball = null;
            OtherBalls = new Dictionary<int, Observation>();
        }

    }
}