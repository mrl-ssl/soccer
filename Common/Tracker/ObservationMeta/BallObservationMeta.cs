using MRL.SSL.Common.Math;
using ProtoBuf;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public class BallObservationMeta : ObservationMeta
    {


        [ProtoMember(1)]
        public VectorF2D OccludingOffset { get; set; }

        [ProtoMember(2)]
        public OccludeType Occluded { get; set; }

        [ProtoMember(3)]
        public int OccludingTeam { get; set; }

        [ProtoMember(4)]
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
        public BallObservationMeta(Observation v) : base(v)
        {

        }
        public BallObservationMeta(Observation v, SingleObjectState state) : base(v, state)
        {
        }
    }

}