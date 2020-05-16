using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Net.Sockets
{
    public enum TcpEventType
    {
        ListenStarted,
        ListenStoped,
        ServerConnecting,
        ServerConnected,
        ServerConnectFailed,
        ServerDisconnected,
        ClientConnected,
        ClientDisconnected,
        ReceivedData,
    }
}
