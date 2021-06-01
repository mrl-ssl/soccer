using System;
using System.Collections.Generic;
using MRL.SSL.Ai.Engine;
using MRL.SSL.Ai.SkillBook;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;

namespace MRL.SSL.Ai.RoleBook
{
    public class HaltRole : RoleBase
    {
        public HaltRole(int id) : base(id)
        {

        }

        public override RoleCategory Category => RoleCategory.Positioner;

        public override float CalculateCost(GameStrategyEngine engine, WorldModel model, int robotId, IDictionary<int, RoleBase> previouslyAssignedRoles)
        {
            throw new NotImplementedException();
        }

        public override void DetermineNextState(GameStrategyEngine engine, WorldModel model, int robotId, IDictionary<int, RoleBase> assignedRoles)
        {

        }

        public override Func<SingleWirelessCommand> Run(GameStrategyEngine engine, WorldModel model, int robotId, IDictionary<int, RoleBase> assignedRoles)
        {
            //     // if (robotId == 3)
            //     return GetSkill<GotoPointSkill>().Go(engine, model, robotId, new VectorF2D(-3.9f, 0f), true, true, true, true,
            //                                   true, true);
            return GetSkill<HaltSkill>().Run();
        }

        public override IList<RoleBase> SwichToRole(GameStrategyEngine engine, WorldModel model, int robotId, IDictionary<int, RoleBase> previouslyAssignedRoles)
        {
            throw new NotImplementedException();
        }
    }
}