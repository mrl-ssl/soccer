using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class SSLDetectionRobot
    {
        [ProtoMember(1)]
        public float Confidence { get; set; }

        [ProtoMember(2)]
        public uint? Id { get; set; }

        [ProtoMember(3)]
        public float X { get; set; }

        [ProtoMember(4)]
        public float Y { get; set; }

        [ProtoMember(5)]
        public float? Orientation { get; set; }

        [ProtoMember(6)]
        public float PixelX { get; set; }

        [ProtoMember(7)]
        public float PixelY { get; set; }

        [ProtoMember(8)]
        public float? Height { get; set; }
    }
}