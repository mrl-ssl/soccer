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
        public SingleObjectState BallState { get; set; }
        public ObservationModel Observations { get; set; }
        public Tracker Tracker { get; set; }
        public RobotCommands Commands { get; set; }

        public double TimeOfCapture { get; set; }
        public WorldModel()
        {
            OurRobots = new Dictionary<int, SingleObjectState>();
            Opponents = new Dictionary<int, SingleObjectState>();
            Observations = new ObservationModel();
        }
    }
}