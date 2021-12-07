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
            if (stream.CanSeek && stream.CanRead)
            {
                long postion = stream.Position;
                stream.Position = 0;
                byte[] result = new byte[stream.Length];
                stream.Read(result, 0, result.Length);
                stream.Position = postion;
                return result;
            }
            else
            {
                throw new NotSupportedException("流不支持读取或者搜索，无法操作");
            }
        }
        public static string Read(this Stream stream, Encoding encoding = default)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            if (stream.CanSeek && stream.CanRead)
            {
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                throw new NotSupportedException("流不支持读取或者搜索，无法操作");
            }
        }
    }
}
