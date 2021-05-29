using System;
using System.Collections.Generic;
using System.Linq;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common;
using MRL.SSL.Common.Utils.Extensions;
namespace MRL.SSL.Ai.Engine
{
    public enum PlayResult
    {
        InPlay,
        Aborted,
        Success,
        Fail,
        Completed
    }
    public abstract class PlayBase
    {
        protected RoleMatcher _roleMatcher = new RoleMatcher();
        public Dictionary<int, RoleBase> PreviouslyAssignedRoles { get; set; } = new Dictionary<int, RoleBase>();

        public abstract bool IsFeasiblel(GameStrategyEngine engine, WorldModel Model, PlayBase LastPlay, ref GameStatus Status);
        public abstract Dictionary<int, RoleBase> RunPlay(GameStrategyEngine engine, WorldModel Model, bool RecalculateRoles);
        protected virtual void RoleAssigner(GameStrategyEngine engine, WorldModel Model, IList<RoleInfo> rolesToAssign, ref Dictionary<int, RoleBase> currentlyAssignedRoles)
        {
            IDictionary<int, RoleBase> matchedRoles;
            if (Model.GoalieID.HasValue)
                matchedRoles = _roleMatcher.MatchRoles(engine, Model, Model.Teammates.Keys.Where(w => w != Model.GoalieID.Value).ToList(), rolesToAssign, PreviouslyAssignedRoles);
            else
                matchedRoles = _roleMatcher.MatchRoles(engine, Model, Model.Teammates.Keys.ToList(), rolesToAssign, PreviouslyAssignedRoles);
            foreach (var key in matchedRoles.Keys)
            {
                AssignRole(Model, key, matchedRoles[key].GetType(), matchedRoles[key].Key, ref currentlyAssignedRoles);
            }
        }

        protected virtual bool AssignRole(WorldModel Model, int robotID, Type roleType, string key, ref Dictionary<int, RoleBase> currentlyAssignedRoles)
        {
            if (Model.Teammates.ContainsKey(robotID))
            {
                if (PreviouslyAssignedRoles != null && PreviouslyAssignedRoles.ContainsKey(robotID) && PreviouslyAssignedRoles[robotID].Key == key)
                    currentlyAssignedRoles[robotID] = PreviouslyAssignedRoles[robotID];
                else
                    currentlyAssignedRoles[robotID] = roleType.GetConstructor(new Type[] { robotID.GetType() }).GetActivator<RoleBase>(robotID);
                return true;
            }
            return false;
        }
        protected virtual bool AssignRole(WorldModel Model, int robotID, RoleBase role, ref Dictionary<int, RoleBase> currentlyAssignedRoles)
        {
            if (Model.Teammates.ContainsKey(robotID))
            {
                if (PreviouslyAssignedRoles != null && PreviouslyAssignedRoles.ContainsKey(robotID) && PreviouslyAssignedRoles[robotID].Key == role.Key)
                    currentlyAssignedRoles[robotID] = PreviouslyAssignedRoles[robotID];
                else
                    currentlyAssignedRoles[robotID] = role;
                return true;
            }
            return false;
        }
        public abstract PlayResult QueryPlayResult();

        public virtual void ResetPlay(WorldModel Model, GameStrategyEngine engine, Dictionary<int, RoleBase> lastPlayRoles)
        {
            if (lastPlayRoles != null)
                PreviouslyAssignedRoles = lastPlayRoles;
            else PreviouslyAssignedRoles.Clear();
        }
    }
}