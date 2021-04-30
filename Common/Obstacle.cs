using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
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

        public abstract bool Meet(SingleObjectState From, SingleObjectState To, float obstacleRadi, float margin = 0f);
        public abstract bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f);
    }

    public class CircleObstacle : ObstacleBase
    {
        protected float radius;

        public override ObstacleType Type => ObstacleType.Circle;

        public CircleObstacle(SingleObjectState state, float radius) : base(state) { this.radius = radius; }

        public override bool Meet(SingleObjectState From, SingleObjectState To, float obstacleRadi, float margin = 0f)
        {
            Vector2D<float> p = VectorF2D.PointOnSegment(From.Location, To.Location, state.Location);
            float d = p.Distance(state.Location);
            return d <= radius + obstacleRadi + margin;
        }

        public override bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f)
        {
            Vector2D<float> v = S1.Location - state.Location;
            return v.SqLength() < (obstacleRadi + radius) * (obstacleRadi + radius) + margin;
        }
    }

    public class RectObstacle : ObstacleBase
    {
        protected float width, heigth;

        public override ObstacleType Type => ObstacleType.Rectangle;

        public RectObstacle(SingleObjectState state, float width, float heigth) : base(state)
        {
            this.width = width;
            this.heigth = heigth;
        }

        public override bool Meet(SingleObjectState From, SingleObjectState To, float obstacleRadi, float margin = 0f)
        {
            Vector2D<float> N = From.Location;
            Vector2D<float> T = To.Location;
            Vector2D<float>[] c = new VectorF2D[4];

            c[0] = new VectorF2D(state.Location.X - width, state.Location.Y - heigth);
            c[1] = new VectorF2D(state.Location.X + width, state.Location.Y - heigth);
            c[2] = new VectorF2D(state.Location.X + width, state.Location.Y + heigth);
            c[3] = new VectorF2D(state.Location.X - width, state.Location.Y + heigth);
            float d = 0;
            // check box against oriented sweep
            for (int i = 0; i < 4; i++)
            {
                d = VectorF2D.DistanceSegToSeg(N, T, c[i], c[(i + 1) % 4]);
                if (d < obstacleRadi + margin) return true;
            }
            return Meet(From, obstacleRadi, margin) || Meet(To, obstacleRadi, margin);
        }

        public override bool Meet(SingleObjectState S1, float obstacleRadi, float margin = 0f)
        {
            Vector2D<float> v = S1.Location - state.Location;
            return System.MathF.Abs(v.X) < width + obstacleRadi + margin && System.MathF.Abs(v.Y) < heigth + obstacleRadi + margin;
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
            : base(
                new SingleObjectState(FieldConfig.Default.OurGoalCenter - new VectorF2D(FieldConfig.Default.DefenceAreaHeight / 2, 0f)),
                FieldConfig.Default.DefenceAreaWidth,
                FieldConfig.Default.DefenceAreaHeight)
        {
        }
    }

    public class OppZoneObstacle : RectObstacle
    {
        public override ObstacleType Type => ObstacleType.OppZone;

        public OppZoneObstacle()
            : base(new SingleObjectState(
                FieldConfig.Default.OppGoalCenter + new VectorF2D(FieldConfig.Default.DefenceAreaHeight / 2, 0f)),
                FieldConfig.Default.DefenceAreaWidth + RobotConfig.Default.Radius * 2,
                FieldConfig.Default.DefenceAreaHeight + RobotConfig.Default.Radius * 2)
        {
        }
    }

    public class BallObstacle : CircleObstacle
    {
        public override ObstacleType Type => ObstacleType.Ball;

        public BallObstacle(SingleObjectState state) : base(state, MergerTrackerConfig.Default.BallRadi) { }
    }
}