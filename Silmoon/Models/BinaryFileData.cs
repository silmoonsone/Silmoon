using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class BinaryFileData
    {
        public byte[] Content { get; set; }
        public int Length { get; set; }
        public string ContentType { get; set; }

        public BinaryFileData()
        {

        }
        public BinaryFileData(byte[] content)
        {
            Content = content;
            Length = content.Length;
            ContentType = content.GetMimeInfo()?.Mime;
        }
        public BinaryFileData(byte[] content, string contentType)
        {
            Content = content;
            Length = content.Length;
            ContentType = contentType;
        }
    }
}
