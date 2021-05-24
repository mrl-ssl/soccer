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
        public bool Avoid { get; set; }

        protected ObstacleBase(SingleObjectState state) { this.state = state; Avoid = false; }
        // protected ObstacleBase() { this.state = new SingleObjectState(); }

        public abstract bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, float margin = 0f);
        public abstract bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f);
    }

    public class CircleObstacle : ObstacleBase
    {
        protected float radius;

        public override ObstacleType Type => ObstacleType.Circle;

        public CircleObstacle(SingleObjectState state, float radius) : base(state) { this.radius = radius; }

        public override bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, float margin = 0f)
        {
            Vector2D<float> p = VectorF2D.PointOnSegment(from.Location, to.Location, state.Location);
            float d = p.Distance(state.Location);
            return d <= radius + obstacleRadi + margin;
        }

        public override bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f)
        {
            Vector2D<float> v = S1.Location - state.Location;
            return v.SqLength() < (obstacleRadi + radius + margin) * (obstacleRadi + radius + margin);
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

        public override bool Meet(SingleObjectState from, SingleObjectState to, float obstacleRadi, float margin = 0f)
        {
            float d = 0;

            for (int i = 0; i < 4; i++)
            {
                d = VectorF2D.DistanceSegToSeg(from.Location, to.Location, c[i], c[(i + 1) % 4]);
                if (d < obstacleRadi + margin) return true;
            }
            return Meet(from, obstacleRadi, margin) || Meet(to, obstacleRadi, margin);
        }

        public override bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f)
        {
            var sqW = (width + margin + obstacleRadi) * (width + margin + obstacleRadi);
            var sqH = (height + margin + obstacleRadi) * (height + margin + obstacleRadi);

            for (int i = 0; i < 4; i++)
            {
                var d = VectorF2D.SqDistanceToLine(S1.Location, c[i], c[(i + 1) % 4]);
                if ((i == 0 || i == 2) && d > sqH) return false;
                if ((i == 1 || i == 3) && d > sqW) return false;
            }
            return true;
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

        public OurZoneObstacle()
            : base(GameParameters.Field.OurPenaltyRearLeft, GameParameters.Field.OurPenaltyRearRight,
                   GameParameters.Field.OurPenaltyBackRight, GameParameters.Field.OurPenaltyBackLeft, VectorF2D.Zero)
        {
        }
    }

    public class OppZoneObstacle : RectObstacle
    {
        public override ObstacleType Type => ObstacleType.OppZone;

        public OppZoneObstacle()
            : base(GameParameters.Field.OppPenaltyRearLeft, GameParameters.Field.OppPenaltyRearRight,
                   GameParameters.Field.OppPenaltyBackRight, GameParameters.Field.OppPenaltyBackLeft, VectorF2D.Zero)
        {
        }
    }

    public class BallObstacle : CircleObstacle
    {
        public override ObstacleType Type => ObstacleType.Ball;

        public BallObstacle(SingleObjectState state) : base(state, MergerTrackerConfig.Default.BallRadi) { }
    }
}