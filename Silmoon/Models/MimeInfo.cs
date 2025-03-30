using Silmoon.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class MimeInfo
    {
        internal static readonly List<(byte[] Magic, int Offset, string Extension, string Mime, FileCategroyType Category)> _signatures = new List<(byte[], int, string, string, FileCategroyType)>
        {
            // 📷 Images
            (new byte[] { 0xFF, 0xD8, 0xFF }, 0, "jpg", "image/jpeg", FileCategroyType.Image),
            (new byte[] { 0x89, 0x50, 0x4E, 0x47 }, 0, "png", "image/png", FileCategroyType.Image),
            (new byte[] { 0x47, 0x49, 0x46, 0x38 }, 0, "gif", "image/gif", FileCategroyType.Image),
            (new byte[] { 0x42, 0x4D }, 0, "bmp", "image/bmp", FileCategroyType.Image),
            (new byte[] { 0x49, 0x49, 0x2A, 0x00 }, 0, "tif", "image/tiff", FileCategroyType.Image),
            (new byte[] { 0x4D, 0x4D, 0x00, 0x2A }, 0, "tif", "image/tiff", FileCategroyType.Image),
            (new byte[] { 0x00, 0x00, 0x01, 0x00 }, 0, "ico", "image/x-icon", FileCategroyType.Image),

            // 🔊 Audio
            (new byte[] { 0x49, 0x44, 0x33 }, 0, "mp3", "audio/mpeg", FileCategroyType.Audio),
            (new byte[] { 0xFF, 0xFB }, 0, "mp3", "audio/mpeg", FileCategroyType.Audio),
            (new byte[] { 0x66, 0x4C, 0x61, 0x43 }, 0, "flac", "audio/flac", FileCategroyType.Audio),
            (new byte[] { 0x4F, 0x67, 0x67, 0x53 }, 0, "ogg", "audio/ogg", FileCategroyType.Audio),

            // 🎥 Video
            (new byte[] { 0x1A, 0x45, 0xDF, 0xA3 }, 0, "mkv", "video/x-matroska", FileCategroyType.Video),

            // 📄 Documents
            (new byte[] { 0x25, 0x50, 0x44, 0x46 }, 0, "pdf", "application/pdf", FileCategroyType.Document),
            (new byte[] { 0xD0, 0xCF, 0x11, 0xE0 }, 0, "doc", "application/vnd.ms-office", FileCategroyType.Document),
            (new byte[] { 0x50, 0x4B, 0x03, 0x04 }, 0, "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", FileCategroyType.Document), // or zip

            // 📦 Archives
            (new byte[] { 0x50, 0x4B, 0x03, 0x04 }, 0, "zip", "application/zip", FileCategroyType.Archive),
            (new byte[] { 0x1F, 0x8B, 0x08 }, 0, "gz", "application/gzip", FileCategroyType.Archive),
            (new byte[] { 0x52, 0x61, 0x72, 0x21 }, 0, "rar", "application/x-rar-compressed", FileCategroyType.Archive),
            (new byte[] { 0x37, 0x7A, 0xBC, 0xAF }, 0, "7z", "application/x-7z-compressed", FileCategroyType.Archive),

            // ⚙️ Executables
            (new byte[] { 0x4D, 0x5A }, 0, "exe", "application/vnd.microsoft.portable-executable", FileCategroyType.Executable),
        };
        public string Mime { get; set; }
        public string Extension { get; set; }
        public FileCategroyType CategroyType { get; set; }

    }
}
