using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public class SingleWirelessCommand
    {
        [ProtoMember(1)]
        public float Vx { get; set; }

        [ProtoMember(2)]
        public float Vy { get; set; }

        [ProtoMember(3)]
        public float W { get; set; }
    }
}