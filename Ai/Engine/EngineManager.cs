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
using System.Diagnostics;
using MRL.SSL.Common.Drawings;

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

        Stopwatch sw = new();
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
        public void EnqueueRefereePacket(char c, RefereeSourceType src)
        {
            SSLRefereePacket.CommandType command;
            if (GameStatusCalculator.CharToRefereeCommand(c, out command))
            {
                var r = new SSLRefereePacket();
                r.Command = command;
                r.Yellow = new SSLRefereePacket.TeamInfo();
                r.Blue = new SSLRefereePacket.TeamInfo();

                _refereeCommandsLock.EnterWriteLock();
                refereeCommands.Enqueue(new RefereeCommand(r, src));
                _refereeCommandsLock.ExitWriteLock();
            }
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

        private void SendVisualizerData(IList<RefereeCommand> refs, WorldModel model)
        {
            if (visIpPort != null)
            {
                using var stream = new MemoryStream();

                Serializer.SerializeWithLengthPrefix<WorldModel>(stream, model, PrefixStyle.Base128, 1);
                if (GameParameters.IsUpdated)
                {
                    Serializer.SerializeWithLengthPrefix<FieldConfig>(stream, GameParameters.Field, PrefixStyle.Base128, 2);
                    GameParameters.IsUpdated = false;
                }
                if (refs != null && refs.Count > 0)
                    foreach (var item in refs)
                        Serializer.SerializeWithLengthPrefix<RefereeCommand>(stream, item, PrefixStyle.Base128, 3);

                DrawingPacket.Serialize(stream, PrefixStyle.Base128, 4);

                _visualizerServer.SendAsync(visIpPort, stream.ToArray(), WebSocketMessageType.Binary);
            }
            else if (!GameParameters.IsUpdated) GameParameters.ReUpdate = true;
        }

        private IList<RefereeCommand> UpdateGameStatus()
        {
            var res = new List<RefereeCommand>();
            RefereeCommand command = null;
            GameStatus status = gameEngine.Status;

            _refereeCommandsLock.EnterUpgradeableReadLock();
            while (refereeCommands.Count > 0)
            {
                _refereeCommandsLock.EnterWriteLock();
                command = refereeCommands.Dequeue();
                status = GameStatusCalculator.CalculateGameStatus(gameEngine.Status, command,
                                                                  GameConfig.Default.OurMarkerIsYellow);
                res.Add(command);
                _refereeCommandsLock.ExitWriteLock();
            }
            _refereeCommandsLock.ExitUpgradeableReadLock();

            gameEngine.Status = status;

            if (command != null)
            {
                gameEngine.RefereeCommand = command;
            }

            return res;
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

                    var vision = RecieveVisionData();
                    if (vision == null)
                        continue;
                    // var start = sw.ElapsedMilliseconds;
                    var model = worldGenerator.GenerateWorldModel(vision, Commands, GameConfig.Default.OurMarkerIsYellow, GameConfig.Default.IsFieldInverted);
                    if (model == null)
                        continue;

                    var refs = UpdateGameStatus();

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

                    DrawingPacket.AddObject(model.Ball.Location, 0.05f, System.Drawing.Color.Red, 0.01f, true);
                    DrawingPacket.AddObject("Fucking ball", model.Ball.Location, System.Drawing.Color.Blue);
                    DrawingPacket.AddObject(model.Opponents[1].Location, model.Ball.Location, System.Drawing.Color.Green);
                    DrawingPacket.AddObject(new List<Common.Math.VectorF2D>{
                        new Common.Math.VectorF2D(0,0),
                        new Common.Math.VectorF2D(0,1),
                        new Common.Math.VectorF2D(-1,0),
                        new Common.Math.VectorF2D(-1,1)
                    }, System.Drawing.Color.Black);

                    SendVisualizerData(refs, model);

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
            // sw.Start();
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
            // sw.Stop();
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