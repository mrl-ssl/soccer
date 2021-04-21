using System;
using System.Net;

namespace MRL.SSL.Common.Utils.Sockets
{
    public class UdpSentEventArgs : EventArgs
    {

        public EndPoint EndPoint { get; set; }
        public long Sent { get; set; }

    }
}