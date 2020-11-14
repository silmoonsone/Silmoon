using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Net.Sockets
{
    [Flags]
    public enum IPFlags : ushort
    {
        res1 = 1 << 7,
        df = 1 << 6,
        mf = 1 << 5,
    }
}
