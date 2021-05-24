using System.Collections.Generic;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public class RobotCommands
    {
        [ProtoMember(1)]
        public IDictionary<int, SingleWirelessCommand> Commands { get; set; }
        public RobotCommands()
        {
            Commands = new Dictionary<int, SingleWirelessCommand>();
        }
        public void AddCommand(int robotId, SingleWirelessCommand swc)
        {
            if (!Commands.ContainsKey(robotId))
                Commands.Add(robotId, swc);
            else
                Commands[robotId] = swc;
        }
    }
}