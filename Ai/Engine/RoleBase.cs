using MRL.SSL.Ai.Utils;
using MRL.SSL.Common.SSLWrapperCommunication;
using System.Collections.Generic;
using MRL.SSL.Common.Utils.Extensions;
using System;

namespace MRL.SSL.Ai.Engine
{
    public abstract class RoleBase
    {
        private static IDictionary<string, object> sharedData = new Dictionary<string, object>();
        public T GetSharedData<T>() where T : new()
        {
            if (!sharedData.ContainsKey(Key))
                sharedData.Add(Key, new T());
            return sharedData[Key].As<T>();
        }
        protected RoleBase(int id) { ID = id; }
        public int ID { get; protected set; }
        public virtual string Key { get { return GetType().ToString() + ID.ToString(); } }
        public abstract RoleCategory Category { get; }

        public int LastState { get; protected set; }
        public int CurrentState { get; protected set; }
        public SkillBase CurrentSkill { get; protected set; }
        protected T GetSkill<T>() where T : SkillBase, new()
        {
            if (CurrentSkill != null && CurrentSkill is T)
                return CurrentSkill as T;
            else
            {
                T t = new T();
                CurrentSkill = t;
                return t;
            }
        }

        public abstract Func<SingleWirelessCommand> Run(GameStrategyEngine engine, WorldModel model, int robotId, IDictionary<int, RoleBase> assignedRoles);
        public abstract void DetermineNextState(GameStrategyEngine engine, WorldModel model, int robotId, IDictionary<int, RoleBase> assignedRoles);
        public abstract float CalculateCost(GameStrategyEngine engine, WorldModel model, int robotId, IDictionary<int, RoleBase> previouslyAssignedRoles);
        public abstract IList<RoleBase> SwichToRole(GameStrategyEngine engine, WorldModel model, int robotId, IDictionary<int, RoleBase> previouslyAssignedRoles);

        public void ResetState()
        {
            CurrentSkill = null;
        }
    }
    public enum RoleCategory
    {
        Goalie,
        Defender,
        Active,
        Positioner,
        Test
    }
}