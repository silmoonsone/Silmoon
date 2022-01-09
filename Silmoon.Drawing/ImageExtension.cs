using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Silmoon.Drawing
{
    public static class ImageExtension
    {
        public static byte[] GetBytes(this Bitmap bitmap, ImageFormat imageFormat)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, imageFormat);
                return stream.ToArray();
            }
        }
        public static byte[] GetBytes(this Image image, ImageFormat imageFormat)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var codeces = ImageCodecInfo.GetImageEncoders();
                var codec = codeces[0];
                foreach (var item in codeces)
                {
                    if (imageFormat.Guid == item.FormatID)
                    {
                        codec = item;
                        break;
                    }
                }
                image.Save(stream, codec, null);
                //image.Save(stream, imageFormat);
                return stream.ToArray();
            }
        }
        public static byte[] GetBytes(this Image image)
        {
            return GetBytes(image, image.RawFormat);
        }
    }
}
