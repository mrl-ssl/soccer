
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
{
    public class GameEvent
    {
        public int BlueScore { get; set; }
        public int YellowScore { get; set; }
        public int TimeOfstage { get; set; }
        public int YellowGoalie { get; set; }
        public int BlueGoalie { get; set; }
        public VectorF2D BallPlacementPosition { get; set; }

        public GameEvent()
        {
        }
    }
}