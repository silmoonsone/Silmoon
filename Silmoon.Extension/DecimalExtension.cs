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
            var c = new CultureInfo("en-US");
            c.NumberFormat.CurrencyNegativePattern = 2;
            return value.ToString("C", c);
        }
        public static string ToRMB(this decimal value)
        {
            var c = new CultureInfo("zh-CN");
            c.NumberFormat.CurrencyNegativePattern = 2;
            return value.ToString("C", c);
        }
    }
}
