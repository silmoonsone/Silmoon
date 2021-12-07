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
        public static string MakeToString(this Stream stream, Encoding encoding = default)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            if (stream.CanSeek && stream.CanRead)
            {
                long postion = stream.Position;

                stream.Position = 0;

                string result = encoding.GetString(stream.ToBytes());
                //byte[] buff = new byte[stream.Length];
                //fixed (byte* pB = buff)
                //{
                //    stream.Read(buff, 0, buff.Length);
                //    string s = new string((sbyte*)pB, 0, buff.Length, encoding);
                //    stream.Position = postion;
                //    return s;
                //}
                stream.Position = postion;
                return result;
            }
            else
            {
                throw new NotSupportedException("流不支持读取或者搜索，无法操作");
            }
        }
    }
}
