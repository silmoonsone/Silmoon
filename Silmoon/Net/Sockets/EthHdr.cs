using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Silmoon.Net.Sockets
{
    [StructLayout(LayoutKind.Explicit)]
    public struct EthHdr
    {
        [FieldOffset(0)]
        public MacAddress h_dest;
        [FieldOffset(6)]
        public MacAddress h_source;
        [FieldOffset(12)]
        public short h_proto;
    }
}
