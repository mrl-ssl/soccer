using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public class ObservationModel
    {
        [ProtoMember(1)]
        public Dictionary<int, RobotObservationMeta> OurRobots { get; set; }

        [ProtoMember(2)]
        public Dictionary<int, RobotObservationMeta> Opponents { get; set; }

        [ProtoMember(3)]
        public BallObservationMeta Ball { get; set; }

        [ProtoMember(4)]
        public Dictionary<int, Observation> OtherBalls { get; set; }

        [ProtoMember(5)]
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