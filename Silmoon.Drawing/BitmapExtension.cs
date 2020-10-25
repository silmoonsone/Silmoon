using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Silmoon.Drawing
{
    public static class BitmapExtension
    {
        public static byte[] GetBytes(this Bitmap bitmap, ImageFormat imageFormat)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, imageFormat);
                return stream.ToArray();
            }
        }
    }
}
