using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class SSLWrapperPacket
    {
        [ProtoMember(1)]
        public SSLDetectionFrame Detection { get; set; }

        [ProtoMember(2)]
        public SSLGeometryData Geometry { get; set; }

    }
}