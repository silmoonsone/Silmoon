using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Silmoon.Extension
{
    public static class NumberExtension
    {
        static readonly CultureInfo enUS = new CultureInfo("en-US");
        static readonly CultureInfo zhCN = new CultureInfo("zh-CN");

        static NumberExtension()
        {
            enUS.NumberFormat.CurrencySymbol = "$";
            zhCN.NumberFormat.CurrencySymbol = "¥";
            enUS.NumberFormat.CurrencyNegativePattern = 2;
            zhCN.NumberFormat.CurrencyNegativePattern = 2;
        }

#if NETSTANDARD
        public static int Clamp(this int self, int min, int max) => Math.Min(max, Math.Max(self, min));
        public static double Clamp(this double self, double min, double max) => Math.Min(max, Math.Max(self, min));

        public static int Negative(this int value) => -Math.Abs(value);
        public static double Negative(this double value) => -Math.Abs(value);
        public static decimal Negative(this decimal value) => -Math.Abs(value);

        public static decimal ToDecimal(this int value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this float value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this double value) => Convert.ToDecimal(value);


        public static string ToUSD(this decimal value, int currencyDecimalDigits = 2)
        {
            enUS.NumberFormat.CurrencyDecimalDigits = currencyDecimalDigits;
            return value.ToString("C", enUS);
        }
        public static string ToRMB(this decimal value, int currencyDecimalDigits = 2)
        {
            zhCN.NumberFormat.CurrencyDecimalDigits = currencyDecimalDigits;
            return value.ToString("C", zhCN);
        }
        public static string ToUSDT(this decimal value, int currencyDecimalDigits = 6, string symbol = "₮")
        {
            // 格式化为指定小数位
            string formatted = value.ToString($"N{currencyDecimalDigits}", CultureInfo.InvariantCulture);
            // 拼接自定义货币符号
            return $"{symbol}{formatted}";
        }
        public static string GetPercentText(this decimal value, string format = "0.00") => (value * 100).ToString(format) + "%";
        public static string ConvertToChineseCurrency(this decimal number)
        {
            var format = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A").Replace("0B0A", "@");
            var simplify = Regex.Replace(format, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var result = Regex.Replace(simplify, ".", match => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空整分角拾佰仟万亿兆京垓秭穰"[match.Value[0] - '-'].ToString());
            return result;
        }


#elif NET10_0_OR_GREATER
        public static decimal ToDecimal<T>(this T value) where T : INumber<T> => Convert.ToDecimal(value);
        public static T Negative<T>(this T value) where T : ISignedNumber<T> => -T.Abs(value);


        public static string ToUSD<T>(this T value, int currencyDecimalDigits = 2) where T : INumber<T>
        {
            enUS.NumberFormat.CurrencyDecimalDigits = currencyDecimalDigits;
            return Convert.ToDecimal(value).ToString("C", enUS);
        }
        public static string ToRMB<T>(this T value, int currencyDecimalDigits = 2) where T : INumber<T>
        {
            zhCN.NumberFormat.CurrencyDecimalDigits = currencyDecimalDigits;
            return Convert.ToDecimal(value).ToString("C", zhCN);
        }
        public static string ToUSDT<T>(this T value, int currencyDecimalDigits = 6, string symbol = "₮") where T : INumber<T>
        {
            string formatted = Convert.ToDecimal(value).ToString($"N{currencyDecimalDigits}", CultureInfo.InvariantCulture);
            return $"{symbol}{formatted}";
        }
        public static string GetPercentText<T>(this T value, string format = "0.00") where T : INumber<T> => (Convert.ToDecimal(value) * 100).ToString(format) + "%";
        public static string ConvertToChineseCurrency<T>(this T number) where T : INumber<T>
        {
            var decimalNumber = Convert.ToDecimal(number);
            var format = decimalNumber.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A").Replace("0B0A", "@");
            var simplify = Regex.Replace(format, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var result = Regex.Replace(simplify, ".", match => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空整分角拾佰仟万亿兆京垓秭穰"[match.Value[0] - '-'].ToString());
            return result;
        }
#endif

        public static T ToEnum<T>(this int value) where T : Enum => (T)(object)value;
        public static bool IsPrime(int n)
        {
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            for (int i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }
        public static decimal Pow(this decimal x, int y) => MathHelperExtension.Pow(x, y);
        public static BigInteger ToBigInteger(this decimal amount, int decimals) => (BigInteger)(amount * (decimal)Math.Pow(10.0, decimals));

    }
}
