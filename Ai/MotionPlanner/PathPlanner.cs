using System;
using System.Collections.Generic;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.Utils;
using System.Linq;
using System.Threading;

namespace MRL.SSL.Ai.MotionPlanner
{
    public class PathPlanner
    {
        ERRT eRRT;
        Obstacles obs;
        List<SingleObjectState> lastPath;
        float lastWeight;
        private ThreadLocal<XorShift> rand;
        public PathPlanner()
        {
            var config = MotionPlannerConfig.Default;
            eRRT = new ERRT(config.UseERRT, config.MaxNodes, config.MaxTries, config.MaxRepulseTries,
                            config.NumWayPoints, config.GoalProbability, config.WayPointProbability,
                            config.SqNearDistTresh, config.ExtendSize);

            obs = new Obstacles();
            lastPath = new List<SingleObjectState>();
            rand = XorShift.CreateInstance();

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
                    obs.Add(new OppRobotObstacle(model.Opponents[item], item));
                }
            if (avoidOurZone)
                obs.Add(new OurZoneObstacle());
            if (avoidOppZone)
                obs.Add(new OppZoneObstacle());

            if (stopBall)
                obs.Add(new CircleObstacle(model.Ball, GameConfig.Default.StopBallRadi));
        }
        public List<SingleObjectState> GetPath(WorldModel model, int robotId, SingleObjectState target)
        {
            float _lastWeight = float.MaxValue, weight;
            bool lastIsSafe = false;
            List<SingleObjectState> res = null;

            if (lastPath.Count > 1)
            {
                lastPath[lastPath.Count - 1] = model.Teammates[robotId];
                res = RandomInterpolateSmoothing(lastPath, obs);
                lastIsSafe = PathWeightCalculator(model, lastPath, robotId, obs, target, out _lastWeight);
                lastWeight = _lastWeight;
            }
            if (!lastIsSafe || MathF.Abs(lastWeight - _lastWeight) > MotionPlannerConfig.Default.RefindPathWeightTresh)
            {
                var path = eRRT.FindPath(model.Teammates[robotId], target, obs, MergerTrackerConfig.Default.OurRobotRadius);
                var smoothedPath = RandomInterpolateSmoothing(path, obs);
                PathWeightCalculator(model, lastPath, robotId, obs, target, out weight);
                if (weight < lastWeight)
                {
                    lastPath = path;
                    lastWeight = weight;
                    res = smoothedPath;
                }
            }


            return res;
        }

        private bool PathWeightCalculator(WorldModel model, List<SingleObjectState> path, int robotID, Obstacles obs, SingleObjectState goal, out float weight)
        {
            if (path.Count < 2)
            {
                weight = float.MaxValue;
                return false;
            }

            var config = MotionPlannerConfig.Default;
            float speed = 0, angle = 0, length = 0;
            ObstacleBase o;
            VectorF2D v1, v2;

            float _angleWeight = config.AngleWeight;
            float _countWeight = config.CountWeight;
            float _speedWieght = config.SpeedWeight;
            float _lengthWieght = config.LengthWeight;
            float _distanceWeight = config.DistanceWeight;


            var s = path[path.Count - 2].Location - path[path.Count - 1].Location;
            speed = MathF.Abs(VectorF2D.AngleBetweenInRadians(s, model.Teammates[robotID].Speed));
            if (speed < Math.PI / 12)
                speed = 0;

            var met = obs.Meet(path[1], path[0], MergerTrackerConfig.Default.OurRobotRadius, out o);
            var metZone = obs.Meet(ObstacleType.OurZone, path[1], path[0],
                                         MergerTrackerConfig.Default.OurRobotRadius, out o);

            v1 = path[1].Location.Sub(path[0].Location);
            length = v1.Length();

            for (int i = path.Count - 1; i > 1; i--)
            {
                if (!met && obs.Meet(path[i], path[i - 1], MergerTrackerConfig.Default.OurRobotRadius, out o))
                    met = true;
                if (!metZone && obs.Meet(ObstacleType.OurZone, path[i], path[i - 1],
                                         MergerTrackerConfig.Default.OurRobotRadius, out o))
                    metZone = true;
                v1 = path[i].Location.Sub(path[i - 1].Location);
                length += v1.Length();
                v2 = path[i - 1].Location.Sub(path[i - 2].Location);
                angle += MathF.Abs(VectorF2D.AngleBetweenInRadians(v1, v2));
            }

            angle /= path.Count;
            bool isSafe = false;
            float baseWeight = 0f, d = 0f;
            if (metZone)
                baseWeight = config.MetZoneBaseWeight;
            else if (met)
                baseWeight = config.MetBaseWeight;
            else
            {
                d = goal.Location.Sub(path[0].Location).Length();
                if (d > config.NotReachedTresh)
                    baseWeight = config.NotReachedBaseWeight;
                else
                {
                    isSafe = true;
                    _distanceWeight = 0;
                }
            }

            weight = (baseWeight + _angleWeight
                        * angle
                        + _countWeight
                        * path.Count
                        + _speedWieght
                        * speed
                        + _lengthWieght
                        * length
                        + d
                        * _distanceWeight);
            return isSafe;
        }
        public List<SingleObjectState> RandomInterpolateSmoothing(List<SingleObjectState> path, Obstacles obs)
        {
            if (path.Count <= 2)
                return path;

            var ppat = path.ToList();
            ObstacleBase o;

            int N = path.Count / 2;
            for (int i = 0; i < N; i++)
            {
                var nodes = new List<int>();
                for (int k = 0; k < ppat.Count; k++)
                    nodes.Add(k);

                int s = rand.Value.RandInt(0, nodes.Count);

                int sKey = nodes[s];
                SingleObjectState start = ppat[sKey];
                nodes.RemoveAt(s);
                if (s > 0 && s < ppat.Count - 1)
                    nodes.RemoveAt(s);
                if (s - 1 >= 0 && nodes.Count > s - 1)
                    nodes.RemoveAt(s - 1);
                if (nodes.Count == 0)
                    continue;
                int e = rand.Value.RandInt(0, nodes.Count);
                int eKey = nodes[e];
                var end = ppat[eKey];

                if (!obs.Meet(start, end, MergerTrackerConfig.Default.OurRobotRadius, out o))
                {
                    int min = (sKey < eKey) ? sKey : eKey;
                    int max = (sKey > eKey) ? sKey : eKey;
                    if (max - min > 1)
                    {
                        ppat.RemoveRange(min + 1, max - min - 1);
                        var v = end.Location.Sub(start.Location);
                        var d = v.Length();
                        var step = MotionPlannerConfig.Default.ExtendSize;
                        if (d > step)
                        {
                            var t = v.Scale(step);
                            int count = (int)(d / step);
                            var lastNode = start.Location;
                            for (int j = 0; j < count; j++)
                            {
                                lastNode = lastNode.Add(t);
                                ppat.Add(new SingleObjectState(lastNode));
                            }
                        }
                    }
                }
                if (ppat.Count == 2)
                    break;
            }
            return ppat;
        }

    }
}