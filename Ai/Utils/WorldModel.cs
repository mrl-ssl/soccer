using System.Collections.Generic;
using MRL.SSL.Common;
using ProtoBuf;

namespace MRL.SSL.Ai.Utils
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
        public Dictionary<int, SingleObjectState> Teammates { get; set; }

        [ProtoMember(5)]
        public Dictionary<int, SingleObjectState> Opponents { get; set; }

        [ProtoMember(6)]
        public SingleObjectState Ball { get; set; }

        [ProtoMember(7)]
        public ObservationModel Observations { get; set; }


        [ProtoMember(8)]
        public RobotCommands Commands { get; set; }

        public MRL.SSL.Ai.MergerTracker.Tracker Tracker { get; set; }
        public WorldModel()
        {
            Teammates = new Dictionary<int, SingleObjectState>();
            Opponents = new Dictionary<int, SingleObjectState>();
            Observations = new ObservationModel();
        }
    }
}