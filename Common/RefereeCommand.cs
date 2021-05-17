using MRL.SSL.Common.SSLWrapperCommunication;
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public enum RefereeSourceType
    {
        Unknown = 0,
        Refbox = 1,
        CommandLine = 2,
        Visualizer = 3
    }
    [ProtoContract]
    public class RefereeCommand
    {
        public RefereeCommand(SSLRefereePacket packet, RefereeSourceType src)
        {
            RefereePacket = packet;
            Source = src;
        }
        [ProtoMember(1, IsRequired = true)]
        public RefereeSourceType Source { get; set; }

        [ProtoMember(2)]
        public SSLRefereePacket RefereePacket { get; set; }

    }
}