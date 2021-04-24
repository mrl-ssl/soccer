using MRL.SSL.Common.Math;

namespace MRL.SSL.Common
{
    public class RawObjectState
    {
        public float Angle { get; set; }
        public VectorF2D Location { get; set; }
        private int lastSeen = -1;
        public int LastSeen
        {
            get { return lastSeen; }
            set { lastSeen = value; }
        }

        public int Camera { get; set; }
        public double Time { get; set; }
        public float Confidence { get; set; }

        public RawObjectState()
        {

        }

        public RawObjectState(float x, float y)
        {
            Location = new VectorF2D(x, y);
        }

        public RawObjectState(float x, float y, float theta)
        {
            Location = new VectorF2D(x, y);
            Angle = theta;
        }
        public RawObjectState(float x, float y, float theta, float conf, double time, int cam)
        {
            Location = new VectorF2D(x, y);
            Angle = theta;
            Camera = cam;
            Confidence = conf;
            Time = time;
        }
        public RawObjectState(VectorF2D loc)
        {
            Location = loc;
        }

        public RawObjectState(VectorF2D loc, float theta)
        {
            Location = loc;
            Angle = theta;
        }

    }
}