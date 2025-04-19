using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Extension
{
    public static class StreamExtension
    {
        [Obsolete]
        public static byte[] ToBytes(this Stream stream, bool AutoSeek = true) => GetByteArray(stream, AutoSeek);
        public static byte[] GetByteArray(this Stream stream, bool AutoSeek = true)
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
        [Obsolete]
        public async static Task<byte[]> ToBytesAsync(this Stream stream, long? Length = null, bool AutoSeek = true) => await GetByteArrayAsync(stream, Length, AutoSeek);
        public async static Task<byte[]> GetByteArrayAsync(this Stream stream, long? Length = null, bool AutoSeek = true)
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
        [Obsolete]
        public static string MakeToString(this Stream stream, Encoding encoding = default)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            if (stream.CanSeek && stream.CanRead)
            {
                long postion = stream.Position;

                stream.Position = 0;

                string result = encoding.GetString(stream.GetByteArray());
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
        public static async Task<string> ReadToEndAsync(this Stream stream, Encoding encoding = default)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            using (StreamReader reader = new StreamReader(stream, encoding))
            {
                return await reader.ReadToEndAsync();
            }
        }
        [Obsolete]
        public static byte[] WhileReadAllToByteArray(this Stream stream, int totalSize) => WhileReadToByteArray(stream, totalSize);
        public static byte[] WhileReadToByteArray(this Stream stream, int totalSize)
        {
            byte[] buffer = new byte[totalSize];
            int readed = 0, readRound;
            while ((readRound = stream.Read(buffer, readed, totalSize)) != 0)
            {
                readed += readRound;
                totalSize -= readRound;
            }
            return buffer;
        }
        public static byte[] WhileReadToEndToByteArray(this Stream stream, int bufferSize = 1024 * 512)
        {
            List<byte> result = new List<byte>();
            byte[] buffer = new byte[bufferSize];
            int readRound;
            while ((readRound = stream.Read(buffer, 0, bufferSize)) != 0)
            {
                result.AddRange(new ArraySegment<byte>(buffer, 0, readRound));
            }
            return result.ToArray();
        }
    }
}
