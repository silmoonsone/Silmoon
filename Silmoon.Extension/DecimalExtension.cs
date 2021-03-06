using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Silmoon.Extension
{
    public static class DecimalExtension
    {
        public static string ToUSD(this decimal value)
        {
            return value.ToString("C", new CultureInfo("en-US"));
        }
        public static string ToRMB(this decimal value)
        {
            return value.ToString("C", new CultureInfo("zh-CN"));
        }
    }
}
