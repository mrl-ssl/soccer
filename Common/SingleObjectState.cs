using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public class SingleObjectState
    {
        private SingleObjectState parent;
        private SingleObjectState child;

        private Vector2D<float> location;
        private Vector2D<float> speed;
        private float angle;
        private float angularSpeed;
        private float stuck;

        private int? battery;

        public SingleObjectState Parent { get => parent; set => parent = value; }
        public SingleObjectState Child { get => child; set => child = value; }

        [ProtoMember(1)]
        public Vector2D<float> Location { get => location; set => location = value; }

        [ProtoMember(2)]
        public Vector2D<float> Speed { get => speed; set => speed = value; }

        [ProtoMember(3)]
        public float Angle { get => angle; set => angle = value; }

        [ProtoMember(4)]
        public float AngularSpeed { get => angularSpeed; set => angularSpeed = value; }

        [ProtoMember(5)]
        public int? Battery { get => battery; set => battery = value; }

        [ProtoMember(6)]
        public float Stuck { get => stuck; set => stuck = value; }

        public SingleObjectState()
        {

        }

        public SingleObjectState(float x, float y)
        {
            location = new VectorF2D(x, y);
        }

        public SingleObjectState(float x, float y, float theta)
        {
            location = new VectorF2D(x, y);
            angle = theta;
        }
        public SingleObjectState(Vector2D<float> loc)
        {
            location = loc;
        }
        public SingleObjectState(Vector2D<float> loc, Vector2D<float> sp)
        {
            location = loc;
            speed = sp;
        }
        public SingleObjectState(Vector2D<float> loc, Vector2D<float> sp, float stuck)
        {
            location = loc;
            speed = sp;
            this.stuck = stuck;
        }
        public SingleObjectState(Vector2D<float> loc, float theta)
        {
            location = loc;
            angle = theta;
        }
        public SingleObjectState(Vector2D<float> loc, Vector2D<float> sp, float theta, float w)
        {
            location = loc;
            speed = sp;
            angle = theta;
            angularSpeed = w;
        }

        public SingleObjectState(Vector2D<float> loc, Vector2D<float> sp, float theta, float w, float stuck)
        {
            location = loc;
            speed = sp;
            angle = theta;
            angularSpeed = w;
            this.stuck = stuck;
        }

        public float DistanceFrom(SingleObjectState state) => location.Distance(state.location);
    }
}