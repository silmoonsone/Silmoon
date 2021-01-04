using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Silmoon.Extension
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static int GetByteLenght(this string value, Encoding encoding)
        {
            return encoding.GetByteCount(value);
        }

        public static string SubStringEncoded(this string s, int length, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(s);
            int n = 0;  //  表示当前的字节数
            int i = 0;  //  要截取的字节数
            for (; i < bytes.GetLength(0) && n < length; i++)
            {
                //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                if (i % 2 == 0)
                {
                    n++;      //  在UCS2第一个字节时n加1
                }
                else
                {
                    //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        n++;
                    }
                }
            }
            //  如果i为奇数时，处理成偶数
            if (i % 2 == 1)
            {
                //  该UCS2字符是汉字时，去掉这个截一半的汉字
                if (bytes[i] > 0)
                    i = i - 1;
                //  该UCS2字符是字母或数字，则保留该字符
                else

                    i = i + 1;
            }
            return encoding.GetString(bytes, 0, i);
        }
        public static T ToEnum<T>(this string value, bool ignoreCase = false) where T : Enum
        {
            var type = typeof(T);
            var result = (T)Enum.Parse(type, value, ignoreCase);
            return result;
        }
        public static T ToEnum<T>(this string value, bool throwException, bool ignoreCase = false) where T : Enum
        {
            try
            {
                var type = typeof(T);
                var result = (T)Enum.Parse(type, value, ignoreCase);
                return result;
            }
            catch
            {
                return default;
            }
        }

        public static string ToBase64String(this string value, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var data = Convert.ToBase64String(Encoding.Default.GetBytes(value));
            return data;
        }
        public static string TrimWithoutNull(this string value)
        {
            if (value == null) return null;
            else return value.Trim();
        }
        public static bool ToBool(this string value, bool defaultResult = false, bool stringNullOrEmptyResult = false)
        {
            if (string.IsNullOrEmpty(value)) return stringNullOrEmptyResult;
            switch (value.ToLower())
            {
                case null:
                    return false;
                case "":
                    return false;
                case "1":
                    return true;
                case "0":
                    return false;
                case "y":
                    return true;
                case "n":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                case "ok":
                    return true;
                case "not":
                    return true;
                case "on":
                    return true;
                case "off":
                    return false;
                case "是":
                    return true;
                case "否":
                    return false;
                case "对":
                    return true;
                case "错":
                    return false;
                case "真":
                    return true;
                case "假":
                    return false;
                case "禁用":
                    return false;
                case "启用":
                    return true;
                case "true":
                    return true;
                case "false":
                    return false;
                case "enable":
                    return true;
                case "disable":
                    return false;
                case "enabled":
                    return true;
                case "disabled":
                    return false;
                case "open":
                    return true;
                case "close":
                    return false;
                case "openning":
                    return true;
                case "opening":
                    return true;
                case "closed":
                    return false;
                case "start":
                    return true;
                case "stop":
                    return false;
                case "started":
                    return true;
                case "stoped":
                    return false;
                default:
                    return defaultResult;
            }
        }
        public static string HideSomeString(this string value, int offset, int count)
        {
            string s1 = value.Substring(0, offset - 1);
            return null;
        }
        public static string RepeateString(this string value, int repeateTimes)
        {
            string s = "";
            for (int i = 0; i < repeateTimes; i++)
            {
                s += value;
            }
            return s;
        }
        public static string FillLength(string value, int length, string fillStr, bool onAfter)
        {
            int fInChrC = length - value.Length;
            if (fInChrC < 1) return value;

            for (int i = 0; i < fInChrC; i++)
            {
                if (onAfter)
                    value += fillStr;
                else value = fillStr + value;
            }
            return value;
        }
        /// <summary>
        /// 保持一个可能过长字符串的长度，如果过长，则截断字符串
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <param name="maxlen">最大长度</param>
        /// <param name="str">截断时，使用一个特定的字符串进行衔接</param>
        /// <returns></returns>
        public static string KeepStringLenght(this string value, int maxlen, string str)
        {
            if (value.Length > maxlen)
            {
                int halflen = (maxlen - str.Length) / 2;
                string result = value.Substring(0, halflen);
                result += str + value.Substring(value.Length - halflen, halflen);
                return result;
            }
            else return value;
        }
    }
}
