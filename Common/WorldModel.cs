using System.Collections.Generic;

namespace MRL.SSL.Common
{
    public class WorldModel
    {

        public bool OurMarkerIsYellow { get; set; }
        public bool FieldIsInverted { get; set; }
        public int? GoalieID { get; set; }
        public IDictionary<int, SingleObjectState> OurRobots { get; set; }
        public IDictionary<int, SingleObjectState> Opponents { get; set; }
        public IDictionary<int, RawObjectState> OurRobotsRaw { get; set; }
        public IDictionary<int, RawObjectState> OpponentsRaw { get; set; }
        public SingleObjectState BallState { get; set; }
        public RawObjectState BallRaw { get; set; }
        public Dictionary<int, RawObjectState> OtherBalls { get; set; }
        public double TimeOfCapture { get; set; }
        public WorldModel()
        {
            OurRobots = new Dictionary<int, SingleObjectState>();
            Opponents = new Dictionary<int, SingleObjectState>();
            OurRobotsRaw = new Dictionary<int, RawObjectState>();
            OpponentsRaw = new Dictionary<int, RawObjectState>();
            OtherBalls = new Dictionary<int, RawObjectState>();

        }

    }
}