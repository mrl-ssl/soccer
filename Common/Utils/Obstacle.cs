using System;
using System.Runtime.CompilerServices;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common.Utils
{
    public enum ObstacleType
    {
        Circle,
        Rectangle,
        OurRobot,
        OppRobot,
        Ball,
        OurZone,
        OppZone
    }

    public abstract class ObstacleBase
    {
        protected SingleObjectState state;
        public abstract ObstacleType Type { get; }
        public SingleObjectState State { get => state; set => state = value; }
        public bool Mask { get; set; }
        public bool CanRepulse { get; set; }

        protected ObstacleBase(SingleObjectState state) { this.state = state; Mask = false; }
        // protected ObstacleBase() { this.state = new SingleObjectState(); }
        public abstract VectorF2D Repulse(SingleObjectState s, float obstacleRadi, float margin = 0.01f);
        public abstract float GetTangents(SingleObjectState from, float obstacleRadi, float margin = 0f);
        public abstract bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, float margin = 0f);
        public abstract bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f);
    }

    public class CircleObstacle : ObstacleBase
    {
        protected float radius;

        public override ObstacleType Type => ObstacleType.Circle;

        public CircleObstacle(SingleObjectState state, float radius) : base(state) { this.radius = radius; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, float margin = 0f)
        {
            var p = VectorF2D.PointOnSegment(from.Location, to.Location, state.Location);
            float d = p.Distance(state.Location);
            return MathHelper.LessThan(d, radius + obstacleRadi + margin);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f)
        {
            return MathHelper.LessThan(S1.Location.Distance(state.Location), (obstacleRadi + radius + margin));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override VectorF2D Repulse(SingleObjectState s, float obstacleRadi, float margin = 0.01f)
        {
            var v = s.Location - state.Location;
            v.NormTo(radius + obstacleRadi + margin);
            return (VectorF2D)v.Add(state.Location);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float GetTangents(SingleObjectState from, float obstacleRadi, float margin = 0)
        {
            var d = from.Location.Distance(state.Location);
            var tetha = MathF.Asin(MathF.Max(radius + obstacleRadi + margin, d) / d);
            return tetha;
        }
    }

    public class RectObstacle : ObstacleBase
    {
        protected VectorF2D[] c = new VectorF2D[4];
        public override ObstacleType Type => ObstacleType.Rectangle;
        protected float width, height;

        public RectObstacle(VectorF2D c0, VectorF2D c1, VectorF2D c2, VectorF2D c3, VectorF2D speed) : base(new SingleObjectState(c0.Interpolate(c2, 0.5f), speed))
        {
            c[0] = c0;
            c[1] = c1;
            c[2] = c2;
            c[3] = c3;
            width = (c0 - c1).Length();
            height = (c1 - c2).Length();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, float margin = 0f)
        {
            float d = 0;

            for (int i = 0; i < 4; i++)
            {
                d = VectorF2D.DistanceSegToSeg(from.Location, to.Location, c[i], c[(i + 1) % 4]);
                if (MathHelper.LessThan(d, obstacleRadi + margin)) return true;
            }
            return Meet(from, obstacleRadi, margin) || Meet(to, obstacleRadi, margin);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f)
        {
            var w = (width + margin + obstacleRadi);
            var h = (height + margin + obstacleRadi);

            for (int i = 0; i < 4; i++)
            {
                var d = VectorF2D.DistanceToLine(S1.Location, c[i], c[(i + 1) % 4]);
                if ((i == 0 || i == 2) && d >= h) return false;
                if ((i == 1 || i == 3) && d >= w) return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override VectorF2D Repulse(SingleObjectState s, float obstacleRadi, float margin = 0.01f)
        {
            VectorF2D minV = null;
            float d = float.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                var v = s.Location.PrependecularPoint(c[i], c[(i + 1) % 4]).Sub(s.Location);
                if (v.SqLength() < d)
                {
                    minV = v;
                    d = v.SqLength();
                }
            }
            minV.Extend(obstacleRadi + margin);
            return minV.Add(s.Location);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float GetTangents(SingleObjectState from, float obstacleRadi, float margin = 0)
        {
            int nearestId = 0;
            float d = c[0].SqDistance(from.Location);
            float minD = d;
            for (int i = 1; i < 4; i++)
            {
                d = c[i].SqDistance(from.Location);
                if (d < minD)
                {
                    minD = d;
                    nearestId = i;
                }
            }
            int prev = ((nearestId - 1) + 4) % 4;
            int next = (nearestId + 1) % 4;
            var v = from.Location - c[nearestId];
            var v1 = c[nearestId] - c[prev];
            var v2 = c[nearestId] - c[next];

            v1.NormTo(margin + obstacleRadi);
            v2.NormTo(margin + obstacleRadi);

            var t1 = c[prev] + v2;
            var t2 = c[next] + v1;

            if (v.Dot(v1) < 0)
                t2 = c[nearestId] + v2;
            else if (v.Dot(v2) < 0)
                t1 = c[nearestId] + v1;
            var a = (t1 - from.Location).AngleBetweenInRadians(t2 - from.Location) / 2f;
            return a;
        }
    }

    public abstract class RobotObstacle : CircleObstacle
    {
        public int Id { get; }

        protected RobotObstacle(SingleObjectState state, int id) : base(state, RobotConfig.Default.Radius) { Id = id; }
    }

    public class OurRobotObstacle : RobotObstacle
    {
        public override ObstacleType Type => ObstacleType.OurRobot;

        public OurRobotObstacle(SingleObjectState state, int id) : base(state, id) { }
    }

    public class OppRobotObstacle : RobotObstacle
    {
        public override ObstacleType Type => ObstacleType.OppRobot;

        public OppRobotObstacle(SingleObjectState state, int id) : base(state, id) { }
    }

    public class OurZoneObstacle : RectObstacle
    {
        public override ObstacleType Type => ObstacleType.OurZone;

        public static float Margin { get { return MergerTrackerConfig.Default.OurRobotRadius; } }
        public OurZoneObstacle()
            : base(GameParameters.Field.OurPenaltyRearLeft.Add(new VectorF2D(-Margin, -Margin)),
                   GameParameters.Field.OurPenaltyRearRight.Add(new VectorF2D(-Margin, Margin)),
                   GameParameters.Field.OurPenaltyBackRight.Add(new VectorF2D(0f, Margin)),
                   GameParameters.Field.OurPenaltyBackLeft.Add(new VectorF2D(0f, -Margin)),
                   VectorF2D.Zero)
        {
            CanRepulse = true;
        }
        public override VectorF2D Repulse(SingleObjectState s, float obstacleRadi, float margin = 0)
        {
            VectorF2D minV = null;
            float d = float.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                if (i == 2) continue;
                var v = s.Location.PrependecularPoint(c[i], c[(i + 1) % 4]).Sub(s.Location);
                if (v.SqLength() < d)
                {
                    minV = v;
                    d = v.SqLength();
                }
            }
            minV.Extend(obstacleRadi + margin);
            return minV.Add(s.Location);
        }
    }

    public class OppZoneObstacle : RectObstacle
    {
        public override ObstacleType Type => ObstacleType.OppZone;
        public static float Margin { get { return MergerTrackerConfig.Default.OpponentRadius; } }

        public OppZoneObstacle()
            : base(
                  GameParameters.Field.OppPenaltyRearLeft.Add(new VectorF2D(Margin, Margin)),
                  GameParameters.Field.OppPenaltyRearRight.Add(new VectorF2D(Margin, -Margin)),
                  GameParameters.Field.OppPenaltyBackRight.Add(new VectorF2D(0, -Margin)),
                  GameParameters.Field.OppPenaltyBackLeft.Add(new VectorF2D(0, Margin)),
                  null)
        {
            CanRepulse = true;
        }
        public override VectorF2D Repulse(SingleObjectState s, float obstacleRadi, float margin = 0)
        {
            VectorF2D minV = null;
            float d = float.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                if (i == 2) continue;
                var v = s.Location.PrependecularPoint(c[i], c[(i + 1) % 4]).Sub(s.Location);
                if (v.SqLength() < d)
                {
                    minV = v;
                    d = v.SqLength();
                }
            }
            minV.Extend(obstacleRadi + margin);
            return minV.Add(s.Location);
        }
    }

    public class BallObstacle : CircleObstacle
    {
        public override ObstacleType Type => ObstacleType.Ball;

        public BallObstacle(SingleObjectState state)
            : base(state, MergerTrackerConfig.Default.BallRadi)
        { }

    }
}