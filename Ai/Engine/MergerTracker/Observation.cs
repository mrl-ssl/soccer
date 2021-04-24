using MRL.SSL.Common.Math;

namespace MRL.SSL.Ai.MergerTracker
{
    public class Observation
    {
        public bool IsValid { get; set; }
        public int LastValid { get; set; }
        public double Time { get; set; }
        public float Confidence { get; set; }
        public VectorF2D Location { get; set; }
        public float Angle { get; set; }

        public Observation()
        {

        }
        public Observation(VectorF2D loc, float angle, float conf, double time)
        {
            Location = loc;
            Angle = angle;
            Confidence = conf;
            Time = time;
        }
    }
}