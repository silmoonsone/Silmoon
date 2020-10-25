using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Silmoon.Drawing
{
    public class ImageHelper
    {
        public static Bitmap Resize(Bitmap ImageData, int Width, int Height)
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.DrawImage(ImageData, 0, 0, Width, Height);
            return bitmap;
        }
        public static byte[] Resize(byte[] ImageData, int Width, int Height)
        {
            using (MemoryStream stream = new MemoryStream(ImageData))
            {
                Bitmap bitmap = new Bitmap(stream);

                var result = Resize(bitmap, Width, Height);
                using (MemoryStream dStream = new MemoryStream())
                {
                    result.Save(dStream, bitmap.RawFormat);
                    dStream.Position = 0;
                    byte[] data = new byte[dStream.Length];
                    dStream.Read(data, 0, data.Length);
                    return data;
                }
            }
        }
        public static byte[] ResizeWidth(byte[] ImageData, int Width, bool Constrain = false)
        {
            using (MemoryStream stream = new MemoryStream(ImageData))
            {
                Bitmap bitmap = new Bitmap(stream);
                double persentWidth = (double)Width / bitmap.Width;
                int height = bitmap.Height;
                if (Constrain) height = (int)(bitmap.Height * persentWidth);

                var result = Resize(bitmap, Width, height);
                using (MemoryStream dStream = new MemoryStream())
                {
                    result.Save(dStream, bitmap.RawFormat);
                    dStream.Position = 0;
                    byte[] data = new byte[dStream.Length];
                    dStream.Read(data, 0, data.Length);
                    return data;
                }
            }
        }
        public static byte[] ResizeHeight(byte[] ImageData, int Height, bool Constrain = false)
        {
            using (MemoryStream stream = new MemoryStream(ImageData))
            {
                Bitmap bitmap = new Bitmap(stream);
                double persentHeight = (double)Height / bitmap.Height;
                int width = bitmap.Width;
                if (Constrain) width = (int)(bitmap.Width * persentHeight);

                var result = Resize(bitmap, width, Height);
                using (MemoryStream dStream = new MemoryStream())
                {
                    result.Save(dStream, bitmap.RawFormat);
                    dStream.Position = 0;
                    byte[] data = new byte[dStream.Length];
                    dStream.Read(data, 0, data.Length);
                    return data;
                }
            }
        }
    }
}
