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
        public bool AvoidBall { get; set; } = true;
        public bool StopBall { get; set; } = false;
        public bool AvoidOurs { get; set; } = true;
        public bool AvoidOpps { get; set; } = true;
        public bool AvoidOurZone { get; set; } = true;
        public bool AvoidOppZone { get; set; } = true;

        public Func<SingleWirelessCommand> Go(GameStrategyEngine engine, WorldModel model, int robotId, VectorF2D target,
                                              float targetAngle)
        {
            planner.SetObstacles(model, robotId, AvoidBall, StopBall, AvoidOurs, AvoidOpps, AvoidOurZone, AvoidOppZone);

            return () =>
            {
                var p = planner.GetPath(model, robotId, new SingleObjectState(target));

                Drawings.AddPath(p, Color.Red);
                Drawings.AddText(model.Teammates[robotId].Speed.ToString(), VectorF2D.Zero);
                return controller.CalculatePathSpeed(p, targetAngle);
            };
        }
    }
}