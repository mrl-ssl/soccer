using System;
using System.Collections.Generic;
using System.Linq;
using MRL.SSL.Ai.Utils;

namespace MRL.SSL.Ai.Engine
{
    public abstract class PlayBase
    {
        protected RoleMatcher _roleMatcher = new RoleMatcher();
        public IDictionary<int, RoleBase> PreviouslyAssignedRoles { get; set; } = new Dictionary<int, RoleBase>();

        public abstract bool IsFeasiblel(GameStrategyEngine engine, WorldModel Model, PlayBase LastPlay, ref GameStatus Status);
        public abstract Dictionary<int, RoleBase> RunPlay(GameStrategyEngine engine, WorldModel Model, bool RecalculateRoles);
        protected virtual void RoleAssigner(GameStrategyEngine engine, WorldModel Model, IList<RoleInfo> rolesToAssign, ref IDictionary<int, RoleBase> currentlyAssignedRoles)
        {
            IDictionary<int, RoleBase> matchedRoles;
            if (Model.GoalieID.HasValue)
                matchedRoles = _roleMatcher.MatchRoles(engine, Model, Model.Teammates.Keys.Where(w => w != Model.GoalieID.Value).ToList(), rolesToAssign, PreviouslyAssignedRoles);
            else
                matchedRoles = _roleMatcher.MatchRoles(engine, Model, Model.Teammates.Keys.ToList(), rolesToAssign, PreviouslyAssignedRoles);
            foreach (var key in matchedRoles.Keys)
            {
                AssignRole(Model, key, matchedRoles[key].GetType(), ref currentlyAssignedRoles);
            }
        }

        protected virtual bool AssignRole(WorldModel Model, int robotID, Type RoleType, ref IDictionary<int, RoleBase> currentlyAssignedRoles)
        {
            RoleBase RoleToBeAssigned = RoleType.GetConstructor(new Type[] { }).Invoke(new object[] { }) as RoleBase;
            if (Model.Teammates.ContainsKey(robotID))
            {

                if (PreviouslyAssignedRoles != null && PreviouslyAssignedRoles.ContainsKey(robotID) && PreviouslyAssignedRoles[robotID].GetType() == RoleType)
                    currentlyAssignedRoles[robotID] = PreviouslyAssignedRoles[robotID];
                else
                    currentlyAssignedRoles[robotID] = RoleToBeAssigned;
                return true;
            }
            return false;
        }
        // public abstract PlayResult QueryPlayResult();

        public virtual void ResetPlay(WorldModel Model, GameStrategyEngine engine, IDictionary<int, RoleBase> lastPlayRoles)
        {
            if (lastPlayRoles != null)
                PreviouslyAssignedRoles = lastPlayRoles;
            else PreviouslyAssignedRoles.Clear();
        }
    }
}