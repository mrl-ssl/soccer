
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
{
    public class RawObjectState
    {
        private VectorF2D location;
        private float angle;

        public VectorF2D Location { get => location; set => location = value; }
        public float Angle { get => angle; set => angle = value; }

        public RawObjectState(float x, float y)
        {
            location = new VectorF2D(x, y);
        }

        public RawObjectState(float x, float y, float theta)
        {
            location = new VectorF2D(x, y);
            angle = theta;
        }
        public RawObjectState(VectorF2D loc)
        {
            location = loc;
        }

    }
}