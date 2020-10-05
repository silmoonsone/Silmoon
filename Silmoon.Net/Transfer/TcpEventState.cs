using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Net.Transfer
{
    public enum TcpEventState
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
