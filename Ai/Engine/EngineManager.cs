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
using System.Net.WebSockets;
using MRL.SSL.Ai.Utils;
using System.Collections.Generic;
using MRL.SSL.Common.Utils.Extensions;

namespace MRL.SSL.Ai.Engine
{
    public sealed class EngineManager : IDisposable
    {
        /// <summary>
        /// an instance of this class 
        /// </summary>
        static EngineManager _manager;
        internal static EngineManager Manager
        {
            get { return EngineManager._manager; }
        }
        /// <summary>
        /// 
        /// </summary>
        ReaderWriterLockSlim _refereeCommandsLock = new ReaderWriterLockSlim();
        /// <summary>
        /// 
        /// </summary>
        Queue<RefereeCommand> refereeCommands;
        Thread _cmcThread;
        UdpClient _visionClient;
        WatsonWsServer _visualizerServer;
        CancellationTokenSource _cmcCancelationSource = new CancellationTokenSource();
        WorldGenerator worldGenerator;
        string visIpPort;
        bool isJoinedVisionMulticastGroup;
        private GameStrategyEngine gameEngine;
        public GameStrategyEngine GameEngine
        {
            get { return gameEngine; }
            private set { gameEngine = value; }
        }


        public RobotCommands Commands { get; set; }
        public EngineManager()
        {
            Commands = new RobotCommands();
            refereeCommands = new Queue<RefereeCommand>();
            _cmcThread = new Thread(new ParameterizedThreadStart(EngineManagerRun));
            _manager = this;
            gameEngine = new GameStrategyEngine(0);
        }

        public void EnqueueRefereePacket(RefereeCommand command)
        {
            _refereeCommandsLock.EnterWriteLock();
            refereeCommands.Enqueue(command);
            _refereeCommandsLock.ExitWriteLock();
        }
        private SSLVisionPacket RecieveVisionData()
        {
            if (_visionClient == null)
                return null;

            var size = _visionClient.Receive();
            if (size == 0)
                return null;

            using var stream = new MemoryStream(_visionClient.ReceiveBuffer.Data, 0, (int)size);

            return Serializer.Deserialize<SSLVisionPacket>(stream);
        }

        private void SendVisualizerData(SSLRefereePacket referee, WorldModel model)
        {
            using var stream = new MemoryStream();

            Serializer.SerializeWithLengthPrefix<WorldModel>(stream, model, PrefixStyle.Base128, 1);
            if (GameParameters.IsUpdated)
            {
                Serializer.SerializeWithLengthPrefix<FieldConfig>(stream, GameParameters.Field, PrefixStyle.Base128, 2);
                GameParameters.IsUpdated = false;
            }
            if (referee != null)
                Serializer.SerializeWithLengthPrefix<SSLRefereePacket>(stream, referee, PrefixStyle.Base128, 3);

            _visualizerServer.SendAsync(visIpPort, stream.ToArray(), WebSocketMessageType.Binary);
        }

        private RefereeCommand UpdateGameStatus()
        {

            RefereeCommand command = null;
            GameStatus status = gameEngine.Status;

            _refereeCommandsLock.EnterUpgradeableReadLock();
            while (refereeCommands.Count > 0)
            {
                _refereeCommandsLock.EnterWriteLock();
                command = refereeCommands.Dequeue();
                status = GameStatusCalculator.CalculateGameStatus(gameEngine.Status, command,
                                                                  GameConfig.Default.OurMarkerIsYellow);
                _refereeCommandsLock.ExitWriteLock();
            }
            _refereeCommandsLock.ExitUpgradeableReadLock();

            gameEngine.Status = status;

            if (command != null)
            {
                gameEngine.RefereeCommand = command;
            }

            return command;
        }

        private void EngineManagerRun(object obj)
        {
            CancellationToken ct = (CancellationToken)obj;
            Console.WriteLine("Engine Mangaer Started!");
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (!isJoinedVisionMulticastGroup)
                        _visionClient.JoinMulticastGroup(ConnectionConfig.Default.VisionName);

                    var referee = UpdateGameStatus();
                    var vision = RecieveVisionData();

                    if (vision == null)
                        continue;

                    var model = worldGenerator.GenerateWorldModel(vision, Commands, GameConfig.Default.OurMarkerIsYellow, GameConfig.Default.IsFieldInverted);
                    if (model == null)
                        continue;

                    model.Status = gameEngine.Status;

                    if (gameEngine.RefereeCommand != null)
                    {
                        if (gameEngine.RefereeCommand.RefereePacket != null)
                        {
                            var p = gameEngine.RefereeCommand.RefereePacket;
                            model.GoalieID = GameConfig.Default.OurMarkerIsYellow ? p.Yellow.Goalkeeper : p.Blue.Goalkeeper;
                            model.OppGoalieID = GameConfig.Default.OurMarkerIsYellow ? p.Blue.Goalkeeper : p.Yellow.Goalkeeper;
                        }
                    }

                    if (visIpPort != null)
                        SendVisualizerData(referee?.RefereePacket, model);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception thrown on Engine Manager run " + ex.StackTrace);
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
            _visionClient.JoinedMulticastGroup += Vision_OnJoinedMulticastGroup;
            _visionClient.LeftMulticastGroup += Vision_OnLeftMulticastGroup;
            isJoinedVisionMulticastGroup = false;

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
            Console.WriteLine("Websocket Is Listening:" + _visualizerServer.IsListening);
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
            _visionClient.LeaveMulticastGroup(ConnectionConfig.Default.VisionName);
            _visionClient.DisconnectAndStop();
            _visionClient.Error -= Vision_OnError;
            _visionClient.JoinedMulticastGroup -= Vision_OnJoinedMulticastGroup;
            _visionClient.LeftMulticastGroup -= Vision_OnLeftMulticastGroup;

            Console.WriteLine("Stopping Websocket...");
            if (_visualizerServer.IsListening)
                _visualizerServer.Stop();
            _visualizerServer.ClientConnected -= Visualizer_OnClientConnected;
            _visualizerServer.ClientDisconnected -= Visualizer_OnClientDisconnected;
            _visualizerServer.MessageReceived -= Visualizer_OnMessageReceived;
            _visualizerServer.Dispose();

            visIpPort = null;
            isJoinedVisionMulticastGroup = false;
        }

        private void Vision_OnJoinedMulticastGroup(object sender, IPAddress e)
        {
            isJoinedVisionMulticastGroup = true;
            Console.WriteLine($"Joined Vision Multicast Group {e}");
        }

        private void Vision_OnLeftMulticastGroup(object sender, IPAddress e)
        {
            isJoinedVisionMulticastGroup = false;
            Console.WriteLine($"Left Vision Multicast Group {e}");
        }

        private static void Vision_OnError(object sender, System.Net.Sockets.SocketError e)
        {
            Console.WriteLine($"Echo Vision UDP server caught an error with code {e}");
        }
        private void Visualizer_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Message Received " + e.IpPort);
        }
        private void Visualizer_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            visIpPort = null;
            Console.WriteLine("Client Disconnected " + e.IpPort);
        }
        private void Visualizer_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            visIpPort = e.IpPort;

            Console.WriteLine("Client Connected " + e.IpPort);
        }


    }
}