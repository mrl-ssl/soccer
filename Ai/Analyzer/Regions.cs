using System;
using System.Collections.Generic;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.Utils;

namespace MRL.SSL.Analyzer
{
    public partial class Regions : IDisposable
    {
        private List<VisibleGoalInterval> GetVisibleGoalIntervals(List<SingleObjectState> ourRobot, List<SingleObjectState> oppRobot, Vector2D<float> fromLocation, float ang, Vector2D<float> goalStart, Vector2D<float> goalEnd)
        {
            List<VisibleGoalInterval> intervals = new List<VisibleGoalInterval>();
            Vector2D<float> pos = new VectorF2D();
            Vector2D<float> goalCenter = Vector2D<float>.Interpolate(goalStart, goalEnd, 0.5f);

            intervals.Add(new VisibleGoalInterval(new Interval(goalStart.Y, goalEnd.Y), (goalEnd - goalStart).Length() * (float)Math.Sin(Math.Abs(Vector2D<float>.AngleBetweenInRadians(goalEnd - goalStart, goalCenter - fromLocation)))));
            Vector2D<float> centerDirection = goalCenter - fromLocation;

            // Our Robots
            foreach (var our in ourRobot)
            {
                Vector2D<float> tmp = our.Location - goalStart;
                pos = goalStart + VectorF2D.FromAngleSize(tmp.AngleInRadians() + ang, tmp.Length());
                if (centerDirection.Dot(pos - fromLocation) > 0)
                    ExcludeObstacle(intervals, new Circle((VectorF2D)pos, 0.09f), (VectorF2D)fromLocation, (VectorF2D)centerDirection, (VectorF2D)goalCenter, new Line((VectorF2D)goalStart, (VectorF2D)goalEnd));
            }
            // Opponent Robots
            foreach (var opp in oppRobot)
            {
                Vector2D<float> tmp = opp.Location - goalStart;
                pos = goalStart + VectorF2D.FromAngleSize(tmp.AngleInRadians() + ang, tmp.Length());
                if (centerDirection.Dot(pos - fromLocation) > 0)
                    ExcludeObstacle(intervals, new Circle((VectorF2D)pos, 0.09f), (VectorF2D)fromLocation, (VectorF2D)centerDirection, (VectorF2D)goalCenter, new Line((VectorF2D)goalStart, (VectorF2D)goalEnd));
            }

            return intervals;
        }

        void ExcludeObstacle(List<VisibleGoalInterval> intervals, Circle obstacle, VectorF2D fromLocation, VectorF2D centerDirection, VectorF2D goalCenter, Line goalLine)
        {
            if (intervals.Count == 0)
                return;
            List<Line> tangentLines;
            List<VectorF2D> tangentPoints;
            int tangents = obstacle.GetTangent(fromLocation, out tangentLines, out tangentPoints);
            Interval toExclude;
            if (tangents == 2)
                toExclude = new Interval(
                    GetExtreme(fromLocation, tangentPoints[0], goalCenter, tangentLines[0], true, goalLine),
                    GetExtreme(fromLocation, tangentPoints[1], goalCenter, tangentLines[1], true, goalLine));
            else if (tangents == 1)
                toExclude = new Interval(
                    GetExtreme(fromLocation, tangentPoints[0], goalCenter, tangentLines[0], true, goalLine),
                    GetExtreme(fromLocation, tangentPoints[0], goalCenter, tangentLines[0], false, goalLine)
                    );
            else //tangents == 0
            {
                Line l = new Line(fromLocation, obstacle.Position).PerpenducilarLineToPoint(fromLocation);
                toExclude = new Interval(
                    GetExtreme(fromLocation, fromLocation, goalCenter, l, true, goalLine),
                    GetExtreme(fromLocation, fromLocation, goalCenter, l, false, goalLine)
                    );
            }
            int i = 0;
            while (i < intervals.Count && intervals[i].interval.End <= toExclude.Start)
                i++;
            if (i < intervals.Count)
                if (intervals[i].interval.Start < toExclude.Start)
                {
                    double temp = intervals[i].interval.End;
                    intervals[i] = new VisibleGoalInterval(new Interval(intervals[i].interval.Start, toExclude.Start), intervals[i].ViasibleWidth);
                    i++;
                    if (temp > toExclude.End)
                    {
                        intervals.Insert(i, new VisibleGoalInterval(new Interval((float)toExclude.End, (float)temp), 0));
                        i++;
                    }
                }
            while (i < intervals.Count && intervals[i].interval.End < toExclude.End)
                intervals.RemoveAt(i);
            if (i < intervals.Count && intervals[i].interval.Start < toExclude.End)
                intervals[i] = new VisibleGoalInterval(new Interval(toExclude.End, intervals[i].interval.End), intervals[i].ViasibleWidth);
        }
        float GetExtreme(VectorF2D fromLocation, VectorF2D tangentPoint, VectorF2D goalCenter, Line l, bool Pos, Line goalLine)
        {
            VectorF2D vect = tangentPoint.Sub(fromLocation);
            if (vect.SqLength() == 0)
                vect = (Pos ? new VectorF2D(l.B, l.A) : new VectorF2D(-l.B, -l.A));
            if (MathF.Sign(vect.X) == MathF.Sign(goalCenter.X - fromLocation.X))
            {
                bool HasValue;
                VectorF2D pos = goalLine.IntersectWithLine(l, out HasValue);
                if (HasValue)
                    return pos.Y;
            }
            if (vect.Y < 0)
                return -1000;
            else
                return 1000;
        }

        public void Dispose()
        {

        }

    }

    public struct Interval
    {
        public float Start;
        public float End;
        public Interval(float start, float end)
        {
            if (start < end)
            {
                Start = start;
                End = end;
            }
            else
            {
                End = start;
                Start = end;
            }
        }
        public float Length()
        {
            return End - Start;
        }
        public bool Contains(float p)
        {
            return p >= Start && p <= End;
        }
    }
    public struct VisibleGoalInterval
    {
        public Interval interval;
        public float ViasibleWidth;

        public VisibleGoalInterval(Interval _interval, float viasibleWidth)
        {
            interval = new Interval(_interval.Start, _interval.End);
            ViasibleWidth = viasibleWidth;
        }
    }
}