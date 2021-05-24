using System;
using MRL.SSL.Ai.Engine;
using MRL.SSL.Common.SSLWrapperCommunication;
using MRL.SSL.Common.Configuration;

namespace MRL.SSL.Ai
{

    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationLoader.Load();
            ConfigurationLoader.Load("Ai");

            var em = new EngineManager();
            var rm = new RefereeManager();

            em.Initialize();
            em.Start();

            rm.InitialConnections();
            rm.Start();

            for (; ; )
            {
                string line = Console.ReadLine();

                // Stop the server
                if (line == "!")
                {
                    rm.Dispose();
                    em.Dispose();

                    Console.WriteLine("Done!");
                    break;
                }
                else if (line.Length == 1)
                {
                    em.EnqueueRefereePacket(line[0], RefereeSourceType.CommandLine);
                }
            }
        }
    }
}
