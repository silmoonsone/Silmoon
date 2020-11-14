using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Silmoon.Net.Sockets
{
    [StructLayout(LayoutKind.Explicit)]
    public struct UdpHdr
    {
        [FieldOffset(0)]
        public ushort source;
        [FieldOffset(2)]
        public ushort dest;
        [FieldOffset(4)]
        public ushort length;
        [FieldOffset(6)]
        public ushort check;
    }
}
