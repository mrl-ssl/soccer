using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using MRL.SSL.Common.Utils.Sockets;
using MRL.SSL.Common.Configuration;
using System.IO;
using System.Net;
using ProtoBuf;
using MRL.SSL.Common.SSLWrapperCommunication;
using MRL.SSL.Common.Math;
using ProtoBuf.Meta;
using System.Collections.Generic;

namespace MRL.SSL.Ai.Engine
{
    public class EngineManager : IDisposable
    {
        Thread _cmcThread;
        UdpClient _visionClient;
        CancellationTokenSource _cmcCancelationSource = new CancellationTokenSource();
        public EngineManager()
        {
            _cmcThread = new Thread(new ParameterizedThreadStart(EngineManagerRun));

        }
        public SSLWrapperPacket RecieveVisionData()
        {
            //
            try
            {
                if (_visionClient == null)
                    return null;

                long size = _visionClient.Receive();
                if (size == 0)
                    return null;
                using var stream = new MemoryStream(_visionClient.ReceiveBuffer.Data, 0, (int)size);

                SSLWrapperPacket packet = Serializer.Deserialize<SSLWrapperPacket>(stream);
                return packet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown on receiving vision data", ex.StackTrace);
                return null;
            }
        }
        private void EngineManagerRun(object obj)
        {
            CancellationToken ct = (CancellationToken)obj;
            Console.WriteLine("Engine Mangaer Started!");
            //   RuntimeTypeModel.Default.Add(typeof(Vec<float>), true).AddSubType(101, typeof(VecF));
            while (!ct.IsCancellationRequested)
            {
                var packet = RecieveVisionData();
                if (packet != null)
                {
                }
            }
            Console.WriteLine("Engine Mangaer Stopped!");
        }
        private static void Vision_OnError(object sender, System.Net.Sockets.SocketError e)
        {
            Console.WriteLine($"Echo Vision UDP server caught an error with code {e}");
        }
        public void Initialize()
        {
            Console.WriteLine("Initializing Stuff...");
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
            Console.WriteLine("Starting CMC Thread...");
            _cmcThread.Start(_cmcCancelationSource.Token);
        }
        public void Dispose()
        {
            Console.WriteLine("Stopping CMC Thread...");
            _cmcCancelationSource.Cancel();
            _cmcThread.Join();
            _cmcCancelationSource.Dispose();


            _visionClient.Error -= Vision_OnError;

        }
    }
}