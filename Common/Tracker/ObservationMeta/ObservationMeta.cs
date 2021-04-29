
namespace MRL.SSL.Common
{
    public class ObservationMeta
    {
        public Observation vision { get; set; }
        public int NotSeen { get; set; }
        public SingleObjectState ViewState { get; set; }
        public double Time { get; set; }

        public ObservationMeta()
        {
        }
        public ObservationMeta(SingleObjectState state)
        {
            ViewState = state;
        }
        public ObservationMeta(Observation v)
        {
            vision = v;
            if (v != null)
                Time = v.Time;
        }
        public ObservationMeta(Observation v, SingleObjectState state)
        {
            vision = v;
            if (v != null)
                Time = v.Time;
            ViewState = state;
        }
    }
}