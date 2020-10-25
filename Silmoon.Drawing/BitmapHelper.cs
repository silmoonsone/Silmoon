using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Silmoon.Drawing
{
    public class BitmapHelper
    {
        public static Bitmap FromBytes(byte[] buffer)
        {
            using (var stream = buffer.MakeStream())
            {
                return new Bitmap(stream);
            }
        }
    }
}
