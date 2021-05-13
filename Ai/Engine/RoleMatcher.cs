
using System;
using System.Collections.Generic;
using System.Linq;
using MRL.SSL.Ai.Utils;

namespace MRL.SSL.Ai.Engine
{
    public class RoleMatcher
    {
        float[,] costs;
        int count;
        IList<int> remainingRoleIndices;
        IDictionary<int, int> bestMatch;
        float bestMatchCost;
        int[] currentList;
        float currentCost;
        public RoleMatcher()
        {
            remainingRoleIndices = new List<int>();
        }

        public IDictionary<int, RoleBase> MatchRoles(GameStrategyEngine engine, WorldModel model, IList<int> robotIDs, IList<RoleInfo> rolesToBeAssigned, IDictionary<int, RoleBase> previouslyAssignedRoles)
        {
            var newRoles = new Dictionary<int, RoleBase>();
            count = Math.Min(robotIDs.Count, rolesToBeAssigned.Count);
            costs = new float[count, count];
            //
            var rolesSwitch = new float[count, count];
            var rolesNotAssignedInSwitchMode = previouslyAssignedRoles.ToDictionary(p => p.Key, p => p.Value);
            var RobotsIDsNotAssignedInSwitch = robotIDs.ToDictionary(p => p, v => false);
            if (previouslyAssignedRoles.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var roleInRow = rolesToBeAssigned[i];

                    if (previouslyAssignedRoles.Any(p => p.Value.Key == roleInRow.Role.Key))
                    {

                        int robotId = previouslyAssignedRoles.Where(p => p.Value.Key == roleInRow.Role.Key).First().Key;

                        int IndexOfRobotID = robotIDs.IndexOf(robotId);


                        if (IndexOfRobotID != -1 && IndexOfRobotID < count)
                        {
                            rolesNotAssignedInSwitchMode.Remove(robotId);
                            RobotsIDsNotAssignedInSwitch[robotId] = true;

                            IList<RoleBase> switches = roleInRow.Role.SwichToRole(engine, model, robotId, previouslyAssignedRoles);
                            if (switches.Count > 0)
                            {
                                for (int j = 0; j < count; j++)
                                {
                                    RoleInfo roleInColumn = rolesToBeAssigned[j];
                                    if (!switches.Any(p => p.Key == roleInColumn.Role.Key))
                                    {
                                        rolesSwitch[IndexOfRobotID, j] = 1000;
                                    }
                                    else
                                    {
                                        rolesSwitch[IndexOfRobotID, j] = 0;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < count; j++)
                                {
                                    rolesSwitch[IndexOfRobotID, j] = 1000;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < count; j++)
                            {
                                rolesSwitch[i, j] = 0;
                            }
                        }
                    }

                }


                foreach (var item in rolesNotAssignedInSwitchMode.Keys)
                {
                    int IndexOfRobotID = robotIDs.IndexOf(item);
                    if (IndexOfRobotID != -1 && IndexOfRobotID < count)
                    {
                        RobotsIDsNotAssignedInSwitch[item] = true;

                        IList<RoleBase> switches = rolesNotAssignedInSwitchMode[item].SwichToRole(engine, model, item, previouslyAssignedRoles);
                        if (switches.Count > 0)
                        {
                            for (int j = 0; j < count; j++)
                            {
                                var roleInColumn = rolesToBeAssigned[j];
                                if (!switches.Any(p => p.Key == roleInColumn.Role.Key))
                                {
                                    rolesSwitch[IndexOfRobotID, j] = 1000;
                                }
                                else
                                {
                                    rolesSwitch[IndexOfRobotID, j] = 0;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < count; j++)
                            {
                                rolesSwitch[IndexOfRobotID, j] = 1000;
                            }
                        }
                    }
                    else
                    {
                        //foreach (var item2 in robotIDs)
                        for (int i = 0; i < count; i++)
                        {
                            int item2 = robotIDs[i];

                            if (RobotsIDsNotAssignedInSwitch[item2] == false)
                            {
                                for (int j = 0; j < count; j++)
                                {
                                    int ii = robotIDs.IndexOf(item2);
                                    rolesSwitch[ii, j] = 0;
                                }
                                RobotsIDsNotAssignedInSwitch[item2] = true;
                            }
                        }
                    }
                }
                //foreach (var item in robotIDs)
                for (int i = 0; i < count; i++)
                {
                    int item = robotIDs[i];
                    if (RobotsIDsNotAssignedInSwitch[item] == false)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            int ii = robotIDs.IndexOf(item);
                            rolesSwitch[ii, j] = 0;
                        }
                        RobotsIDsNotAssignedInSwitch[item] = true;
                    }
                }
            }


            for (int i = 0; i < count; i++)
            {
                int robotID = robotIDs[i];
                for (int j = 0; j < count; j++)
                {
                    var roleInfo = rolesToBeAssigned[j];
                    RoleBase prev = null;
                    bool hadRole = previouslyAssignedRoles.TryGetValue(robotID, out prev);
                    costs[i, j] = (roleInfo.Role.CalculateCost(engine, model, robotID, previouslyAssignedRoles) + ((hadRole && prev.Key == roleInfo.Role.Key) ? -roleInfo.Margin : 0)) * roleInfo.Weight + rolesSwitch[i, j];
                }
            }

            bestMatch = new Dictionary<int, int>();
            bestMatchCost = float.MaxValue;
            remainingRoleIndices.Clear();
            for (int i = 0; i < count; i++)
                remainingRoleIndices.Add(i);
            currentList = new int[count];
            currentCost = 0;
            findBestMatch(count - 1);

            for (int i = 0; i < count; i++)
            {
                int robotID = robotIDs[i];
                RoleBase last, newrole;
                if (previouslyAssignedRoles.TryGetValue(robotID, out last) && last.Key == rolesToBeAssigned[bestMatch[i]/*i*/].Role.Key)
                    newrole = last;
                else
                {
                    newrole = rolesToBeAssigned[bestMatch[i]].Role;
                    newrole.ResetState();
                }
                newRoles.Add(robotID, newrole);
            }
            return newRoles;
        }
        private void findBestMatch(int index)
        {
            for (int i = 0; i < remainingRoleIndices.Count; i++)
            {
                currentList[index] = remainingRoleIndices[i];
                currentCost += costs[index, currentList[index]];
                if (index == 0)
                {
                    if (currentCost < bestMatchCost)
                    {
                        bestMatchCost = currentCost;
                        bestMatch.Clear();
                        for (int j = 0; j < count; j++)
                            bestMatch.Add(j, currentList[j]);
                    }
                }
                else
                {
                    remainingRoleIndices.RemoveAt(i);
                    findBestMatch(index - 1);
                    remainingRoleIndices.Insert(i, currentList[index]);
                }
                currentCost -= costs[index, currentList[index]];
            }
        }
    }
}

