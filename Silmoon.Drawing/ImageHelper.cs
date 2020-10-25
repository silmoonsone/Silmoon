using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            Bitmap bitmap = BitmapHelper.FromBytes(ImageData);
            var result = Resize(bitmap, Width, Height);
            return result.GetBytes(bitmap.RawFormat);
        }
        public static byte[] ResizeWidth(byte[] ImageData, int Width, bool Constrain = false)
        {
            Bitmap bitmap = BitmapHelper.FromBytes(ImageData);

            double persentWidth = (double)Width / bitmap.Width;
            int height = bitmap.Height;
            if (Constrain) height = (int)(bitmap.Height * persentWidth);

            var result = Resize(bitmap, Width, height);
            return result.GetBytes(bitmap.RawFormat);
        }
        public static byte[] ResizeHeight(byte[] ImageData, int Height, bool Constrain = false)
        {
            Bitmap bitmap = BitmapHelper.FromBytes(ImageData);

            double persentHeight = (double)Height / bitmap.Height;
            int width = bitmap.Width;
            if (Constrain) width = (int)(bitmap.Width * persentHeight);

            var result = Resize(bitmap, width, Height);
            return result.GetBytes(bitmap.RawFormat);
        }
        public static byte[] Compress(byte[] ImageData, CompositingQuality quality = CompositingQuality.Default)
        {
            Bitmap bitmap = BitmapHelper.FromBytes(ImageData);
            var result = Compress(bitmap, quality);
            return result.GetBytes(bitmap.RawFormat);
        }
        public static Bitmap Compress(Bitmap bitmap, CompositingQuality quality = CompositingQuality.Default)
        {
            Bitmap img = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics g = Graphics.FromImage(img);
            g.CompositingQuality = quality;
            g.DrawImage(bitmap, 0, 0, img.Width, img.Height);
            return img;
        }
    }
}
