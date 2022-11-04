using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Silmoon.Extension
{
    public static class BigIntegerHelper
    {
        public static BigInteger[] ParseArray(string[] strings)
        {
            if (strings is null) return null;
            List<BigInteger> bigIntegers = new List<BigInteger>();
            foreach (var item in strings)
            {
                bigIntegers.Add(BigInteger.Parse(item));
            }
            return bigIntegers.ToArray();
        }
    }
}
