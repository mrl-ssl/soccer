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
            var config = PathPlannerConfig.Default;
            eRRT = new ERRT(config.UseERRT, config.MaxNodes, config.MaxTries, config.MaxRepulseTries,
                            config.NumWayPoints, config.GoalProbability, config.WayPointProbability,
                            config.SqNearDistTresh, config.ExtendSize);

            obs = new Obstacles();
            lastPath = new List<SingleObjectState>();
            rand = XorShift.CreateInstance();
            lastWeight = float.MaxValue;
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
            int lastObsIdx = -1, obsIdx = -1;
            List<SingleObjectState> res = null;

            if (lastPath.Count > 1)
            {
                lastPath[lastPath.Count - 1] = model.Teammates[robotId];
                res = RandomInterpolateSmoothing(lastPath, obs);
                // res = lastPath;
                lastPath = res;
                lastIsSafe = PathWeightCalculator(model, res, robotId, obs, target, out lastObsIdx, out _lastWeight);

            }
            if (!lastIsSafe || MathF.Abs(lastWeight - _lastWeight) > PathPlannerConfig.Default.RefindPathWeightTresh)
            {
                var path = eRRT.FindPath(model.Teammates[robotId], target, obs, MergerTrackerConfig.Default.OurRobotRadius);
                var smoothedPath = RandomInterpolateSmoothing(path, obs);
                // var smoothedPath = path;
                PathWeightCalculator(model, smoothedPath, robotId, obs, target, out obsIdx, out weight);
                if (weight < _lastWeight || (lastObsIdx != obsIdx && obsIdx != -1 && lastObsIdx != -1))
                {
                    lastPath = path;
                    lastWeight = weight;
                    res = smoothedPath;
                }
                else
                    lastWeight = _lastWeight;
            }
            else
                lastWeight = _lastWeight;

            return res;
        }

        private bool PathWeightCalculator(WorldModel model, List<SingleObjectState> path, int robotID, Obstacles obs, SingleObjectState goal, out int obsIdx, out float weight)
        {
            obsIdx = -1;
            if (path.Count < 2)
            {
                weight = float.MaxValue;
                return false;
            }

            var config = PathPlannerConfig.Default;
            float speed = 0, angle = 0, length = 0;
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

            var metZone = obs.Meet(ObstacleType.OurZone, path[1], path[0],
                                         MergerTrackerConfig.Default.OurRobotRadius, out obsIdx);
            var met = !metZone && obs.Meet(path[1], path[0], MergerTrackerConfig.Default.OurRobotRadius, out obsIdx);

            v1 = path[1].Location.Sub(path[0].Location);
            length = v1.Length();

            for (int i = path.Count - 1; i > 1; i--)
            {
                if (!met && !metZone
                    && (obs.Meet(ObstacleType.OurZone, path[i], path[i - 1],
                                           MergerTrackerConfig.Default.OurRobotRadius, out obsIdx)
                        || obs.Meet(ObstacleType.OppZone, path[i], path[i - 1],
                                           MergerTrackerConfig.Default.OurRobotRadius, out obsIdx)))
                {
                    metZone = true;
                }
                if (!metZone && !met && obs.Meet(path[i], path[i - 1], MergerTrackerConfig.Default.OurRobotRadius, out obsIdx))
                    met = true;

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
                isSafe = true;

            d = goal.Location.Sub(path[0].Location).Length();
            if (d > config.NotReachedTresh)
            {
                baseWeight += config.NotReachedBaseWeight;
                isSafe = false;
            }
            else
            {
                _distanceWeight = 0;
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

            int N = path.Count / 2;// Math.Min(20, path.Count / 2);
            for (int i = 0; i < N; i++)
            {
                var nodes = new List<int>();
                for (int k = 0; k < ppat.Count; k++)
                    nodes.Add(k);

                int s = i < N / 2 ? 0 : rand.Value.RandInt(0, nodes.Count);

                int sKey = nodes[s];
                SingleObjectState start = ppat[sKey];
                nodes.RemoveAt(s);
                if (s < nodes.Count)
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
                        // var lastNode = ppat[min].Location;
                        // var v = ppat[max].Location.Sub(lastNode);

                        ppat.RemoveRange(min + 1, max - min - 1);
                        // var d = v.Length();
                        // var step = PathPlannerConfig.Default.ExtendSize;
                        // if (d > step)
                        // {
                        //     var t = v.Scale(step);
                        //     int count = (int)(d / step);
                        //     var extra = new List<SingleObjectState>();
                        //     for (int j = 0; j < count; j++)
                        //     {
                        //         lastNode = lastNode.Add(t);
                        //         extra.Add(new SingleObjectState(lastNode));
                        //     }
                        //     ppat.InsertRange(min, extra);
                        // }
                    }
                }
                if (ppat.Count == 2)
                    break;
            }
            return ppat;
        }

    }
}