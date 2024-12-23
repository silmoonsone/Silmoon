﻿using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Silmoon.Extension;
using System.Threading.Tasks;

namespace Silmoon.Compress
{
    public static class CompressHelper
    {
        public static byte[] Compress(byte[] data) => data.Compress();
        public static byte[] Decompress(byte[] compressedData) => compressedData.Decompress();

        public static string CompressStringToBase64String(string text, Encoding encoding = null)
        {
            if (encoding is null) encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes(text);
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gZipStream.Write(bytes, 0, bytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }
        public static string DecompressBase64StringToString(string base64CompressedText, Encoding encoding = null)
        {
            var bytes = Convert.FromBase64String(base64CompressedText);
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    if (encoding is null) encoding = Encoding.UTF8;
                    using (var streamReader = new StreamReader(gZipStream, encoding))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        public static string CompressStringToHexString(string text, Encoding encoding = null)
        {
            if (encoding is null) encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes(text);
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gZipStream.Write(bytes, 0, bytes.Length);
                }
                return outputStream.ToArray().ToHexString();
            }
        }
        public static string DecompressHexStringToString(string hexStringCompressedText, Encoding encoding = null)
        {
            var bytes = hexStringCompressedText.HexStringToByteArray();
            using (var inputStream = new MemoryStream(bytes.Data))
            {
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    if (encoding is null) encoding = Encoding.UTF8;
                    using (var streamReader = new StreamReader(gZipStream, encoding))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        public static byte[] CompressStringToByteArray(string text, Encoding encoding = null)
        {
            if (encoding is null) encoding = Encoding.UTF8;
            var data = encoding.GetBytes(text);
            return data.Compress();
        }
        public static string DecompressByteArrayToString(byte[] compressedBytes, Encoding encoding = null)
        {
            var decompressedBytes = compressedBytes.Decompress();
            if (encoding is null) encoding = Encoding.UTF8;
            return encoding.GetString(decompressedBytes);
        }

        public static void ExtractGZipFile(string zipPath, string extractPath)
        {
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
        public static async Task ExtractZipFileAsync(string zipPath, string extractPath, Action<string, double, double> progressCallback)
        {
            using (var zipArchive = ZipFile.OpenRead(zipPath))
            {
                double totalEntries = zipArchive.Entries.Count;
                double currentEntryIndex = 0;

                foreach (var entry in zipArchive.Entries)
                {
                    string destinationPath = Path.Combine(extractPath, entry.FullName);

                    // 确保目标文件夹已创建
                    string directoryPath = Path.GetDirectoryName(destinationPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // 如果条目是文件夹本身，则创建文件夹并跳过文件解压
                    if (string.IsNullOrEmpty(entry.Name)) // 表示这是一个文件夹
                    {
                        Directory.CreateDirectory(destinationPath);
                        continue;
                    }

                    // 报告当前文件名和整体解压进度
                    progressCallback?.Invoke(entry.FullName, 0, currentEntryIndex / totalEntries);

                    // 异步解压单个文件
                    using (var inputStream = entry.Open())
                    using (var outputStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        byte[] buffer = new byte[4096];
                        int readBytes;
                        long totalRead = 0;

                        while ((readBytes = await inputStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await outputStream.WriteAsync(buffer, 0, readBytes);
                            totalRead += readBytes;

                            // 报告当前文件的解压进度
                            double fileProgress = totalRead / (double)entry.Length;
                            progressCallback?.Invoke(entry.FullName, fileProgress, currentEntryIndex / totalEntries);
                        }
                    }

                    currentEntryIndex++;
                    // 报告完成当前文件的解压
                    progressCallback?.Invoke(entry.FullName, 1, currentEntryIndex / totalEntries);
                }
            }
        }
    }
}
