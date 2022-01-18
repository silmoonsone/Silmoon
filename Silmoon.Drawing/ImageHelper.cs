using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Silmoon.Drawing
{
    public class ImageHelper
    {
        public static byte[] Resize(byte[] ImageData, int Width, int Height)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                var result = Resize(bitmap, Width, Height);
                return result.GetBytes(format);
            }
        }
        public static Bitmap Resize(Bitmap BitmapData, int Width, int Height)
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.DrawImage(BitmapData, 0, 0, Width, Height);
            return bitmap;
        }

        public static byte[] ResizeWidth(byte[] ImageData, int Width, bool LessSizeNoProcess, bool Constrain = false)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                var result = ResizeWidth(bitmap, Width, LessSizeNoProcess, Constrain);
                return result.GetBytes(format);
            }
        }
        public static Bitmap ResizeWidth(Bitmap BitmapData, int Width, bool LessSizeNoProcess, bool Constrain = false)
        {
            if (LessSizeNoProcess && BitmapData.Width < Width) return BitmapData;
            double persentWidth = (double)Width / BitmapData.Width;
            int height = BitmapData.Height;
            if (Constrain) height = (int)(BitmapData.Height * persentWidth);

            var result = Resize(BitmapData, Width, height);
            return result;
        }
        public static byte[] ResizeHeight(byte[] ImageData, int Height, bool LessSizeNoProcess, bool Constrain = false)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                var result = ResizeHeight(bitmap, Height, LessSizeNoProcess, Constrain);
                return result.GetBytes(format);
            }
        }
        public static Bitmap ResizeHeight(Bitmap BitmapData, int Height, bool LessSizeNoProcess, bool Constrain = false)
        {
            if (LessSizeNoProcess && BitmapData.Height < Height) return BitmapData;

            double persentHeight = (double)Height / BitmapData.Height;
            int width = BitmapData.Width;
            if (Constrain) width = (int)(BitmapData.Width * persentHeight);

            var result = Resize(BitmapData, width, Height);
            return result;
        }

        public static byte[] Compress(byte[] ImageData, CompositingQuality quality = CompositingQuality.Default)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                var result = Compress(bitmap, quality);
                return result.GetBytes(format);
            }
        }
        public static Bitmap Compress(Bitmap BitmapData, CompositingQuality quality = CompositingQuality.Default)
        {
            Bitmap img = new Bitmap(BitmapData.Width, BitmapData.Height);
            Graphics g = Graphics.FromImage(img);
            g.CompositingQuality = quality;
            g.DrawImage(BitmapData, 0, 0, img.Width, img.Height);
            return img;
        }


        public static Bitmap FixiPhoneOrientation(Bitmap BitmapData)
        {
            if (BitmapData.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = BitmapData.GetPropertyItem(0x0112)?.Value[0] ?? 1;
                switch (rotationValue)
                {
                    case 1: // landscape, do nothing
                        break;

                    case 8: // rotated 90 right
                            // de-rotate:
                        BitmapData.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottoms up
                        BitmapData.RotateFlip(rotateFlipType: RotateFlipType.Rotate180FlipNone);
                        break;

                    case 6: // rotated 90 left
                        BitmapData.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }
            return BitmapData;
        }
        public static byte[] FixiPhoneOrientation(byte[] ImageData)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                if (bitmap.PropertyIdList.Contains(0x0112))
                {
                    var rotationValue = bitmap.GetPropertyItem(0x0112)?.Value[0] ?? 1;
                    if (rotationValue != 1)
                    {
                        var result = FixiPhoneOrientation(bitmap);
                        return result.GetBytes(format);
                    }
                }
                return ImageData;
            }
        }
    }
}
