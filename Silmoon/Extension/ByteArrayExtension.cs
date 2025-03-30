using Silmoon.Enums;
using Silmoon.Models;
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
        public static MimeInfo ToMimeInfo(this byte[] data)
        {
            if (data == null || data.Length < 12)
                return null;

            var ascii = Encoding.ASCII;

            // 🎯 特殊结构识别
            string boxType = ascii.GetString(data, 4, 4);
            if (boxType == "ftyp")
            {
                string majorBrand = ascii.GetString(data, 8, 4);
                switch (majorBrand)
                {
                    case "mp42":
                    case "isom":
                    case "avc1":
                        return new MimeInfo { Extension = "mp4", Mime = "video/mp4", CategroyType = FileCategroyType.Video };
                    case "M4A ":
                    case "M4B ":
                    case "M4P ":
                        return new MimeInfo { Extension = "m4a", Mime = "audio/mp4", CategroyType = FileCategroyType.Audio };
                    case "heic":
                    case "heix":
                        return new MimeInfo { Extension = "heic", Mime = "image/heic", CategroyType = FileCategroyType.Image };
                    case "avif":
                        return new MimeInfo { Extension = "avif", Mime = "image/avif", CategroyType = FileCategroyType.Image };
                }
            }

            // WEBP
            if (ascii.GetString(data, 0, 4) == "RIFF" && ascii.GetString(data, 8, 4) == "WEBP")
            {
                return new MimeInfo { Extension = "webp", Mime = "image/webp", CategroyType = FileCategroyType.Image };
            }

            // WAV
            if (ascii.GetString(data, 0, 4) == "RIFF" && ascii.GetString(data, 8, 4) == "WAVE")
            {
                return new MimeInfo { Extension = "wav", Mime = "audio/wav", CategroyType = FileCategroyType.Audio };
            }

            // AVI
            if (ascii.GetString(data, 0, 4) == "RIFF" && ascii.GetString(data, 8, 4) == "AVI ")
            {
                return new MimeInfo { Extension = "avi", Mime = "video/x-msvideo", CategroyType = FileCategroyType.Video };
            }

            // 🧪 标准魔数匹配
            foreach (var sig in MimeInfo._signatures)
            {
                if (data.Length >= sig.Offset + sig.Magic.Length)
                {
                    bool match = true;
                    for (int i = 0; i < sig.Magic.Length; i++)
                    {
                        if (data[sig.Offset + i] != sig.Magic[i])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        return new MimeInfo
                        {
                            Extension = sig.Extension,
                            Mime = sig.Mime,
                            CategroyType = sig.Category
                        };
                    }
                }
            }

            return null;
        }
    }
}