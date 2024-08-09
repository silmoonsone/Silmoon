using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class ByteArrayExtension2
    {
        public static string ToHexString(this Span<byte> value, bool TrimPerfixZero, bool Add0xPrefix = false)
        {
            if (value == null) return null;
            StringBuilder sb = new StringBuilder(value.Length * 2);
            foreach (byte b in value)
            {
                sb.Append(b.ToString("x2"));
            }
            var hexStr = sb.ToString();
            if (TrimPerfixZero) hexStr = hexStr.TrimStart('0');

            if (Add0xPrefix) return "0x" + hexStr;
            else return hexStr;
        }
        public static string ToHexString(this Span<byte> value) => ToHexString(value, false, false);
        public static string ToHexString(this ReadOnlySpan<byte> value, bool TrimPerfixZero, bool Add0xPrefix = false)
        {
            if (value == null) return null;
            StringBuilder sb = new StringBuilder(value.Length * 2);
            foreach (byte b in value)
            {
                sb.Append(b.ToString("x2"));
            }
            var hexStr = sb.ToString();
            if (TrimPerfixZero) hexStr = hexStr.TrimStart('0');

            if (Add0xPrefix) return "0x" + hexStr;
            else return hexStr;
        }
        public static string ToHexString(this ReadOnlySpan<byte> value) => ToHexString(value, false, false);

    }
}
