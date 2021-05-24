using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class SingleWirelessCommand
    {
        [ProtoMember(1, IsRequired = true)]
        public float Vx { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public float Vy { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public float W { get; set; }
    }
}