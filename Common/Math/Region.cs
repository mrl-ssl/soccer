using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Common.Math
{
    [ProtoContract]
    public class Region
    {
        [ProtoMember(1, IsRequired = true)]
        public List<VectorF2D> Positions { get; set; }

        public Region(List<VectorF2D> positions)
        {
            Positions = positions;
        }

        public Region(ICollection<VectorF2D> positions)
        {
            Positions = new List<VectorF2D>(positions);
        }

        public static implicit operator Region(List<VectorF2D> positions) => new Region(positions);
        public static implicit operator Region(VectorF2D[] positions) => new Region(positions);
    }
}