using System;
using MRL.SSL.Ai.Engine;
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
            em.Initialize();
            em.Start();

            for (; ; )
            {
                string line = Console.ReadLine();


                // Restart the server
                if (line == "!")
                {
                    em.Dispose();
                    Console.WriteLine("Done!");
                    break;
                }
            }
        }


    }
}
