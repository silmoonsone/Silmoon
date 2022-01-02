﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

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

        public static string EncodeSubstring(this string s, int length, Encoding encoding)
        {
            if (s.IsNullOrEmpty()) return s;
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
            var data = Convert.ToBase64String(encoding.GetBytes(value));
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
                case "":
                case "0":
                case "n":
                case "no":
                case "off":
                case "否":
                case "错":
                case "假":
                case "禁用":
                case "false":
                case "disable":
                case "disabled":
                case "close":
                case "closed":
                case "stop":
                case "stoped":
                    return false;
                case "1":
                case "y":
                case "yes":
                case "ok":
                case "not":
                case "on":
                case "是":
                case "对":
                case "真":
                case "启用":
                case "true":
                case "enable":
                case "enabled":
                case "open":
                case "openning":
                case "opening":
                case "start":
                case "started":
                    return true;
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
        public static string Fill(this string value, int length, string fillStr, bool onAfter = true)
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
        public enum Strength
        {
            Invalid = 0,
            Weak = 1,
            Normal = 2,
            Strong = 3,
        };
        /// <summary>
        /// 计算密码强度
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        public static Strength PasswordStrength(this string password)
        {
            //空字符串强度值为0
            if (password == "") return Strength.Invalid;
            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }
            if (iLtt == 0 && iSym == 0) return Strength.Weak; //纯数字密码
            if (iNum == 0 && iLtt == 0) return Strength.Weak; //纯符号密码
            if (iNum == 0 && iSym == 0) return Strength.Weak; //纯字母密码
            if (password.Length <= 6) return Strength.Weak; //长度不大于6的密码
            if (iLtt == 0) return Strength.Normal; //数字和符号构成的密码
            if (iSym == 0) return Strength.Normal; //数字和字母构成的密码
            if (iNum == 0) return Strength.Normal; //字母和符号构成的密码
            if (password.Length <= 10) return Strength.Normal; //长度不大于10的密码
            return Strength.Strong; //由数字、字母、符号构成的密码
        }
        public static string Substring(this string str, int count, string perfix = "")
        {
            if (str.Length > count)
            {
                return str.Substring(0, count) + perfix;
            }
            else { return str; }
        }
        public static string StripHtml(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
        public static bool LastSubstringEqual(this string s, string str)
        {
            if (s.IsNullOrEmpty() || str.IsNullOrEmpty()) return false;
            var lastIndexOf = s.LastIndexOf(str);
            if (lastIndexOf == -1) return false;
            else
            {
                return lastIndexOf == s.Length - str.Length;
            }
        }
        public static bool StartSubstringEqual(this string s, string str)
        {
            if (s.IsNullOrEmpty() || str.IsNullOrEmpty()) return false;
            var indexOf = s.IndexOf(str);
            if (indexOf == -1) return false;
            else
            {
                return indexOf == 0;
            }
        }

    }
}
