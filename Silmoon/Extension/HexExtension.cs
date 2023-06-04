using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Silmoon.Extension
{
    public static class HexExtension
    {
        public static bool IsHexString(this string value)
        {
            bool isHex;
            value = value.Substring(value.StartsWith("0x") ? 2 : 0);
            foreach (var c in value)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }

        public static byte[] HexStringToByteArray(this string hex)
        {
            if (hex == null) return null;
            if (hex.StartsWith("0x")) hex = hex.Substring(2);

            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];

            if (numberChars % 2 != 0)
            {
                hex = "0" + hex;
                numberChars = hex.Length;
            }
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
        public static string ByteArrayToHexString(this byte[] value, bool TrimZeroPerfix = false, bool Add0xPrefix = false)
        {
            if (value == null) return null;

            StringBuilder sb = new StringBuilder(value.Length * 2);
            foreach (byte b in value)
            {
                sb.Append(b.ToString("x2"));
            }

            var hexStr = sb.ToString();
            if (TrimZeroPerfix) hexStr = hexStr.TrimStart('0');

            if (Add0xPrefix) return "0x" + hexStr;
            else return hexStr;
        }

        public static BigInteger HexStringToBigInteger(this string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

            if (value.StartsWith("0x")) value = value.Substring(2);

            return BigInteger.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }
        public static string BigIntegerToHexString(this BigInteger value, bool TrimZeroPrefix, bool Add0xPrefix = false)
        {
            if (value == 0) return Add0xPrefix ? "0x0" : "0";

            var hexStr = value.ToString("x");
            if (TrimZeroPrefix) hexStr = hexStr.TrimStart('0');

            if (Add0xPrefix) return "0x" + hexStr;
            else return hexStr;
        }
    }
}
