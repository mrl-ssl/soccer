using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Ai.Utils.Threading;
using MRL.SSL.Common;
using System.Linq;

namespace MRL.SSL.Ai.Engine
{
    public class GameStrategyEngine : IDisposable
    {
        private PlayBase[] implementedPlays;
        private Dictionary<Type, RoleBase> implementedRoles;
        private Dictionary<int, RoleBase> assignedroles;
        // PlayBase lastRunningPlay;
        Random rnd;
        public GameStatus Status { get; set; }
        public int EngineId { get; private set; }
        public RefereeCommand RefereeCommand { get; internal set; }
        public PlayBase LastRunningPlay { get; set; }
        TaskThreadPool taskScheduler;


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
            assignedroles = new();
            rnd = new Random();
            taskScheduler = new TaskThreadPool(Environment.ProcessorCount, false);
        }


        public RobotCommands PlayGame(WorldModel Model)
        {
            var status = Model.Status;
            PlayBase selectedplay = null;
            if (LastRunningPlay == null
                || !LastRunningPlay.IsFeasiblel(this, Model, LastRunningPlay, ref status)
                || LastRunningPlay.QueryPlayResult() != PlayResult.InPlay)
            {
                List<PlayBase> feasibleplays = new List<PlayBase>();
                foreach (PlayBase p in implementedPlays)
                {
                    if (p.IsFeasiblel(this, Model, LastRunningPlay, ref status))
                        feasibleplays.Add(p);
                }
                if (feasibleplays.Count == 0)
                    //TODO Implement enough plays to span the state space, so we'll never see this error.
                    throw new Exception("No Plays are feasible");
                selectedplay = feasibleplays[rnd.Next(0, feasibleplays.Count)];

            }
            Model.Status = status;
            Status = status;
            if (selectedplay != null)
            {
                selectedplay.ResetPlay(Model, this, assignedroles);
                LastRunningPlay = selectedplay;
            }

            assignedroles = LastRunningPlay.RunPlay(this, Model, selectedplay != null);
            var tasks = new Task<SingleWirelessCommand>[assignedroles.Count];
            var ids = new Dictionary<int, int>();
            int i = 0;

            foreach (int RobotID in assignedroles.Keys)
            {
                assignedroles[RobotID].DetermineNextState(this, Model, RobotID, assignedroles);
                tasks[i] = taskScheduler.Run(assignedroles[RobotID].Run(this, Model, RobotID, assignedroles));
                ids.Add(i++, RobotID);
            }
            Task.WaitAll(tasks);
            RobotCommands rc = new RobotCommands();
            for (int j = 0; j < tasks.Length; j++)
            {
                rc.AddCommand(ids[j], tasks[j].Result);
            }
            return rc;
        }

        public void Dispose()
        {
            taskScheduler.Dispose();
        }
    }
}