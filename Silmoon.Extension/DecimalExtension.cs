using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Silmoon.Extension
{
    public static class DecimalExtension
    {
        static readonly CultureInfo enUS = new CultureInfo("en-US");
        static readonly CultureInfo zhCN = new CultureInfo("zh-CN");
        public static string ToUSD(this decimal value)
        {
            enUS.NumberFormat.CurrencyNegativePattern = 2;
            return value.ToString("C", enUS);
        }
        public static string ToRMB(this decimal value)
        {
            zhCN.NumberFormat.CurrencyNegativePattern = 2;
            return value.ToString("C", zhCN);
        }
        public static decimal Pow(this decimal x, int y)
        {
            decimal result = 1;
            for (int i = 0; i < y; i++)
            {
                result *= x;
            }
            return result;
        }
    }
}
