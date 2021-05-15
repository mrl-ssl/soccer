using System.Collections.Generic;
using MRL.SSL.Common;
using ProtoBuf;

namespace MRL.SSL.Ai.Utils
{
    [ProtoContract]
    public class WorldModel
    {
        [ProtoMember(1, IsRequired = true)]
        public bool OurMarkerIsYellow { get; set; }
        [ProtoMember(2, IsRequired = true)]
        public bool FieldIsInverted { get; set; }
        [ProtoMember(3)]
        public uint? GoalieID { get; set; }

        [ProtoMember(4)]
        public IDictionary<int, SingleObjectState> Teammates { get; set; }

        [ProtoMember(5)]
        public IDictionary<int, SingleObjectState> Opponents { get; set; }

        [ProtoMember(6)]
        public SingleObjectState Ball { get; set; }

        [ProtoMember(7)]
        public ObservationModel Observations { get; set; }


        [ProtoMember(8)]
        public RobotCommands Commands { get; set; }

        [ProtoMember(9, IsRequired = true)]
        public GameStatus Status { get; set; }
        [ProtoMember(10)]
        public uint? OppGoalieID { get; set; }

        public MRL.SSL.Ai.MergerTracker.Tracker Tracker { get; set; }


        public WorldModel()
        {
            Teammates = new Dictionary<int, SingleObjectState>();
            Opponents = new Dictionary<int, SingleObjectState>();
            Observations = new ObservationModel();
        }
    }
}