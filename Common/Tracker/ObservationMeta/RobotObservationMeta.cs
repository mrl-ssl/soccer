
namespace MRL.SSL.Common
{
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