using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.Utils.Extensions;

namespace MRL.SSL.Common.Utils
{
    public class ERRT
    {
        private float goalProbbality;
        private float wayPointProbbality;
        private int numWayPoints;
        private float extendSize;
        private float sqNearDistTresh;
        private int maxNodes;
        private int maxTries;
        private int maxRepulseTries;
        VectorF2D field;
        VectorF2D minv, maxv;
        private KdTree tree;
        private SingleObjectState[] wayPoints;

        private ThreadLocal<XorShift> rand;
        private bool useERrrt;

        public ERRT(
            bool _useErrt = true, int _maxNodes = 500, int _maxTries = 500, int _maxRepulseTries = 10, int _numWayPoints = 60,
            float _goalProbbality = 0.2f, float _wayPointProbbality = 0.3f, float _sqNearDistTresh = 0.01f,
            float _extendSize = 0.15f)
        {
            useERrrt = _useErrt;
            maxNodes = _maxNodes;
            maxTries = _maxTries;
            maxRepulseTries = _maxRepulseTries;
            numWayPoints = _numWayPoints;
            goalProbbality = _goalProbbality;
            wayPointProbbality = _wayPointProbbality;
            sqNearDistTresh = _sqNearDistTresh;
            extendSize = _extendSize;

            tree = new KdTree();
            rand = XorShift.CreateInstance();
            wayPoints = new SingleObjectState[numWayPoints];

            float x = MathF.Max(FieldConfig.Default.OurGoalCenter.X, MathF.Abs(FieldConfig.Default.OppGoalCenter.X));
            float y = MathF.Max(FieldConfig.Default.OurRightCorner.Y, MathF.Abs(FieldConfig.Default.OurLeftCorner.Y));

            field = new VectorF2D(x + FieldConfig.Default.BoundaryWidth, y + FieldConfig.Default.BoundaryWidth);

            for (int i = 0; i < numWayPoints; i++)
            {
                wayPoints[i] = RandomState();
            }
            minv = new VectorF2D(-field.X, -field.Y);
            maxv = new VectorF2D(field.X, field.Y);
            tree.SetDim(minv, maxv, 16, 8);
        }
        protected bool RepulseTarget(SingleObjectState pos, Obstacles obs, float obstacleRadi, ref bool obsMasked, out SingleObjectState target)
        {
            target = pos.Clone().As<SingleObjectState>();
            bool inObs = true;
            bool repulsed = false;
            int counter = 0;
            bool failed = false;
            obsMasked = false;
            while (inObs && counter < maxRepulseTries)
            {
                ObstacleBase o;
                if (obs.Meet(target, obstacleRadi, out o))
                {
                    if (!o.CanRepulse ||
                    (failed
                     && (o.Type != ObstacleType.OurZone
                        || o.Type != ObstacleType.OppZone))
                    )
                    {
                        o.Mask = true;
                        obsMasked = true;
                    }
                    else
                    {
                        target.Location = o.Repulse(target, obstacleRadi);
                        repulsed = true;
                    }
                }
                else
                    inObs = false;
                counter++;

                if (inObs && counter >= maxRepulseTries && !failed)
                {
                    failed = true;
                    repulsed = false;
                    counter = 0;
                    target = pos.Clone().As<SingleObjectState>();
                }
            }

            return repulsed;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected SingleObjectState RandomState()
        {
            return new SingleObjectState(new VectorF2D(field.X * (1f - 2f * rand.Value.RandFloat()),
                                                        field.Y * (1f - 2f * rand.Value.RandFloat())));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected SingleObjectState ChoosTarget(SingleObjectState goal, out TargetType type)
        {
            var r = rand.Value.RandFloat();
            if (r < goalProbbality)
            {
                type = TargetType.Goal;
                return goal;
            }
            else if (r < (wayPointProbbality + goalProbbality) && useERrrt)
            {
                int l = rand.Value.RandInt() % numWayPoints;
                if (wayPoints[l] != null)
                {
                    type = TargetType.WayPoint;
                    return wayPoints[l];
                }

            }
            type = TargetType.Random;
            return RandomState();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected SingleObjectState Extend(SingleObjectState nearest, SingleObjectState target, Obstacles obs, float obstacleRadi)
        {
            var t = target.Location.Sub(nearest.Location);
            var l = t.Length();

            if (l >= extendSize)
                t = t.Scale(extendSize / l);
            else
                return null;

            var tt = nearest.Location.Add(t);
            var n = new SingleObjectState();
            n.Location = tt;

            ObstacleBase o;
            var s = new VectorF2D(1, 2);

            if (obs.Meet(nearest, n, obstacleRadi, out o))
            {
                return null;
                // var a = o.GetTangents(nearest, obstacleRadi);
                // n.Location = (VectorF2D)(nearest.Location + t.GetRotate(a));
                // if (obs.Meet(nearest, n, obstacleRadi, out o))
                // {
                //     n.Location = (VectorF2D)(nearest.Location + t.GetRotate(-a));
                //     if (obs.Meet(nearest, n, obstacleRadi, out o))
                //         return null;
                // }
            }
            return n;
        }

        protected SingleObjectState AddNode(SingleObjectState n, SingleObjectState parent)
        {
            if (tree.Size >= maxNodes || n == null) return null;

            n.Parent = parent;
            if (tree.Add(n)) return n;

            return null;
        }


        public List<SingleObjectState> FindPath(SingleObjectState init, SingleObjectState goal, Obstacles obs, float obstacleRadi)
        {
            init.Parent = null;

            SingleObjectState nearest, nearestGoal;
            SingleObjectState _init, _goal;
            ObstacleBase o;
            TargetType type;
            int counter = 0;
            bool obsMasked = false;
            float nearestDist = 0, lastNodeSqDist = 0;

            var initRepulsed = RepulseTarget(init, obs, obstacleRadi, ref obsMasked, out _init);
            _goal = goal;
            // var goalRepulsed = RepulseTarget(goal, obs, obstacleRadi, ref obsMasked, out _goal);

            float d = _init.Location.SqDistance(_goal.Location);

            tree.Clear();

            nearest = nearestGoal = AddNode(_init, null);

            if (!obs.Meet(_init, _goal, obstacleRadi, out o))
                nearestGoal = nearest = AddNode(_goal, _init);
            else if (d <= sqNearDistTresh)
            {
                var target = _goal;
                float s = 1.0f;
                bool met;
                do
                {
                    target.Location = _init.Location.Interpolate(_goal.Location, s);
                    met = obs.Meet(_init, target, obstacleRadi, out o);
                    s -= 0.1f;
                } while (s > 0 && met);

                nearestGoal = nearest = AddNode(target, _init);
            }
            else
            {
                d = nearest.Location.SqDistance(_goal.Location);
                while (counter < maxTries && tree.Size < maxNodes && d > sqNearDistTresh)
                {
                    var target = ChoosTarget(_goal, out type);
                    if (type == TargetType.Goal)
                        nearest = nearestGoal;
                    else
                        nearest = tree.Nearest(out nearestDist, target.Location);
                    target = Extend(nearest, target, obs, obstacleRadi);
                    var n = AddNode(target, nearest);
                    if (n != null)
                    {
                        lastNodeSqDist = n.Location.SqDistance(_goal.Location);
                        if (lastNodeSqDist < d)
                        {
                            nearestGoal = n;
                            d = lastNodeSqDist;
                        }
                    }
                    counter++;
                }
                int i = 0;
                if (useERrrt)
                {
                    if (!initRepulsed && !obsMasked && ((d <= sqNearDistTresh) || rand.Value.RandFloat() < 0.1f))
                    {
                        var p = nearestGoal;
                        while (p != null)
                        {
                            i = rand.Value.RandInt() % numWayPoints;
                            wayPoints[i] = p;
                            wayPoints[i].Parent = null;
                            p = p.Parent;
                        }
                    }
                    else
                    {
                        i = rand.Value.RandInt() % numWayPoints;
                        wayPoints[i] = RandomState();
                    }
                }
            }

            obs.ClearMasks();

            if (nearestGoal.Location.Distance(_goal.Location) > 0.001f)
            {
                if (!obs.Meet(nearestGoal, _goal, obstacleRadi, out o))
                {
                    _goal.Parent = nearestGoal;
                    nearestGoal = _goal;
                }
            }

            List<SingleObjectState> res = new List<SingleObjectState>();
            while (nearestGoal != null)
            {
                res.Add(nearestGoal);
                nearestGoal = nearestGoal.Parent;
            }

            if (initRepulsed)
                res.Add(init);

            return res;
        }
        protected enum TargetType
        {
            Goal,
            WayPoint,
            Random
        }
    }
}