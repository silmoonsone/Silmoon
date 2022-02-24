using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Silmoon.Net
{
    public class SocketHelper
    {
        static Socket socket = null;
        public static int UdpSendTo(string s)
        {
            if (socket is null) socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            lock (socket)
            {
                var i = socket.SendTo(Encoding.UTF8.GetBytes(s), new IPEndPoint(IPAddress.Loopback, 20001));
                return i;
            }
        }
        public static int UdpSendTo(string s, IPAddress address, int port)
        {
            if (socket is null) socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            lock (socket)
            {
                var i = socket.SendTo(Encoding.UTF8.GetBytes(s), new IPEndPoint(address, port));
                return i;
            }
        }
    }
}
