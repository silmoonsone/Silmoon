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
        public static Bitmap Resize(Bitmap BitmapData, int Width, int Height)
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(BitmapData, 0, 0, Width, Height);
            }
            return bitmap;
        }
        public static byte[] Resize(byte[] ImageData, int Width, int Height)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                using (var result = Resize(bitmap, Width, Height))
                {
                    return result.GetBytes(format);
                }
            }
        }

        public static Bitmap ResizeWidth(Bitmap BitmapData, int Width, bool IfLessKeepSize, bool Constrain = false)
        {
            if (IfLessKeepSize && BitmapData.Width < Width) Width = BitmapData.Width;
            double persentWidth = (double)Width / BitmapData.Width;
            int height = BitmapData.Height;
            if (Constrain) height = (int)(BitmapData.Height * persentWidth);

            var result = Resize(BitmapData, Width, height);
            return result;
        }
        public static byte[] ResizeWidth(byte[] ImageData, int Width, bool IfLessKeepSize, bool Constrain = false)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                using (var result = ResizeWidth(bitmap, Width, IfLessKeepSize, Constrain))
                {
                    return result.GetBytes(format);
                }
            }
        }
        public static Bitmap ResizeHeight(Bitmap BitmapData, int Height, bool IfLessKeepSize, bool Constrain = false)
        {
            if (IfLessKeepSize && BitmapData.Height < Height) Height = BitmapData.Height;

            double persentHeight = (double)Height / BitmapData.Height;
            int width = BitmapData.Width;
            if (Constrain) width = (int)(BitmapData.Width * persentHeight);

            var result = Resize(BitmapData, width, Height);
            return result;
        }
        public static byte[] ResizeHeight(byte[] ImageData, int Height, bool IfLessKeepSize, bool Constrain = false)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                using (var result = ResizeHeight(bitmap, Height, IfLessKeepSize, Constrain))
                {
                    return result.GetBytes(format);
                }
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
        public static byte[] Compress(byte[] ImageData, CompositingQuality quality = CompositingQuality.Default)
        {
            using (Bitmap bitmap = new Bitmap(ImageData.GetStream()))
            {
                var format = bitmap.RawFormat;
                using (var result = Compress(bitmap, quality))
                {
                    return result.GetBytes(format);
                }
            }
        }


        public static void FixiPhoneOrientation(Bitmap BitmapData)
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
                        FixiPhoneOrientation(bitmap);
                        return bitmap.GetBytes(format);
                    }
                }
                return ImageData;
            }
        }

        public static byte[] AdjustImageSizeWithWidth(byte[] imageData, int width, CompositingQuality quality = CompositingQuality.HighSpeed)
        {
            using (var image = new Bitmap(imageData.GetStream()))
            {
                var imageFormat = image.RawFormat;
                FixiPhoneOrientation(image);
                using (var image2 = ResizeWidth(image, width, true, true))
                {
                    using (var image3 = Compress(image2, quality))
                    {
                        var data = image3.GetBytes(imageFormat);
                        return data;
                    }
                }
            }
        }
    }
}
