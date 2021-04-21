using System;
using System.Net;

namespace MRL.SSL.Common.Utils.Sockets
{
    public class UdpReceivedEventArgs : EventArgs
    {
        public EndPoint EndPoint { get; set; }
        public byte[] Buffer { get; set; }
        public long Offset { get; set; }
        public long Size { get; set; }
    }
}