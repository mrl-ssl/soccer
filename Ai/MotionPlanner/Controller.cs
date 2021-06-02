using System.Collections.Generic;
using MRL.SSL.Common.SSLWrapperCommunication;
using MRL.SSL.Common.Utils;

namespace MRL.SSL.Ai.MotionPlanner
{
    public class Controller
    {
        protected AdaptiveTunner aTunner;
        public Controller()
        {
            aTunner = new AdaptiveTunner();
        }

        public SingleWirelessCommand CalculatePathSpeed(List<SingleObjectState> path)
        {
            return new();
        }
    }
}