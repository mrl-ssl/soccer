using System.Collections.Generic;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.Utils;

namespace MRL.SSL.Ai.MotionPlanner
{
    public class PathPlanner
    {
        ERRT eRRT;
        Obstacles obs;
        public PathPlanner()
        {
            eRRT = new ERRT(false);
            obs = new Obstacles();
        }
        public void SetObstacles(WorldModel model, int robotId, bool avoidBall, bool stopBall, bool avoidOurs, bool avoidOpps, bool avoidOurZone, bool avoidOppZone)
        {
            obs.Clear();
            if (avoidBall)
                obs.Add(new BallObstacle(model.Ball));
            if (avoidOurs)
                foreach (var item in model.Teammates.Keys)
                {
                    if (robotId == item) continue;
                    obs.Add(new OurRobotObstacle(model.Teammates[item], item));
                }
            if (avoidOpps)
                foreach (var item in model.Opponents.Keys)
                {
                    obs.Add(new OppRobotObstacle(model.Teammates[item], item));
                }
            if (avoidOurZone)
                obs.Add(new OurZoneObstacle());
            if (avoidOppZone)
                obs.Add(new OppZoneObstacle());

            if (stopBall)
                obs.Add(new CircleObstacle(model.Ball, GameConfig.Default.StopBallRadi));
        }
        public List<SingleObjectState> GetPath(WorldModel model, int robotId, VectorF2D target)
        {
            var path = eRRT.FindPath(model.Teammates[robotId], new SingleObjectState(target), obs, MergerTrackerConfig.Default.OurRobotRadius);
            return path;
        }
    }
}