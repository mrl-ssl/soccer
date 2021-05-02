
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public class RobotObservationMeta : ObservationMeta
    {



        public RobotObservationMeta() : base()
        {

        }
        public RobotObservationMeta(SingleObjectState state) : base(state)
        {
        }
        public RobotObservationMeta(Observation v) : base(v)
        {

        }
        public RobotObservationMeta(Observation v, SingleObjectState state) : base(v, state)
        {
        }
    }

}