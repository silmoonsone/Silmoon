using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class ByteExtension
    {
        public static MemoryStream GetStream(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return stream;
        }
        public static BinaryReader GetBinaryReader(this byte[] bytes)
        {
            return new BinaryReader(GetStream(bytes));
        }
        public static string ByteToHexString(this byte[] value, bool TrimStart0 = false, bool Add0x = false)
        {
            var strPrex = Add0x ? "0x" : "";
            var hex = string.Concat(value.Select(b => b.ToString("x2")).ToArray());
            if (TrimStart0) hex = hex.TrimStart('0');
            return strPrex + hex;
        }
    }
}