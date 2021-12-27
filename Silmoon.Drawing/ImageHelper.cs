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


        public static Image FixiPhoneOrientation(Image ImageData)
        {
            if (ImageData.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = ImageData.GetPropertyItem(0x0112)?.Value[0] ?? 1;
                switch (rotationValue)
                {
                    case 1: // landscape, do nothing
                        break;

                    case 8: // rotated 90 right
                            // de-rotate:
                        ImageData.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottoms up
                        ImageData.RotateFlip(rotateFlipType: RotateFlipType.Rotate180FlipNone);
                        break;

                    case 6: // rotated 90 left
                        ImageData.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }
            return ImageData;
        }
        public static byte[] FixiPhoneOrientation(byte[] ImageData)
        {
            using (Image image = Image.FromStream(ImageData.GetStream()))
            {
                var result = FixiPhoneOrientation(image);
                return result.GetBytes(image.RawFormat);
            }
        }
    }
}
