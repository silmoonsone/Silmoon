using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Net.Protocol
{
    public struct SDPacket
    {
        public uint PacketID;
        public uint ServiceID;
        public SDFlags Flags;
        public int StateID;
        public uint Length;
        public byte[] Data;
    }
}
