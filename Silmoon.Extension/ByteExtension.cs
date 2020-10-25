using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Silmoon.Extension
{
    public static class ByteExtension
    {
        public static MemoryStream MakeStream(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return stream;
        }
    }
}