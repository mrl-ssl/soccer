using System;
using System.Collections.Generic;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
{
    public class ERRT
    {
        private readonly float robotRadi = RobotConfig.Default.Radius;

        private const int maxNodesCount = 400;
        private const int wayPointsCount = 30;
        private const float extendStepSize = 0.15f;
        private const float nearThreshold = 0.14f;
        private const float goalProb = 0.2f;
        private const float wayPointProb = 0.4f;

        private readonly Random random;
        private KdTree.KdTree tree;
        private readonly SingleObjectState[] wayPoints;

        public ERRT()
        {
            random = new Random(Environment.TickCount);
            tree = new KdTree.KdTree(maxNodesCount);
            wayPoints = new SingleObjectState[wayPointsCount];
            for (int i = 0; i < wayPointsCount; i++)
                wayPoints[i] = GenerateRandomState();
        }

        public List<SingleObjectState> PlanPath(SingleObjectState init, SingleObjectState goal, Obstacles obstacles)
        {
            init.Parent = null;
            tree.Reset(init);
            int iteration = 0;
            bool goalIsTarget, isExtended;
            float distance = init.DistanceFrom(goal), bestDistance = distance;
            SingleObjectState target, nearest, best = init;
            while (iteration < maxNodesCount && bestDistance > nearThreshold)
            {
                target = ChoosTarget(goal, out goalIsTarget);
                nearest = (goalIsTarget) ? best : tree.Nearest(target);
                isExtended = Extend(target, nearest, obstacles);
                if (isExtended)
                {
                    distance = target.DistanceFrom(goal);
                    if (distance < bestDistance)
                    {
                        best = target;
                        bestDistance = distance;
                    }
                }
                iteration++;
            }
            var path = GeneratePath(best);
            if (bestDistance < nearThreshold) //path found
                FillWayPoints(path);
            return path;
        }

        private void FillWayPoints(List<SingleObjectState> path)
        {
            List<int> usedIndexes = new List<int>(path.Count);
            int t = random.Next(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                while (usedIndexes.Contains(t)) t = random.Next(wayPointsCount);
                usedIndexes.Add(t);
                wayPoints[t] = path[i];
                wayPoints[t].Parent = null;
            }
        }

        private List<SingleObjectState> GeneratePath(SingleObjectState lastNode)
        {
            List<SingleObjectState> path = new List<SingleObjectState>();
            while (lastNode.Parent != null)
            {
                path.Add(lastNode);
                lastNode = lastNode.Parent;
            }
            path.Reverse();
            return path;
        }

        private bool Extend(SingleObjectState target, SingleObjectState nearest, Obstacles obstacles)
        {
            Vector2D<float> node2nearest = nearest.Location - target.Location;
            float length = node2nearest.Length();
            if (length > extendStepSize)
            {
                node2nearest.Scale(extendStepSize / length);
                target.Location = nearest.Location + node2nearest;
            }
            if (obstacles.Meet(target, robotRadi) == null)
            {
                target.Parent = nearest;
                tree.Add(target);
                return true;
            }
            return false;
        }

        private SingleObjectState ChoosTarget(SingleObjectState goal, out bool goalIsTarget)
        {
            goalIsTarget = false;
            float p = (float)random.NextDouble();
            if (p < goalProb)
            {
                goalIsTarget = true;
                return goal;
            }
            if (p < wayPointProb + goalProb)
                return wayPoints[random.Next(wayPointsCount)];
            return GenerateRandomState();
        }
        private SingleObjectState GenerateRandomState() => new SingleObjectState();//new SingleObjectState(MotionPlannerParameters.FieldLength_H - MotionPlannerParameters.FieldLength * random.NextDouble(), MotionPlannerParameters.FieldWidth_H - MotionPlannerParameters.FieldWidth * random.NextDouble());
    }
}