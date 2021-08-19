using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Silmoon.Extension
{
    public static class StreamExtension
    {
        public static byte[] ToBytes(this Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
                byte[] result = new byte[stream.Length];
                stream.Read(result, 0, result.Length);
                return result;
            }
            else
            {
                throw new NotSupportedException("流不支持搜索，无法直接转化成字节流");
            }
        }
    }
}
