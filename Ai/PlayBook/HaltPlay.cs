using System.Collections.Generic;
using MRL.SSL.Ai.Engine;
using MRL.SSL.Ai.RoleBook;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common;

namespace MRL.SSL.Ai.PlayBook
{
    public class HaltPlay : PlayBase
    {
        public override bool IsFeasiblel(GameStrategyEngine engine, WorldModel Model, PlayBase LastPlay, ref GameStatus Status)
        {
            return Status == GameStatus.Halt;
        }

        public override PlayResult QueryPlayResult()
        {
            return PlayResult.InPlay;
        }

        public override Dictionary<int, RoleBase> RunPlay(GameStrategyEngine engine, WorldModel Model, bool RecalculateRoles)
        {
            Dictionary<int, RoleBase> currentlyAssignedRoles = new Dictionary<int, RoleBase>();
            foreach (var item in Model.Teammates.Keys)
            {
                AssignRole(Model, item, new HaltRole(item), ref currentlyAssignedRoles);
            }

            PreviouslyAssignedRoles = currentlyAssignedRoles;

            return currentlyAssignedRoles;
        }
    }
}