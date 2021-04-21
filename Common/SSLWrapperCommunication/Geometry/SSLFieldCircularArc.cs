using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class SSLFieldCircularArc
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public VectorF2D Center { get; set; }

        [ProtoMember(3)]
        public float Radius { get; set; }

        [ProtoMember(4)]
        public float A1 { get; set; }

        [ProtoMember(5)]
        public float A2 { get; set; }

        [ProtoMember(6)]
        public float Thickness { get; set; }

        [ProtoMember(7)]
        public SSLFieldShapeType Type { get; set; }

    }
}