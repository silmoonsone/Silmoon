using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Silmoon.Net.Sockets
{
    public class TcpEventArgs : EventArgs
    {
        public TcpEventType EventType { get; set; }
        public IPEndPoint IPEndPoint { get; set; }
        public Socket Socket { get; set; }
        public byte[] Data { get; set; }
    }
}
