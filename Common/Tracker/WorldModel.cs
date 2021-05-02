using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public class WorldModel
    {
        [ProtoMember(1)]
        public bool OurMarkerIsYellow { get; set; }
        [ProtoMember(2)]
        public bool FieldIsInverted { get; set; }
        [ProtoMember(3)]
        public int? GoalieID { get; set; }

        [ProtoMember(4)]
        public Dictionary<int, SingleObjectState> OurRobots { get; set; }

        [ProtoMember(5)]
        public Dictionary<int, SingleObjectState> Opponents { get; set; }

        [ProtoMember(6)]
        public SingleObjectState BallState { get; set; }

        [ProtoMember(7)]
        public ObservationModel Observations { get; set; }

        public Tracker Tracker { get; set; }

        [ProtoMember(8)]
        public RobotCommands Commands { get; set; }
        public WorldModel()
        {
            OurRobots = new Dictionary<int, SingleObjectState>();
            Opponents = new Dictionary<int, SingleObjectState>();
            Observations = new ObservationModel();
        }
    }
}