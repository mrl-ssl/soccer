using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Ai.Utils
{
    [ProtoContract]
    public class Observation
    {
        public bool IsValid { get; set; }
        public int LastValid { get; set; }

        [ProtoMember(1)]
        public uint Camera { get; set; }

        [ProtoMember(2)]
        public float Confidence { get; set; }

        [ProtoMember(3)]
        public VectorF2D Location { get; set; }

        [ProtoMember(4)]
        public float Angle { get; set; }

        public Observation()
        {
        }
        public Observation(uint cam)
        {
            Camera = cam;
        }
        public Observation(VectorF2D loc, float angle, float conf, uint cam)
        {
            Location = loc;
            Angle = angle;
            Confidence = conf;
            Camera = cam;
        }
    }
}