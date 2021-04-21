using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{

    [ProtoContract]
    public class SSLFieldLineSegment
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public VectorF2D P1 { get; set; }

        [ProtoMember(3)]
        public VectorF2D P2 { get; set; }

        [ProtoMember(4)]
        public float Thickness { get; set; }

        [ProtoMember(5)]
        public SSLFieldShapeType Type { get; set; }

    }
}