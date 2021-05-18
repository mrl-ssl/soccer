using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public class Circle
    {
        [ProtoMember(1, IsRequired = true)]
        public VectorF2D Position { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public float Radius { get; set; }

        public Circle(VectorF2D position, float radius)
        {
            Position = position;
            Radius = radius;
        }
    }
}