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

        public MacAddress(byte a0, byte a1, byte a2, byte a3, byte a4, byte a5)
        {
            this.a0 = a0;
            this.a1 = a1;
            this.a2 = a2;
            this.a3 = a3;
            this.a4 = a4;
            this.a5 = a5;
        }

        public override string ToString()
        {
            return a0.ToString("X") + a1.ToString("X") + a2.ToString("X") + a3.ToString("X") + a4.ToString("X") + a5.ToString("X");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MacAddress))
            {
                return false;
            }

            var address = (MacAddress)obj;
            return a0 == address.a0 &&
                   a1 == address.a1 &&
                   a2 == address.a2 &&
                   a3 == address.a3 &&
                   a4 == address.a4 &&
                   a5 == address.a5;
        }
        public override int GetHashCode()
        {
            var hashCode = 405784741;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + a0.GetHashCode();
            hashCode = hashCode * -1521134295 + a1.GetHashCode();
            hashCode = hashCode * -1521134295 + a2.GetHashCode();
            hashCode = hashCode * -1521134295 + a3.GetHashCode();
            hashCode = hashCode * -1521134295 + a4.GetHashCode();
            hashCode = hashCode * -1521134295 + a5.GetHashCode();
            return hashCode;
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
