using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Secure
{
    public static class HashHelper
    {
        private static Random Random { get; set; } = new Random();

        public static string GetMD5Hash(this string s) => s.GetMD5Hash(Encoding.UTF8);
        public static string GetMD5Hash(this string s, Encoding encoding)
        {
            using (var c = new MD5CryptoServiceProvider())
            {
                byte[] data = c.ComputeHash(s.GetBytes(encoding));
                return BitConverter.ToString(data).Replace("-", "").ToLower();
            }
        }
        public static string GetSHA1Hash(this string s) => s.GetSHA1Hash(Encoding.UTF8);
        public static string GetSHA1Hash(this string s, Encoding encoding)
        {
            using (var c = new SHA1CryptoServiceProvider())
            {
                byte[] bresult = c.ComputeHash(s.GetBytes(encoding));
                return BitConverter.ToString(bresult).Replace("-", "").ToLower();
            }
        }
        public static string GetSHA256Hash(this string s) => s.GetSHA256Hash(Encoding.UTF8);
        public static string GetSHA256Hash(this string s, Encoding encoding)
        {
            using (var c = new SHA256CryptoServiceProvider())
            {
                byte[] bresult = c.ComputeHash(s.GetBytes(encoding));
                return BitConverter.ToString(bresult).Replace("-", "").ToLower();
            }
        }

        public static string RandomNumbers(int length, bool firstNotZero = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            lock (Random)
            {
                for (int i = 0; i < length; i++)
                {
                    int num = Random.Next(0, 9);
                    if (firstNotZero && stringBuilder.Length == 0 && num == 0)
                    {
                        i--;
                        continue;
                    }
                    stringBuilder.Append(num);
                }
            }
            return stringBuilder.ToString();
        }
        public static string RandomChars(int length, bool IncludeUpper = true, bool IncludeLower = true, bool IncludeNumbers = true)
        {
            if (!IncludeUpper && !IncludeLower && !IncludeNumbers) throw new InvalidOperationException("指定的随机选项全部是False，也就是说随机不出来任何字符");
            StringBuilder stringBuilder = new StringBuilder();
            lock (Random)
            {
                for (int i = 0; i < length; i++)
                {
                    int code = Random.Next('0', 'z');
                    if ((code >= ':' && code <= '@') || code >= '[' && code <= '`')
                    {
                        i--;
                        continue;
                    }
                    if (!IncludeUpper && (code >= 'A' && code <= 'Z'))
                    {
                        i--;
                        continue;
                    }
                    if (!IncludeLower && (code >= 'a' && code <= 'z'))
                    {
                        i--;
                        continue;
                    }
                    if (!IncludeNumbers && (code >= '0' && code <= '9'))
                    {
                        i--;
                        continue;
                    }

                    stringBuilder.Append((char)code);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
