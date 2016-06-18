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

        public override string ToString()
        {
            return a0.ToString("X") + a1.ToString("X") + a2.ToString("X") + a3.ToString("X") + a4.ToString("X") + a5.ToString("X");
        }

        public static bool operator ==(MacAddress c1, MacAddress c2)
        {
            if (c1.a0 == c2.a0 &&
               c1.a1 == c2.a1 &&
               c1.a2 == c2.a2 &&
               c1.a3 == c2.a3 &&
               c1.a4 == c2.a4 &&
               c1.a5 == c2.a5)
                return true;
            else return false;
        }
        public static bool operator !=(MacAddress c1, MacAddress c2)
        {
            if (c1.a0 == c2.a0 &&
               c1.a1 == c2.a1 &&
               c1.a2 == c2.a2 &&
               c1.a3 == c2.a3 &&
               c1.a4 == c2.a4 &&
               c1.a5 == c2.a5)
                return false;
            else return true;
        }

        public static MacAddress BroadcastAddress
        {
            get
            {
                MacAddress mac;
                mac.a0 = 0xff;
                mac.a1 = 0xff;
                mac.a2 = 0xff;
                mac.a3 = 0xff;
                mac.a4 = 0xff;
                mac.a5 = 0xff;
                return mac;
            }
        }
    }
}
