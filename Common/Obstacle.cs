using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
{
    public class Obstacle
    {
        ObstacleType type;
        VectorF2D reagion;
        SingleObjectState state;

        public ObstacleType Type { get => type; set => type = value; }
        /// <summary>
        /// length and width of obstacle region
        /// for circle obstacles size of this vector used
        /// </summary>
        public VectorF2D Reagion { get => reagion; set => reagion = value; }
        public SingleObjectState State { get => state; set => state = value; }

        public bool Meet(SingleObjectState From, SingleObjectState To, float obstacleRadi)
        {
            if (IsCircle())
                return MeetCircle(From, To, obstacleRadi);
            else
                return MeetRectangle(From, To, obstacleRadi);
        }

        public bool Meet(SingleObjectState S1, float obstacleRadi)
        {
            // TODO: must check for rectangle
            Vector2D<float> v = S1.Location - state.Location;
            return v.Length() < obstacleRadi + reagion.X;
        }

        private bool MeetRectangle(SingleObjectState From, SingleObjectState To, float obstacleRadi)
        {
            VectorF2D N = From.Location;
            VectorF2D T = To.Location;
            VectorF2D[] c = new VectorF2D[4];

            c[0] = new VectorF2D(state.Location.X - reagion.X, state.Location.Y - reagion.Y);
            c[1] = new VectorF2D(state.Location.X + reagion.X, state.Location.Y - reagion.Y);
            c[2] = new VectorF2D(state.Location.X + reagion.X, state.Location.Y + reagion.Y);
            c[3] = new VectorF2D(state.Location.X - reagion.X, state.Location.Y + reagion.Y);
            float d = 0;
            // check box against oriented sweep
            for (int i = 0; i < 4; i++)
            {
                d = VectorF2D.DistanceSegToSeg(N, T, c[i], c[(i + 1) % 4]);
                if (d < obstacleRadi) return (true);
            }
            return (Meet(new SingleObjectState(N), obstacleRadi) || Meet(new SingleObjectState(T), obstacleRadi));
        }
        private bool MeetCircle(SingleObjectState From, SingleObjectState To, float obstacleRadi)
        {
            Vector2D<float> p = VectorF2D.PointOnSegment(From.Location, To.Location, state.Location);
            float d = p.Distance(state.Location);
            return d <= reagion.X + obstacleRadi;
        }

        private bool IsCircle() =>
            type == ObstacleType.Circle ||
            type == ObstacleType.OurRobot ||
            type == ObstacleType.OppRobot ||
            type == ObstacleType.ZoneCircle ||
            type == ObstacleType.Ball;
    }

    public enum ObstacleType
    {
        Circle,
        Rectangle,
        OurRobot,
        OppRobot,
        Ball,
        ZoneCircle,
        ZoneRectangle
    }
}