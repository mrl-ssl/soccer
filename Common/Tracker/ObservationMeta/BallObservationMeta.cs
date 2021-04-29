using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
{
    public class BallObservationMeta : ObservationMeta
    {

        public VectorF2D OccludingOffset { get; set; }
        public OccludeType Occluded { get; set; }
        public int OccludingTeam { get; set; }
        public int OccludingId { get; set; }
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