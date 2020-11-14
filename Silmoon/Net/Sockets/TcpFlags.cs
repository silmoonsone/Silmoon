using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Net.Sockets
{
    [Flags]
    public enum TcpFlags : byte
    {
        crw = 1 << 7,
        ece = 1 << 6,
        urg = 1 << 5,
        ack = 1 << 4,
        psh = 1 << 3,
        rst = 1 << 2,
        syn = 1 << 1,
        fin = 1,
    }
}
