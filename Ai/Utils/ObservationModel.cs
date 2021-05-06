using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Ai.Utils
{
    [ProtoContract]
    public class ObservationModel
    {
        [ProtoMember(1)]
        public IDictionary<int, RobotObservationMeta> OurRobots { get; set; }

        [ProtoMember(2)]
        public IDictionary<int, RobotObservationMeta> Opponents { get; set; }

        [ProtoMember(3)]
        public BallObservationMeta Ball { get; set; }

        [ProtoMember(4)]
        public IList<Observation> OtherBalls { get; set; }

        // [ProtoMember(5)]
        public double TimeOfCapture { get; set; }
        public ObservationModel()
        {
            OurRobots = new Dictionary<int, RobotObservationMeta>();
            Opponents = new Dictionary<int, RobotObservationMeta>();
            Ball = null;
            OtherBalls = new List<Observation>();
        }

    }
}