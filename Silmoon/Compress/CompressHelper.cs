using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Silmoon.Extension;

namespace Silmoon.Compress
{
    public static class CompressHelper
    {
        public static byte[] Compress(byte[] Data)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gZipStream.Write(Data, 0, Data.Length);
                }
                return outputStream.ToArray();
            }
        }
        public static byte[] Decompress(byte[] compressedData)
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

        public static string CompressStringToBase64String(string Text)
        {
            var bytes = Encoding.UTF8.GetBytes(Text);
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gZipStream.Write(bytes, 0, bytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }
        public static string DecompressBase64StringToString(string Base64CompressedText)
        {
            var bytes = Convert.FromBase64String(Base64CompressedText);
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    using (var streamReader = new StreamReader(gZipStream, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        public static string CompressStringToHexString(string Text)
        {
            var bytes = Encoding.UTF8.GetBytes(Text);
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gZipStream.Write(bytes, 0, bytes.Length);
                }
                return outputStream.ToArray().ByteArrayToHexString();
            }
        }
        public static string DecompressHexStringToString(string HexStringCompressedText)
        {
            var bytes = HexStringCompressedText.HexStringToByteArray();
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    using (var streamReader = new StreamReader(gZipStream, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
