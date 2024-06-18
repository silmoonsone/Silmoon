using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Net.Transfer
{
    public enum TcpState
    {
        Unknown,
        ServerConnecting,
        ServerConnected,
        ServerDisconnected,
        ServerConnectFail,
        ListenStarted,
        ListenStoped,
    }
}
