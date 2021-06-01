using System;
using MRL.SSL.Ai.Engine;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common.SSLWrapperCommunication;

namespace MRL.SSL.Ai.SkillBook
{
    public class HaltSkill : SkillBase
    {
        public Func<SingleWirelessCommand> Run(WorldModel model, int robotId)
        {
            return () => new SingleWirelessCommand() { KickSpeed = robotId == 3 ? 6.5f : 0, KickAngle = 45f };
        }
    }
}