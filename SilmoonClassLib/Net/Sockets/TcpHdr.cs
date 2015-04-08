using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Silmoon.Net.Sockets
{
    [StructLayout(LayoutKind.Explicit)]
    public struct TcpHdr
    {
        [FieldOffset(0)]
        public ushort source;
        [FieldOffset(2)]
        public ushort dest;
        [FieldOffset(4)]
        public uint seq;
        [FieldOffset(8)]
        public uint ack_seq;
        /// <summary>
        /// len up to 2bit
        /// </summary>
        [FieldOffset(12)]
        public byte doff;
        [FieldOffset(13)]
        public TcpFlags flag;
        [FieldOffset(14)]
        public ushort window;
        [FieldOffset(16)]
        public ushort check;
    }
}
