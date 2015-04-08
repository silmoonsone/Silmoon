using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Silmoon.Net.Sockets
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MacAddress
    {
        [FieldOffset(0)]
        public byte a0;

        [FieldOffset(1)]
        public byte a1;

        [FieldOffset(2)]
        public byte a2;

        [FieldOffset(3)]
        public byte a3;

        [FieldOffset(4)]
        public byte a4;

        [FieldOffset(5)]
        public byte a5;
    }
}
