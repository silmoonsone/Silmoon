using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Silmoon.Extension
{
    public static class BigIntegerExtension
    {
        public static BigInteger[] ToBigIntegerArray(this string[] strArray)
        {
            if (strArray is null) return null;
            List<BigInteger> bigIntegers = new List<BigInteger>();
            foreach (var item in strArray)
            {
                bigIntegers.Add(BigInteger.Parse(item));
            }
            return bigIntegers.ToArray();
        }

        public static int[] ToIntArray(this BigInteger[] bigIntegers)
        {
            int[] result = new int[bigIntegers.Length];

            for (int i = 0; i < bigIntegers.Length; i++)
            {
                if (bigIntegers[i] > int.MaxValue || bigIntegers[i] < int.MinValue)
                {
                    throw new OverflowException("Value is too large or too small for an int.");
                }

                result[i] = (int)bigIntegers[i];
            }

            return result;
        }
        public static uint[] ToUintArray(this BigInteger[] bigIntegers)
        {
            uint[] result = new uint[bigIntegers.Length];

            for (int i = 0; i < bigIntegers.Length; i++)
            {
                if (bigIntegers[i] > uint.MaxValue || bigIntegers[i] < uint.MinValue)
                {
                    throw new OverflowException("Value is too large or too small for an uint.");
                }

                result[i] = (uint)bigIntegers[i];
            }

            return result;
        }
        public static long[] ToLongArray(this BigInteger[] bigIntegers)
        {
            long[] result = new long[bigIntegers.Length];

            for (int i = 0; i < bigIntegers.Length; i++)
            {
                if (bigIntegers[i] > long.MaxValue || bigIntegers[i] < long.MinValue)
                {
                    throw new OverflowException("Value is too large or too small for a long.");
                }

                result[i] = (long)bigIntegers[i];
            }

            return result;
        }
        public static ulong[] ToUlongArray(this BigInteger[] bigIntegers)
        {
            ulong[] result = new ulong[bigIntegers.Length];

            for (int i = 0; i < bigIntegers.Length; i++)
            {
                if (bigIntegers[i] > ulong.MaxValue || bigIntegers[i] < ulong.MinValue)
                {
                    throw new OverflowException("Value is too large or too small for a ulong.");
                }

                result[i] = (ulong)bigIntegers[i];
            }

            return result;
        }
    }
}
