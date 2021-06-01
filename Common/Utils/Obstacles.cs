// using System;
// using System.Collections.Generic;
// using MRL.SSL.Common.Configuration;

// namespace MRL.SSL.Common.Utils
// {
//     public class Obstacles
//     {
//         Dictionary<ObstacleType, List<ObstacleBase>> obstacles;

//         public Obstacles()
//         {
//             obstacles = new Dictionary<ObstacleType, List<ObstacleBase>>
//             {
//                 { ObstacleType.Ball, new List<ObstacleBase>(2) },
//                 { ObstacleType.OppRobot, new List<ObstacleBase>(MergerTrackerConfig.Default.MaxTeamRobots) },
//                 { ObstacleType.OurRobot, new List<ObstacleBase>(MergerTrackerConfig.Default.MaxTeamRobots) },
//                 { ObstacleType.OppZone, new List<ObstacleBase>(1) },
//                 { ObstacleType.OurZone, new List<ObstacleBase>(1) },
//                 { ObstacleType.Circle, new List<ObstacleBase>() },
//                 { ObstacleType.Rectangle, new List<ObstacleBase>() }
//             };
//         }

//         /// <summary>
//         /// Check there is specific obstacle type in given state and returns that.
//         /// if there is not returns null
//         /// </summary>
//         /// <param name="s">target state</param> 
//         public bool Meet(SingleObjectState s, float obstacleRadi, out ObstacleBase obs, float margin = 0f)
//         {
//             foreach (var type in obstacles.Keys)
//             {
//                 foreach (var item in obstacles[type])
//                     if (!item.Mask && item.Meet(s, obstacleRadi, margin))
//                     {
//                         obs = item;
//                         return true;
//                     }
//             }
//             obs = null;
//             return false;
//         }

//         /// <summary>
//         /// Check there is obstacle from state a to b and returns that.
//         /// if there is not returns null
//         /// </summary>
//         public bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, out ObstacleBase obs, Dictionary<ObstacleType, float> margins = null)
//         {
//             foreach (var type in obstacles.Keys)
//             {
//                 float margin = (margins != null && margins.ContainsKey(type)) ? margins[type] : 0f;
//                 foreach (var item in obstacles[type])
//                     if (!item.Mask && item.Meet(from, to, obstacleRadi, margin))
//                     {
//                         obs = item;
//                         return true;
//                     }
//             }
//             obs = null;
//             return false;
//         }

//         public void Remove(ObstacleBase o)
//         {
//             var idx = obstacles[o.Type].FindIndex(f => f == o);
//             if (idx >= 0)
//                 obstacles[o.Type].RemoveAt(idx);
//         }

//         /// <summary>
//         /// Clear all obstacles
//         /// </summary>
//         public void Clear()
//         {
//             foreach (var item in obstacles.Keys)
//                 obstacles[item].Clear();
//         }

//         /// <summary>
//         /// Avoid all obstacles from given type
//         /// (Call this before calling "Meet" to apply changes)
//         /// </summary>
//         /// <param name="type">type of obstacles you want to avoid from</param>
//         /// <param name="avoid">set false to not avoid</param>
//         public void ClearMasks()
//         {
//             foreach (var type in obstacles.Keys)
//                 foreach (var item in obstacles[type])
//                     item.Mask = false;
//         }

//         public void Add(ObstacleBase obstacle) => obstacles[obstacle.Type].Add(obstacle);

//         public void Clear(ObstacleType type) => obstacles[type].Clear();


//     }
// }
using System;
using System.Collections.Generic;
using MRL.SSL.Common.Configuration;

namespace MRL.SSL.Common.Utils
{
    public class Obstacles
    {
        ObstacleBase[] obstacles;
        int count;
        public Obstacles()
        {
            count = 0;
            obstacles = new ObstacleBase[30];
        }

        /// <summary>
        /// Check there is specific obstacle type in given state and returns that.
        /// if there is not returns null
        /// </summary>
        /// <param name="s">target state</param> 
        public bool Meet(ObstacleType type, SingleObjectState from, SingleObjectState to, float obstacleRadi, out ObstacleBase obs, float margin = 0f)
        {
            for (int i = 0; i < count; i++)
            {
                var o = obstacles[i];
                if (o.Type != type) continue;
                if (!o.Mask && o.Meet(from, to, obstacleRadi, 0f))
                {
                    obs = o;
                    return true;
                }
            }
            obs = null;
            return false;
        }
        public bool Meet(SingleObjectState s, float obstacleRadi, out ObstacleBase obs, float margin = 0f)
        {
            for (int i = 0; i < count; i++)
            {
                var o = obstacles[i];
                if (!o.Mask && o.Meet(s, obstacleRadi, margin))
                {
                    obs = o;
                    return true;
                }
            }
            obs = null;
            return false;
        }

        /// <summary>
        /// Check there is obstacle from state a to b and returns that.
        /// if there is not returns null
        /// </summary>
        public bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, out ObstacleBase obs)
        {
            for (int i = 0; i < count; i++)
            {
                var o = obstacles[i];
                if (!o.Mask && o.Meet(from, to, obstacleRadi, 0f))
                {
                    obs = o;
                    return true;
                }
            }
            obs = null;
            return false;
        }
        public bool Meet(ObstacleType type, SingleObjectState from, SingleObjectState to, float obstacleRadi, out int idx, float margin = 0f)
        {
            for (int i = 0; i < count; i++)
            {
                var o = obstacles[i];
                if (o.Type != type) continue;
                if (!o.Mask && o.Meet(from, to, obstacleRadi, 0f))
                {
                    idx = i;
                    return true;
                }
            }
            idx = -1;
            return false;
        }
        public bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, out int idx)
        {
            for (int i = 0; i < count; i++)
            {
                var o = obstacles[i];
                if (!o.Mask && o.Meet(from, to, obstacleRadi, 0f))
                {
                    idx = i;
                    return true;
                }
            }
            idx = -1;
            return false;
        }
        /// <summary>
        /// Clear all obstacles
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < count; i++)
            {
                obstacles[i] = null;
            }
            count = 0;
        }

        /// <summary>
        /// Avoid all obstacles from given type
        /// (Call this before calling "Meet" to apply changes)
        /// </summary>
        /// <param name="type">type of obstacles you want to avoid from</param>
        /// <param name="avoid">set false to not avoid</param>
        public void ClearMasks()
        {
            for (int i = 0; i < count; i++)
            {
                obstacles[i].Mask = false;
            }
        }

        public void Add(ObstacleBase obstacle)
        {
            obstacles[count++] = obstacle;
        }
    }
}
