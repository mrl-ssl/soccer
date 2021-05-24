using System.Collections.Generic;
using MRL.SSL.Common.Configuration;

namespace MRL.SSL.Common.Utils
{
    public class Obstacles
    {
        Dictionary<ObstacleType, List<ObstacleBase>> obstacles;

        public Obstacles()
        {
            obstacles = new Dictionary<ObstacleType, List<ObstacleBase>>
            {
                { ObstacleType.Ball, new List<ObstacleBase>(2) },
                { ObstacleType.OppRobot, new List<ObstacleBase>(MergerTrackerConfig.Default.MaxTeamRobots) },
                { ObstacleType.OurRobot, new List<ObstacleBase>(MergerTrackerConfig.Default.MaxTeamRobots) },
                { ObstacleType.OppZone, new List<ObstacleBase>(1) },
                { ObstacleType.OurZone, new List<ObstacleBase>(1) },
                { ObstacleType.Circle, new List<ObstacleBase>() },
                { ObstacleType.Rectangle, new List<ObstacleBase>() }
            };
        }

        /// <summary>
        /// Check there is specific obstacle type in given state and returns that.
        /// if there is not returns null
        /// </summary>
        /// <param name="s">target state</param>
        /// <param name="type">type of obstacle you want to check for</param>
        public ObstacleBase Meet(SingleObjectState s, ObstacleType type, float obstacleRadi, float margin = 0f)
        {
            foreach (var item in obstacles[type])
                if (item.Avoid && item.Meet(s, obstacleRadi, margin))
                    return item;
            return null;
        }

        /// <summary>
        /// Check there is obstacle from state a to b and returns that.
        /// if there is not returns null
        /// </summary>
        public ObstacleBase Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, Dictionary<ObstacleType, float> margins = null)
        {
            foreach (var type in obstacles.Keys)
            {
                float margin = (margins != null && margins.ContainsKey(type)) ? margins[type] : 0f;
                foreach (var item in obstacles[type])
                    if (item.Avoid && item.Meet(from, to, obstacleRadi, margin))
                        return item;
            }
            return null;
        }

        /// <summary>
        /// Clear all obstacles
        /// </summary>
        public void Clear()
        {
            foreach (var item in obstacles.Keys)
                obstacles[item].Clear();
        }

        /// <summary>
        /// Avoid all obstacles from given type
        /// (Call this before calling "Meet" to apply changes)
        /// </summary>
        /// <param name="type">type of obstacles you want to avoid from</param>
        /// <param name="avoid">set false to not avoid</param>
        public void AvoidAll(ObstacleType type, bool avoid = true)
        {
            foreach (var item in obstacles[type])
                item.Avoid = avoid;
        }

        /// <summary>
        /// Avoid from some robots
        /// (Call this before calling "Meet" to apply changes)
        /// </summary>
        /// <param name="robotsId">ids of robots you want to avoid from</param>
        /// <param name="ours">true if given ids are our robots ids</param>
        /// <param name="avoid">set false to not avoid from specific robots</param>
        public void AvoidSpecificRobots(List<int> robotsId, bool ours, bool avoid = true)
        {
            ObstacleType type = ours ? ObstacleType.OurRobot : ObstacleType.OppRobot;
            foreach (var item in obstacles[type])
                if (item is RobotObstacle robotObs && robotsId.Contains(robotObs.Id))
                    item.Avoid = avoid;
        }

        public void AddObstacle(ObstacleBase obstacle) => obstacles[obstacle.Type].Add(obstacle);

        public void RemoveObstacles(ObstacleType type) => obstacles[type].Clear();


    }
}
