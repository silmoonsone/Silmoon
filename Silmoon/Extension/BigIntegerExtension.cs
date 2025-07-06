using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Silmoon.Extension
{
    public static class BigIntegerExtension
    {
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
        public static decimal ToDecimal(this BigInteger amount, int decimals)
        {
            decimal result = (decimal)amount / (decimal)Math.Pow(10, decimals);
            return result;
        }
        /// <summary>
        /// BigInteger → 十六进制字符串  
        /// *完全保留旧签名*：<c>TrimZeroPrefix</c>、<c>Add0xPrefix</c> 位置不变。  
        /// 追加两个可选参数而不破坏现有调用：
        /// </summary>
        /// <param name="value">要转换的数。</param>
        /// <param name="add0xPrefix">是否在结果前加 <c>"0x"</c>。</param>
        /// <param name="signed">
        /// <c>true</c>（默认） ⇒ 按 **二补码** 输出，负数会以全 `f`-前缀展示；<br/>
        /// <c>false</c> ⇒ 把整数字段视为 **无符号正数**，即便最高位是 1 也不出现负号。
        /// </param>
        /// <param name="upperCase">是否用大写 A-F。默认小写。</param>
        public static string BigIntegerToHexString(this BigInteger value, bool add0xPrefix = false, bool signed = true, bool upperCase = false)
        {
            // 0 直接返回
            if (value.IsZero) return add0xPrefix ? "0x0" : "0";

            string hex;

#if NET5_0_OR_GREATER
        // -------- .NET 5+：走 Convert.ToHexString（硬件加速） --------
        if (!signed)
        {
            // 无符号路径：直接获取大端 bytes
            byte[] bytes = value.ToByteArray(isUnsigned: true, isBigEndian: true);  // :contentReference[oaicite:0]{index=0}
            hex = Convert.ToHexString(bytes);                                       // :contentReference[oaicite:1]{index=1}
        }
        else
        {
            // 有符号路径：用默认 ToString("x")
            hex = value.ToString("x");
        }

        // Convert.ToHexString 总是大写
        if (!upperCase && signed)  // 有符号路径里 hex 已是小写
        {
            hex = hex.ToLowerInvariant();
        }
        else if (!upperCase && !signed)
        {
            hex = hex.ToLowerInvariant();
        }
        else if (upperCase && signed)
        {
            hex = hex.ToUpperInvariant();
        }
#else
            // -------- 旧框架 fallback --------
            hex = value.ToString(upperCase ? "X" : "x");

            if (!signed && value.Sign < 0)
            {
                // 把补码翻为正数串（补 2^(4·digits)）
                int bits = 4 * hex.Length;
                BigInteger unsigned = value + (BigInteger.One << bits);
                hex = unsigned.ToString(upperCase ? "X" : "x");
            }
#endif


            return add0xPrefix ? "0x" + hex : hex;
        }
    }
}
