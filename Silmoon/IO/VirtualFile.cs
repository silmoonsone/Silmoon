using Silmoon.Extension;
using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.IO
{
    public class VirtualFile
    {
        public byte[] Content { get; set; }
        public int Length { get; set; }
        public string ContentType { get; set; }

        public VirtualFile()
        {

        }
        public VirtualFile(byte[] content)
        {
            Content = content;
            Length = content.Length;
            ContentType = content.GetMimeInfo()?.Mime;
        }
        public VirtualFile(byte[] content, string contentType)
        {
            Content = content;
            Length = content.Length;
            ContentType = contentType;
        }
    }
}
