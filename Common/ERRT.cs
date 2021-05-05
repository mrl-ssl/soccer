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
        private const float extendMaxStepSize = 0.15f;
        private const float nearThreshold = 0.14f;
        private const float goalProb = 0.2f;
        private const float wayPointProb = 0.4f;

        private readonly Random random;
        private KdTree.KdTree tree;
        private readonly SingleObjectState[] wayPoints;
        private readonly SingleObjectState[] lastPath;

        public ERRT()
        {
            random = new Random(Environment.TickCount);
            tree = new KdTree.KdTree(maxNodesCount);
            wayPoints = new SingleObjectState[wayPointsCount];
            for (int i = 0; i < wayPointsCount; i++)
                wayPoints[i] = GenerateRandomState();
        }

        public SingleObjectState[] PlanPath(SingleObjectState init, SingleObjectState goal, Obstacles obstacles)
        {
            init.Parent = null;
            tree.Reset(init);
            int iteration = 0;
            float distance = init.Location.Distance(goal.Location), bestDistance = distance;
            SingleObjectState target, nearest, best = init;
            while (iteration < maxNodesCount && bestDistance > nearThreshold)
            {
                target = ChoosTarget(goal);
                if (obstacles.Meet(target, robotRadi) != null)
                    continue;
                nearest = tree.Nearest(target);
                Extend(target, nearest);
                distance = target.Location.Distance(goal.Location);
                if (distance < bestDistance)
                {
                    best = target;
                    bestDistance = distance;
                }
                iteration++;
            }
            var path = GeneratePath(best, iteration);
            FillWayPoints(path);
            return path;
        }

        private void FillWayPoints(SingleObjectState[] path)
        {
            List<int> usedIndexes = new List<int>(wayPointsCount);
            int t = random.Next(path.Length);
            for (int i = 0; i < wayPointsCount; i++)
            {
                while (usedIndexes.Contains(t)) t = random.Next(path.Length);
                wayPoints[i] = path[t];
            }
        }

        private SingleObjectState[] GeneratePath(SingleObjectState lastNode, int nodesCount)
        {
            SingleObjectState[] path = new SingleObjectState[nodesCount];
            SingleObjectState node = lastNode;
            int i = nodesCount;
            while (node.Parent != null)
            {
                path[i--] = node;
                node = node.Parent;
            }
            return path;
        }

        private void Extend(SingleObjectState node, SingleObjectState nearest)
        {
            Vector2D<float> node2nearest = nearest.Location - node.Location;
            float length = node2nearest.Length();
            if (length > extendMaxStepSize)
            {
                node2nearest.Scale(extendMaxStepSize / length);
                node.Location = (VectorF2D)(nearest.Location + node2nearest);
            }
            tree.Add(node);
            node.Parent = nearest;
        }

        private SingleObjectState ChoosTarget(SingleObjectState goal)
        {
            float p = (float)random.NextDouble();
            if (p < goalProb)
                return goal;
            if (p < wayPointProb + goalProb)
                return wayPoints[random.Next(wayPointsCount)];
            return GenerateRandomState();
        }
        private SingleObjectState GenerateRandomState() => new SingleObjectState();//new SingleObjectState(MotionPlannerParameters.FieldLength_H - MotionPlannerParameters.FieldLength * random.NextDouble(), MotionPlannerParameters.FieldWidth_H - MotionPlannerParameters.FieldWidth * random.NextDouble());
    }
}