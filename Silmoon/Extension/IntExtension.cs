using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Silmoon.Extension
{
    public static class IntExtension
    {
        static readonly CultureInfo enUS = new CultureInfo("en-US");
        static readonly CultureInfo zhCN = new CultureInfo("zh-CN");

        public static T ToEnum<T>(this int value) where T : Enum
        {
            return (T)(object)value;
        }
        public static string ToUSD(this int value)
        {
            enUS.NumberFormat.CurrencyNegativePattern = 2;
            return value.ToString("C", enUS);
        }
        public static string ToRMB(this int value)
        {
            zhCN.NumberFormat.CurrencyNegativePattern = 2;
            return value.ToString("C", zhCN);
        }
        public static bool IsPrime(int n)
        {
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            for (int i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }
    }
}
