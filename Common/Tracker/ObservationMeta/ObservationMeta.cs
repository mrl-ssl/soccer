
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    [ProtoInclude(4, typeof(BallObservationMeta))]
    [ProtoInclude(5, typeof(RobotObservationMeta))]
    public class ObservationMeta
    {
        protected Observation vision;
        protected int notSeen;
        protected SingleObjectState viewState { get; set; }

        [ProtoMember(1)]
        public Observation Vision { get => vision; set { vision = value; } }

        [ProtoMember(2, IsRequired = true)]
        public int NotSeen { get => notSeen; set { notSeen = value; } }

        [ProtoMember(3)]
        public SingleObjectState ViewState { get => viewState; set { viewState = value; } }

        // [ProtoMember(4)]
        public double Time { get => time; set { time = value; } }

        protected double time;

        public ObservationMeta()
        {
        }
        public ObservationMeta(SingleObjectState state)
        {
            viewState = state;
        }
        public ObservationMeta(Observation v)
        {
            vision = v;
            if (v != null)
                time = v.Time;
        }
        public ObservationMeta(Observation v, SingleObjectState state)
        {
            vision = v;
            if (v != null)
                time = v.Time;
            viewState = state;
        }
    }
}