using System;
using MRL.SSL.Ai.Engine;
using MRL.SSL.Common.SSLWrapperCommunication;

namespace MRL.SSL.Ai.SkillBook
{
    public class HaltSkill : SkillBase
    {
        public Func<SingleWirelessCommand> Run()
        {
            return () => new SingleWirelessCommand();
        }
    }
}