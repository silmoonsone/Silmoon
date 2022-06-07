using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class RandomExtension
    {
        public static long NextLong(this Random rnd)
        {
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            return BitConverter.ToInt64(buf, 0);
        }
        public static ulong NextULong(this Random rnd)
        {
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            return BitConverter.ToUInt64(buf, 0);
        }
    }
}
