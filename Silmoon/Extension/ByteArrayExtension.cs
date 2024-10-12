using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class ByteArrayExtension
    {
        public static MemoryStream GetStream(this byte[] bytes) => new MemoryStream(bytes);
        public static BinaryReader GetBinaryReader(this byte[] bytes) => new BinaryReader(GetStream(bytes));
        /// <summary>
        /// 分组数据，将一段数据按照指定的长度进行分割，最后不足的长度将不会冲零。
        /// </summary>
        /// <param name="data">需要分组的数据</param>
        /// <param name="slen">每段数据的长度</param>
        /// <returns></returns>
        public static byte[][] GroupData(this byte[] data, int slen)
        {
            int scount = data.Length / slen;
            int remainder;
            if ((remainder = (data.Length % slen)) != 0) scount++;

            byte[][] result = new byte[scount][];

            for (int i = 0; i < scount; i++)
            {
                if (i != (scount - 1))
                {
                    result[i] = new byte[slen];
                    Array.Copy(data, i * slen, result[i], 0, slen);
                }
                else
                {
                    result[i] = new byte[remainder];
                    Array.Copy(data, i * slen, result[i], 0, remainder);
                }
            }
            return result;
        }
        public static string GetString(this byte[] data, Encoding encoding = null)
        {
            if (data is null) return null;
            if (encoding == null) encoding = Encoding.UTF8;
            return encoding.GetString(data);
        }
        public static string GetBase64String(this byte[] data) => data is null ? null : Convert.ToBase64String(data);
        public static string ToHexString(this byte[] value, bool TrimPerfixZero, bool Add0xPrefix = false)
        {
            if (value == null) return null;

            StringBuilder sb = new StringBuilder(value.Length * 2);
            foreach (byte b in value)
            {
                //if (TrimPerfixZero && sb.Length == 0 && b == 0) continue;
                sb.Append(b.ToString("x2"));
            }

            var hexStr = sb.ToString();
            if (TrimPerfixZero) hexStr = hexStr.TrimStart('0');

            if (Add0xPrefix) return "0x" + hexStr;
            else return hexStr;
        }
        public static string ToHexString(this byte[] value) => ToHexString(value, false, false);
        public static byte[] Compress(this byte[] data)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gZipStream.Write(data, 0, data.Length);
                }
                return outputStream.ToArray();
            }
        }
        public static byte[] Decompress(this byte[] compressedData)
        {
            using (var compressedStream = new MemoryStream(compressedData))
            using (var decompressedStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    gZipStream.CopyTo(decompressedStream);
                }
                return decompressedStream.ToArray();
            }
        }
    }
}