
using System;
using System.IO;
using System.Threading;
using MRL.SSL.Common.Configuration;
using System.Net;
using System.Text;
using MRL.SSL.Common.SSLWrapperCommunication;
using ProtoBuf;

namespace MRL.SSL.Ai.Engine
{
    public class RefereeManager : IDisposable
    {

        MRL.SSL.Common.Utils.Sockets.UdpClient _refereeClient;
        ASCIIEncoding ascii = new ASCIIEncoding();

        private Thread _listeningThread;
        CancellationTokenSource _cancelationSource = new CancellationTokenSource();
        bool isJoinedMulticastGroup;

        public RefereeManager()
        {

        }

        public void InitialConnections()
        {

            Console.WriteLine("Initializing Referee Manager...");
            if (_refereeClient != null)
                _refereeClient.Dispose();

            _refereeClient = new MRL.SSL.Common.Utils.Sockets.UdpClient(IPAddress.Any, ConnectionConfig.Default.RefPort);
            _refereeClient.Error += Client_OnError;
            _refereeClient.SetupMulticast(true);
            _refereeClient.Connect();
            _refereeClient.OptionReceiveTimeout = 2000;
            _refereeClient.JoinedMulticastGroup += Client_OnJoinedMulticastGroup;
            _refereeClient.LeftMulticastGroup += Client_OnLeftMulticastGroup;
            isJoinedMulticastGroup = false;
        }


        public void Start()
        {
            Console.WriteLine("Starting Referee Manager Thread...");
            _listeningThread = new Thread(new ParameterizedThreadStart(RefereeRun));
            _listeningThread.Start(_cancelationSource.Token);
        }
        private void RefereeRun(object obj)
        {
            CancellationToken ct = (CancellationToken)obj;
            Console.WriteLine("Referee Manager Started!");

            uint lastCounter = uint.MaxValue;
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (!isJoinedMulticastGroup)
                        _refereeClient.JoinMulticastGroup(ConnectionConfig.Default.RefName);
                    var packet = RecieveRefereeData();
                    if (packet == null || lastCounter == packet.CommandCounter)
                        continue;
                    lastCounter = packet.CommandCounter;
                    EngineManager.Manager.EnqueueRefereePacket(packet);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception thrown on referee manager run " + ex.StackTrace);
                }
            }
            Console.WriteLine("Referee Mangaer Stopped!");
        }
        public void ApplyPacketFromCommand(char c)
        {
            SSLRefereePacket p = new SSLRefereePacket();
            switch (c)
            {
                case 'F':
                    p.Command = SSLRefereePacket.CommandType.DirectFreeBlue;
                    break;
                case 'f':
                    p.Command = SSLRefereePacket.CommandType.DirectFreeYellow;
                    break;
                case 'S':
                    p.Command = SSLRefereePacket.CommandType.Stop;
                    break;
                case 's':
                    p.Command = SSLRefereePacket.CommandType.ForceStart;
                    break;
                case 'I':
                    p.Command = SSLRefereePacket.CommandType.IndirectFreeBlue;
                    break;
                case 'i':
                    p.Command = SSLRefereePacket.CommandType.IndirectFreeYellow;
                    break;
                case 'K':
                    p.Command = SSLRefereePacket.CommandType.PrepareKickoffBlue;
                    break;
                case 'k':
                    p.Command = SSLRefereePacket.CommandType.PrepareKickoffYellow;
                    break;
                case 'P':
                    p.Command = SSLRefereePacket.CommandType.PreparePenaltyBlue;
                    break;
                case 'p':
                    p.Command = SSLRefereePacket.CommandType.PreparePenaltyYellow;
                    break;
                case 'T':
                    p.Command = SSLRefereePacket.CommandType.TimeoutBlue;
                    break;
                case 't':
                    p.Command = SSLRefereePacket.CommandType.TimeoutYellow;
                    break;
                case 'c':
                case 'H':
                    p.Command = SSLRefereePacket.CommandType.Halt;
                    break;
                case ' ':
                    p.Command = SSLRefereePacket.CommandType.NormalStart;
                    break;
                default:
                    p = null;
                    break;
            }
            if (p != null) EngineManager.Manager.EnqueueRefereePacket(p);
        }

        private SSLRefereePacket RecieveRefereeData()
        {
            if (_refereeClient == null)
                return null;

            long size = _refereeClient.Receive();
            if (size == 0 || ConnectionConfig.Default.IgnoreRefbox)
                return null;

            using var stream = new MemoryStream(_refereeClient.ReceiveBuffer.Data, 0, (int)size);

            return Serializer.Deserialize<SSLRefereePacket>(stream);
        }

        private void Client_OnError(object sender, System.Net.Sockets.SocketError e)
        {
            if (e != System.Net.Sockets.SocketError.TimedOut)
                Console.WriteLine($"Echo Referee UDP server caught an error with code {e}");
        }
        private void Client_OnJoinedMulticastGroup(object sender, IPAddress e)
        {
            isJoinedMulticastGroup = true;
            Console.WriteLine($"Joined Referee Multicast Group {e}");
        }
        private void Client_OnLeftMulticastGroup(object sender, IPAddress e)
        {
            isJoinedMulticastGroup = false;
            Console.WriteLine($"Left Referee Multicast Group {e}");
        }
        public void Dispose()
        {

            Console.WriteLine("Stopping Referee Thread...");
            _cancelationSource.Cancel();
            _listeningThread.Join();
            _cancelationSource.Dispose();

            Console.WriteLine("Stopping Referee Socket...");
            _refereeClient.LeaveMulticastGroup(ConnectionConfig.Default.RefName);
            _refereeClient.DisconnectAndStop();
            _refereeClient.Error -= Client_OnError;
            _refereeClient.JoinedMulticastGroup -= Client_OnJoinedMulticastGroup;
            _refereeClient.LeftMulticastGroup -= Client_OnLeftMulticastGroup;

        }

    }
}

