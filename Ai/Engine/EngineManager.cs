using System;
using System.Threading;
using MRL.SSL.Common.Utils.Sockets;
using MRL.SSL.Common.Configuration;
using System.IO;
using System.Net;
using ProtoBuf;
using MRL.SSL.Common.SSLWrapperCommunication;
using MRL.SSL.Ai.MergerTracker;
using WatsonWebsocket;
using MRL.SSL.Common;
using System.Diagnostics;
using System.Net.WebSockets;
using MRL.SSL.Common.Math;
using MRL.SSL.Ai.Utils;

namespace MRL.SSL.Ai.Engine
{
    public class EngineManager : IDisposable
    {
        Thread _cmcThread;
        UdpClient _visionClient;
        WatsonWsServer _visualizerServer;
        CancellationTokenSource _cmcCancelationSource = new CancellationTokenSource();
        WorldGenerator worldGenerator;
        string visIpPort;

        public RobotCommands Commands { get; set; }
        public EngineManager()
        {

            Commands = new RobotCommands();
            _cmcThread = new Thread(new ParameterizedThreadStart(EngineManagerRun));
        }
        public SSLWrapperPacket RecieveVisionData()
        {
            //

            if (_visionClient == null)
                return null;

            long size = _visionClient.Receive();
            if (size == 0)
                return null;
            using var stream = new MemoryStream(_visionClient.ReceiveBuffer.Data, 0, (int)size);

            SSLWrapperPacket packet = Serializer.Deserialize<SSLWrapperPacket>(stream);
            return packet;

        }
        private void EngineManagerRun(object obj)
        {
            CancellationToken ct = (CancellationToken)obj;
            Console.WriteLine("Engine Mangaer Started!");
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var packet = RecieveVisionData();
                    if (packet != null)
                    {
                        var model = worldGenerator.GenerateWorldModel(packet, Commands, false, false);

                        if (model != null)
                        {
                            if (visIpPort != null)
                            {
                                using var stream = new MemoryStream();

                                Serializer.Serialize<WorldModel>(stream, model);

                                _visualizerServer.SendAsync(visIpPort, stream.ToArray(), WebSocketMessageType.Binary);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception thrown on Engine Manager run", ex.StackTrace);
                }
            }
            Console.WriteLine("Engine Mangaer Stopped!");
        }
        public void Initialize()
        {
            Console.WriteLine("Initializing Stuff...");

            worldGenerator = new();

            visIpPort = null;

            if (_visualizerServer != null)
            {
                if (_visualizerServer.IsListening)
                    _visualizerServer.Stop();
                _visualizerServer.Dispose();
            }
            _visualizerServer = new WatsonWsServer(ConnectionConfig.Default.VisualizerName, ConnectionConfig.Default.VisualizerPort, false);
            _visualizerServer.ClientConnected += Visualizer_OnClientConnected;
            _visualizerServer.ClientDisconnected += Visualizer_OnClientDisconnected;
            _visualizerServer.MessageReceived += Visualizer_OnMessageReceived;


            if (_visionClient != null)
                _visionClient.Dispose();

            _visionClient = new UdpClient(IPAddress.Any, ConnectionConfig.Default.VisionPort);
            _visionClient.Error += Vision_OnError;
            _visionClient.SetupMulticast(true);
            _visionClient.Connect();
            _visionClient.OptionReceiveTimeout = 2000;
            _visionClient.JoinMulticastGroup(ConnectionConfig.Default.VisionName);
        }
        public void Start()
        {
            try
            {
                Console.WriteLine("Starting Websocket...");
                _visualizerServer.Start();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Failed Starting Websocket: " + e);
            }
            Console.WriteLine("Websocket IsListening:" + _visualizerServer.IsListening);
            Console.WriteLine("Starting CMC Thread...");
            _cmcThread.Start(_cmcCancelationSource.Token);
        }
        public void Dispose()
        {
            Console.WriteLine("Stopping CMC Thread...");
            _cmcCancelationSource.Cancel();
            _cmcThread.Join();
            _cmcCancelationSource.Dispose();

            Console.WriteLine("Stopping Vision Socket...");
            _visionClient.DisconnectAndStop();
            _visionClient.Error -= Vision_OnError;

            Console.WriteLine("Stopping Websocket...");
            if (_visualizerServer.IsListening)
                _visualizerServer.Stop();
            _visualizerServer.ClientConnected -= Visualizer_OnClientConnected;
            _visualizerServer.ClientDisconnected -= Visualizer_OnClientDisconnected;
            _visualizerServer.MessageReceived -= Visualizer_OnMessageReceived;
            _visualizerServer.Dispose();

            visIpPort = null;

        }
        private static void Vision_OnError(object sender, System.Net.Sockets.SocketError e)
        {
            Console.WriteLine($"Echo Vision UDP server caught an error with code {e}");
        }
        private void Visualizer_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Client Connected", e.IpPort);
        }
        private void Visualizer_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            visIpPort = null;
            Console.WriteLine("Client Connected", e.IpPort);
        }
        private void Visualizer_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            visIpPort = e.IpPort;

            Console.WriteLine("Client Disconnected", e.IpPort);
        }


    }
}