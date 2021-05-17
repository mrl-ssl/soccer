using System;
using System.Collections.Generic;
using System.Reflection;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common;

namespace MRL.SSL.Ai.Engine
{
    public class GameStrategyEngine : IDisposable
    {
        private PlayBase[] implementedPlays;
        private Dictionary<Type, RoleBase> implementedRoles;
        // PlayBase lastRunningPlay;
        Random rnd;
        public GameStatus Status { get; set; }
        public int EngineId { get; private set; }
        public RefereeCommand RefereeCommand { get; internal set; }

        public GameStrategyEngine(int id)
        {
            EngineId = id;
            var pb = new List<PlayBase>();
            Type[] types = System.Reflection.Assembly.GetAssembly(typeof(GameStrategyEngine)).GetTypes();
            implementedRoles = new Dictionary<Type, RoleBase>();

            // ImplementedStrategies = new List<StrategyBase>();
            foreach (Type t in types)
            {
                if (t.IsClass && t.IsSubclassOf(typeof(PlayBase)))
                    pb.Add(t.GetConstructor(new Type[] { }).Invoke(new object[] { }) as PlayBase);
                else if (t.IsClass && t.IsSubclassOf(typeof(RoleBase)))
                    implementedRoles.Add(t, t.GetConstructor(BindingFlags.Instance | BindingFlags.Default | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(new object[] { }) as RoleBase);
            }
            implementedPlays = pb.ToArray();
            rnd = new Random();
        }


        public void PlayGame(WorldModel Model, bool StrategyChanged)
        { }

        public void Dispose()
        {

        }
    }
}