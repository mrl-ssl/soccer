using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Common
{
    [ProtoContract]
    public class RobotCommands
    {
        [ProtoMember(1)]
        public Dictionary<int, SingleWirelessCommand> Commands { get; set; }
        public RobotCommands()
        {
            Commands = new Dictionary<int, SingleWirelessCommand>();
        }
    }
}