using System;
using MRL.SSL.Ai.Engine;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;
using MRL.SSL.Common;
using System.Drawing;
using MRL.SSL.Common.Utils;

namespace MRL.SSL.Ai.SkillBook
{
    public class GotoPointSkill : SkillBase
    {

        public Func<SingleWirelessCommand> Go(GameStrategyEngine engine, WorldModel model, int robotId, VectorF2D target, bool avoidBall, bool stopBall, bool avoidOurs, bool avoidOpps, bool avoidOurZone, bool avoidOppZone)
        {
            planner.SetObstacles(model, robotId, avoidBall, stopBall, avoidOurs, avoidOpps, avoidOurZone, avoidOppZone);
            return () =>
            {

                var p = planner.GetPath(model, robotId, new SingleObjectState(target));
                Drawings.AddPath(p, Color.Red);
                return new SingleWirelessCommand();
            };
        }
    }
}