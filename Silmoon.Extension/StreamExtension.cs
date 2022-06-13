﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Extension
{
    public static class StreamExtension
    {
        public static byte[] ToBytes(this Stream stream, bool AutoSeek = true)
        {
            if (AutoSeek && !stream.CanSeek) throw new NotSupportedException("流不支持读取或者搜索，无法操作");

            bool seek = stream.CanSeek && AutoSeek;

            long postion = 0;
            if (seek) postion = stream.Position;
            if (seek) stream.Position = 0;
            byte[] result = new byte[stream.Length];
            stream.Read(result, 0, result.Length);
            if (seek) stream.Position = postion;
            return result;
        }
        public async static Task<byte[]> ToBytesAsync(this Stream stream, long? Length = null, bool AutoSeek = true)
        {
            if (AutoSeek && !stream.CanSeek) throw new NotSupportedException("流不支持读取或者搜索，无法操作");

            bool seek = stream.CanSeek && AutoSeek;

            long postion = 0;
            if (seek) postion = stream.Position;
            if (seek) stream.Position = 0;
            if (Length is null) Length = stream.Length;
            byte[] result = new byte[Length.Value];
            await stream.ReadAsync(result, 0, result.Length).ConfigureAwait(false);
            if (seek) stream.Position = postion;
            return result;

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
