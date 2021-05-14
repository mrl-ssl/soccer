using MRL.SSL.Common.SSLWrapperCommunication;

namespace MRL.SSL.Common
{
    public class RefereeCommand
    {
        public RefereeCommand(SSLRefereePacket packet)
        {
            RefereePacket = packet;
        }
        public RefereeCommand(char c)
        {
            Command = c;
        }
        public char Command { get; set; }
        public SSLRefereePacket RefereePacket { get; set; }

    }
}