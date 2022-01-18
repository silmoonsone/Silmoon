using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Silmoon.Drawing
{
    public class BitmapHelper
    {
        public static Bitmap FromBytes(byte[] buffer)
        {
            var stream = buffer.GetStream();
            return new Bitmap(stream);
        }
    }
}
