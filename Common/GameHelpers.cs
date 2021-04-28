using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
{
    public static class GameHelpers
    {
        public static bool IsInField(VectorF2D location, float margin)
        {
            var config = FieldConfig.Default;

            if (location.X < config.OppGoalCenter.X - margin || location.X > config.OurGoalCenter.X + margin)
                return false;
            if (location.Y < config.OurRightCorner.Y - margin || location.Y > config.OurLeftCorner.Y + margin)
                return false;
            return true;
        }
    }
}