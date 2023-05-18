using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class ByteArrayExtension
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
        public static string GetHexString(this byte[] value, bool TrimStart0 = false, bool Add0x = false)
        {
            var strPrex = Add0x ? "0x" : "";
            var hex = string.Concat(value.Select(b => b.ToString("x2")).ToArray());
            if (TrimStart0) hex = hex.TrimStart('0');
            return strPrex + hex;
        }
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
        public static string GetString(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data);
        }
    }
}