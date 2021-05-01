using System.Collections.Generic;

namespace MRL.SSL.Common
{
    public class Obstacles
    {
        //These constance not important and just good for better performance
        private const int circleObstaclesInitSize = 100;
        private const int rectObstaclesInitSize = 100;
        private const int ourRobotObstaclesInitSize = 8;
        private const int oppRobotObstaclesInitSize = 8;
        private const int ballObstaclesInitSize = 1;
        private const int ourZoneObstaclesInitSize = 1;
        private const int oppZoneObstaclesInitSize = 1;
        //////////////////////////////////////////////////////////////////////

        Dictionary<ObstacleType, List<ObstacleBase>> obstacles;

        public Obstacles()
        {
            obstacles = new Dictionary<ObstacleType, List<ObstacleBase>>
            {
                { ObstacleType.Ball, new List<ObstacleBase>(ballObstaclesInitSize) },
                { ObstacleType.Circle, new List<ObstacleBase>(circleObstaclesInitSize) },
                { ObstacleType.OppRobot, new List<ObstacleBase>(oppRobotObstaclesInitSize) },
                { ObstacleType.OppZone, new List<ObstacleBase>(oppZoneObstaclesInitSize) },
                { ObstacleType.OurRobot, new List<ObstacleBase>(ourRobotObstaclesInitSize) },
                { ObstacleType.OurZone, new List<ObstacleBase>(ourZoneObstaclesInitSize) },
                { ObstacleType.Rectangle, new List<ObstacleBase>(rectObstaclesInitSize) }
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
                if (!item.Avoid)
                    if (item.Meet(s, obstacleRadi, margin))
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
                    if (!item.Avoid)
                        if (item.Meet(from, to, obstacleRadi, margin))
                            return item;
            }
            return null;
        }

        /// <summary>
        /// Clear all obstacles
        /// </summary>
        public void Clear()
        {
            foreach (var item in obstacles.Values)
                item.Clear();
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

        public void RemoveSpecificRobots(bool ours, List<int> ids)
        {
            ObstacleType type = ours ? ObstacleType.OurRobot : ObstacleType.OppRobot;
            for (int i = obstacles[type].Count - 1; i >= 0; i--)
                if (obstacles[type][i] is RobotObstacle robotObs && ids.Contains(robotObs.Id))
                    obstacles[type].RemoveAt(i);
        }
    }
}
