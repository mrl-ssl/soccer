using System;
using System.Net.Sockets;
using System.Text;
using MRL.SSL.Common.Utils.Sockets;
using System.Threading;
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

        private static void OnReceived(object sender, UdpReceivedEventArgs e)
        {

            Console.WriteLine("Incoming: " + Encoding.UTF8.GetString(e.Buffer, (int)e.Offset, (int)e.Size));
            Console.WriteLine("current Thread: " + Thread.CurrentThread.ManagedThreadId);
            // Echo the message back to the sender
            ((UdpServer)sender).ReceiveAsync();
            ((UdpServer)sender).SendAsync(e.EndPoint, e.Buffer, 0, e.Size);
        }

        private static void OnSent(object sender, UdpSentEventArgs e)
        {
            // Console.WriteLine("current Thread: " + Thread.CurrentThread.ManagedThreadId);

            // ((UdpServer)sender).ReceiveAsync();
        }

        private static void OnError(object sender, SocketError e)
        {
            Console.WriteLine($"Echo UDP server caught an error with code {e}");
        }


        private static void OnStarted(object sender, EventArgs e)
        {
            ((UdpServer)sender).ReceiveAsync();
        }

    }
}
