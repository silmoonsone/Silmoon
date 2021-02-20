using System;
using System.Collections.Generic;
using System.IO;
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
    }
}