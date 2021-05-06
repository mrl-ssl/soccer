
using MRL.SSL.Common;
using MRL.SSL.Common.Math;
using ProtoBuf;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;

namespace MRL.SSL.Ai.Utils
{

    [ProtoContract]
    public enum OccludeType { Visible, MaybeOccluded, Occluded };

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
        public ObservationMeta(Observation v, double time)
        {
            vision = v;
            // if (v != null)
            //     time = v.Time;
        }
        public ObservationMeta(Observation v, double time, SingleObjectState state)
        {
            vision = v;
            // if (v != null)
            //     time = v.Time;
            viewState = state;
        }
    }
    [ProtoContract]
    public class BallObservationMeta : ObservationMeta
    {

        [ProtoMember(1)]
        public VectorF2D OccludingOffset { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public OccludeType Occluded { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public int OccludingTeam { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public int OccludingId { get; set; }

        public bool HasCollision { get; set; }
        public MatrixF Covariances { get; set; }
        public int CollidedTeam { get; set; }
        public int CollidedRobot { get; set; }

        public BallObservationMeta() : base()
        {

        }
        public BallObservationMeta(SingleObjectState state) : base(state)
        {
        }
        public BallObservationMeta(Observation v, double time) : base(v, time)
        {

        }
        public BallObservationMeta(Observation v, double time, SingleObjectState state) : base(v, time, state)
        {
        }
    }
    [ProtoContract]
    public class RobotObservationMeta : ObservationMeta
    {

        public RobotObservationMeta() : base()
        {

        }
        public RobotObservationMeta(SingleObjectState state) : base(state)
        {
        }
        public RobotObservationMeta(Observation v, double time) : base(v, time)
        {

        }
        public RobotObservationMeta(Observation v, double time, SingleObjectState state) : base(v, time, state)
        {
        }
    }
}