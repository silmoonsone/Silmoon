using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Silmoon.Extension
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
        public static int GetLengthEncoded(this string value, Encoding encoding) => encoding.GetByteCount(value);
        public static int GetLengthSpecial(this string s)
        {
            // 如果字符串为空或者为 null，直接返回长度为0
            if (string.IsNullOrEmpty(s)) return 0;

            // 创建一个 TextElementEnumerator 对象来遍历字符串中的文本元素
            // TextElementEnumerator 可以正确处理由多个 Unicode 标量组成的复合字符
            var enumerator = StringInfo.GetTextElementEnumerator(s);

            int count = 0;  // 记录已处理的字符数

            // 遍历字符串中的每一个文本元素
            while (enumerator.MoveNext())
            {
                string textElement = enumerator.GetTextElement(); // 获取当前的文本元素（一个完整的字符）

                // 检查当前字符是否为非西文字符
                // 这里我们简单地假定任何非 ASCII 字符都为非西文字符
                bool isNonWestern = textElement.Any(c => c > 127);

                count += isNonWestern ? 2 : 1;  // 非西文字符占两个位置，西文字符占一个位置
            }

            // 返回计算出的长度
            return count;
        }

        public static bool IsNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.All(char.IsDigit);
        }
        public static bool IsDecimal(this string value)
        {
            decimal result;
            return decimal.TryParse(value, out result);
        }

        public static string SubstringEncoded(this string s, int length, Encoding encoding)
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
        public static string SubstringSpecial(this string s, int startIndex, int length)
        {
            // 如果字符串为空或者为 null，直接返回原始的字符串
            if (string.IsNullOrEmpty(s)) return s;

            // 创建一个 TextElementEnumerator 对象来遍历字符串中的文本元素
            // TextElementEnumerator 可以正确处理由多个 Unicode 标量组成的复合字符
            var enumerator = StringInfo.GetTextElementEnumerator(s);

            var result = new StringBuilder();
            int count = 0;  // 记录已处理的字符数

            // 遍历字符串中的每一个文本元素
            while (enumerator.MoveNext())
            {
                string textElement = enumerator.GetTextElement(); // 获取当前的文本元素（一个完整的字符）

                // 检查当前字符是否为非西文字符
                // 这里我们简单地假定任何非 ASCII 字符都为非西文字符
                bool isNonWestern = textElement.Any(c => c > 127);

                // 调整 startIndex 的计算，考虑非西文字符的长度
                if (startIndex > 0)
                {
                    startIndex -= isNonWestern ? 2 : 1;
                    continue;
                }

                // 如果已处理的字符数加上当前字符的长度大于期望的长度，则停止遍历
                if (count + (isNonWestern ? 2 : 1) > length) break;

                // 添加到结果字符串中
                result.Append(textElement);
                count += isNonWestern ? 2 : 1;  // 非西文字符占两个位置，西文字符占一个位置
            }

            // 返回结果字符串
            return result.ToString();
        }

        public static string InsertSeparator(this string input, int groupSize, string separator = " ")
        {
            if (string.IsNullOrEmpty(input)) return input;

            // 去除输入字符串中可能存在的分隔符
            //input = input.Replace(separator, "");

            string result = string.Empty;
            for (int i = 0; i < input.Length; i += groupSize)
            {
                int length = Math.Min(groupSize, input.Length - i);
                result += input.Substring(i, length);

                if (i + length < input.Length)
                {
                    result += separator;
                }
            }

            return result;
        }

        public static string EndSubstring(this string s, int endIndex, bool strictMode = false)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (endIndex <= 0) return "";
            if (strictMode && endIndex > s.Length)
                throw new ArgumentOutOfRangeException(nameof(endIndex), "endIndex cannot be larger than length of string.");
            if (endIndex > s.Length) return s;

            return s.Substring(s.Length - endIndex);
        }
        public static string EndSubstring(this string s, int endIndex, int length, bool strictMode = false)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (length <= 0 || endIndex <= 0) return "";
            if (strictMode && (endIndex > s.Length || length > endIndex))
                throw new ArgumentOutOfRangeException(endIndex > s.Length ? nameof(endIndex) : nameof(length), "Parameter cannot be larger than length of string or endIndex.");

            if (endIndex > s.Length) endIndex = s.Length;
            if (length > endIndex) return ""; // 当 strictMode 为 false 时的行为

            return s.Substring(s.Length - endIndex, length);
        }
        public static string SubstringSafe(this string s, int startIndex, int length, bool strictMode = false)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (startIndex < 0) startIndex = 0; // 调整 startIndex 至最小值 0
            if (length < 0) length = s.Length - startIndex; // 如果 length 为 -1，取到字符串末尾
            if (strictMode)
            {
                if (startIndex > s.Length) throw new ArgumentOutOfRangeException(nameof(startIndex), "startIndex cannot be larger than length of string.");
                if (startIndex + length > s.Length) throw new ArgumentOutOfRangeException(nameof(length), "Length goes beyond the end of the string.");
                return s.Substring(startIndex, length);
            }
            if (startIndex >= s.Length) return ""; // 如果 startIndex 超出字符串长度，返回空字符串
            if (startIndex + length > s.Length) length = s.Length - startIndex; // 调整 length 以不超过字符串末尾

            return s.Substring(startIndex, length);
        }
        public static string SubstringSafe(this string s, int startIndex, bool strictMode = false) => SubstringSafe(s, startIndex, -1, strictMode);

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
            catch (Exception ex)
            {
                if (throwException) throw ex;
                else return default;
            }
        }

        public static string ToBase64String(this string value, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var data = Convert.ToBase64String(encoding.GetBytes(value));
            return data;
        }
        public static string TrimWithoutNull(this string value) => value?.Trim();
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
        public static string HidePart(this string value, int index, int length, string replacement = "*")
        {
            // 检查参数合法性
            if (index < 0 || index >= value.Length || index + length > value.Length || length < 0)
            {
                throw new ArgumentOutOfRangeException("参数start或length的值不合法");
            }

            // 生成用于替换的字符串
            string replaceWith = replacement != string.Empty ? new string(replacement[0], length) : string.Empty;

            // 返回替换后的字符串
            return value.Substring(0, index) + replaceWith + value.Substring(index + length);
        }
        public static string RepeatString(this string value, int repeateTimes)
        {
            string s = string.Empty;
            for (int i = 0; i < repeateTimes; i++)
            {
                s += value;
            }
            return s;
        }
        public static string Fill(this string str, int Length, string FillStr, bool Append = true)
        {
            int clength = Length - str.Length;
            if (clength < 1) return str;

            for (int i = 0; i < clength; i++)
            {
                if (Append) str += FillStr;
                else str = FillStr + str;
            }
            return str;
        }
        /// <summary>
        /// 保持一个可能过长字符串的长度，如果过长，则截断字符串
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <param name="maxlen">最大长度</param>
        /// <param name="str">截断时，使用一个特定的字符串进行衔接</param>
        /// <returns></returns>
        public static string KeepLessStringLength(this string value, int maxlen, string str)
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
        public static string AdjustStringLength(this string str, int Length, bool SubStringFromStart, char PadChar, bool Append)
        {
            if (str.Length > Length)
            {
                if (SubStringFromStart)
                    str = str.Substring(0, Length);
                else
                    str = str.Substring(str.Length - Length);
            }
            else if (str.Length < Length)
            {
                var padding = new string(PadChar, Length - str.Length);
                if (Append)
                    str = str + padding;
                else
                    str = padding + str;
            }
            return str;
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
        public static StateSet<Strength> PasswordStrength(this string password)
        {
            // 空字符串强度值为0
            if (string.IsNullOrEmpty(password)) return StateSet<Strength>.Create(Strength.Invalid, "密码不能为空");

            // 字符统计
            int iNum = 0, iLttLower = 0, iLttUpper = 0, iSym = 0;
            foreach (char c in password)
            {
                if (char.IsDigit(c)) iNum++;
                else if (char.IsLower(c)) iLttLower++;
                else if (char.IsUpper(c)) iLttUpper++;
                else iSym++;
            }

            // 判断密码种类的数量
            int typesCount = (iNum > 0 ? 1 : 0) + ((iLttLower > 0 || iLttUpper > 0) ? 1 : 0) + (iSym > 0 ? 1 : 0);

            if (typesCount == 1) // 只有一种类型的字符
            {
                if (iNum > 0) return StateSet<Strength>.Create(Strength.Weak, "纯数字密码");
                if (iSym > 0) return StateSet<Strength>.Create(Strength.Weak, "纯符号密码");
                return StateSet<Strength>.Create(Strength.Weak, "纯字母密码");
            }

            if (password.Length <= 6) return StateSet<Strength>.Create(Strength.Weak, "长度不大于6的密码");

            if (typesCount == 2) // 有两种类型的字符
            {
                if (iLttLower > 0 && iLttUpper > 0) return StateSet<Strength>.Create(Strength.Normal, "大小写字母混合密码");
                if (iNum > 0 && iSym > 0) return StateSet<Strength>.Create(Strength.Normal, "数字和符号构成的密码");
                if (iNum > 0 && (iLttLower > 0 || iLttUpper > 0)) return StateSet<Strength>.Create(Strength.Normal, "数字和字母构成的密码");
                return StateSet<Strength>.Create(Strength.Normal, "字母和符号构成的密码");
            }

            if (password.Length <= 10) return StateSet<Strength>.Create(Strength.Normal, "长度不大于10的密码");

            return StateSet<Strength>.Create(Strength.Strong); //由数字、字母、符号构成的密码
        }
        public static string Substring(this string str, int count, string suffix = StringHelper.EmptyString) => str.Length > count ? str.Substring(0, count) + suffix : str;
        /// <summary>
        /// 强制脱去HTML、script标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StripHtml(this string str) => Regex.Replace(str, "<.*?>", string.Empty);
        public static bool CheckStringLengthGte(this string str, int length) => str?.Length >= length;


        /// <summary>
        /// 字符串是否是电子邮件地址
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string Email)
        {
            if (string.IsNullOrEmpty(Email)) return false;
            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return regex.IsMatch(Email);
        }
        /// <summary>
        /// 字符串是否是手机号码
        /// </summary>
        /// <param name="MobilePhone"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(this string MobilePhone)
        {
            if (string.IsNullOrEmpty(MobilePhone)) return false;
            Regex regex = new Regex(@"^((1[3,5,6,8][0-9])|(14[5,7])|(17[0,1,2,3,5,6,7,8])|(19[1,8,9]))\d{8}$");
            return regex.IsMatch(MobilePhone);
        }
        /// <summary>
        /// 字符串是否是固定电话号码
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static bool IsPhone(this string Phone)
        {
            if (string.IsNullOrEmpty(Phone)) return false;
            Regex regex = new Regex(@"^(\d{3,4}-)?\d{6,8}$");
            return regex.IsMatch(Phone);
        }
        /// <summary>
        /// 字符串是否是身份证号码
        /// </summary>
        /// <param name="CardId"></param>
        /// <returns></returns>
        public static bool IsCardId(this string CardId)
        {
            if (CardId is null || CardId.Trim().Length != 18) return false;
            long n = 0;
            //数字验证
            if (long.TryParse(CardId.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(CardId.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;
            }

            //省份验证
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(CardId.Remove(2)) == -1)
            {
                return false;
            }

            //生日验证
            string birth = CardId.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;
            }
            //校验码验证
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = CardId.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != CardId.Substring(17, 1).ToLower())
            {
                return false;
            }
            return true;
            //符合GB11643-1999标准
        }
        /// <summary>
        /// 字符串是否是营业执照许可号码
        /// </summary>
        /// <param name="BusinessLicense"></param>
        /// <returns></returns>
        public static bool IsBusinessLicense(this string BusinessLicense)
        {
            if (string.IsNullOrEmpty(BusinessLicense)) return false;
            Regex regex = new Regex(@"^[0-9A-Z]{8}-[0-9A-Z]$");
            return regex.IsMatch(BusinessLicense);
        }
        /// <summary>
        /// 检查字符串是否是Url
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="RequireHttps">是否要求强制https</param>
        /// <returns></returns>
        public static bool IsUrl(this string Url, bool RequireHttps = false)
        {
            if (Url.IsNullOrEmpty()) return false;
            // 验证 URL 的正则表达式
            string pattern = RequireHttps ?
                @"^https:\/\/([\da-z\.-]+)\.([a-z\.]{2,6})(:[0-9]{1,5})?([\/\w \.-]*)*\/?$" :
                @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})(:[0-9]{1,5})?([\/\w \.-]*)*\/?$";

            // 创建一个 Regex 对象
            Regex regex = new Regex(pattern);

            // 检查 URL 是否符合格式
            return regex.IsMatch(Url);
        }
        /// <summary>
        /// 检查字符串是否是IPv4地址
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool IsIPv4Address(this string IP)
        {
            if (string.IsNullOrEmpty(IP)) return false;
            Regex regex = new Regex(@"^(\d+)\.(\d+)\.(\d+)\.(\d+)$");
            if (regex.IsMatch(IP))
            {
                if (Regex.IsMatch(IP, @"^0\.\d+\.0\.\d+$")) return false;
                string[] arr = IP.Split('.');
                if (int.Parse(arr[0]) < 256 && int.Parse(arr[1]) < 256 && int.Parse(arr[2]) < 256 && int.Parse(arr[3]) < 256) return true;
            }
            return false;
        }
        /// <summary>
        /// 检查字符串是否是IPv6地址
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool IsIPv6Address(this string IP)
        {
            if (string.IsNullOrEmpty(IP)) return false;
            Regex regex = new Regex(@"^([\da-fA-F]{1,4}:){7}[\da-fA-F]{1,4}$");
            return regex.IsMatch(IP);
        }
        /// <summary>
        /// 检查字符串是否是IP地址，使用了IPv4和IPv6的检查
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool IsIPAddress(this string IP) => IP.IsIPv4Address() || IP.IsIPv6Address();



        // 正则表达式匹配 SemVer 和四位版本号
        private static readonly Regex SemVerRegex = new Regex(@"^\d+\.\d+\.\d+(\.\d+)?$|^\d+\.\d+\.\d+(-[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?(\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?$", RegexOptions.Compiled);
        public static bool IsValidVersion(this string version) => !string.IsNullOrWhiteSpace(version) && SemVerRegex.IsMatch(version);
        public static bool IsNewerVersionThan(this string version1, string version2, bool equalIsNewer = false)
        {
            if (!version1.IsValidVersion()) throw new ArgumentException($"The version number '{version1}' is not valid.", nameof(version1));
            if (!version2.IsValidVersion()) throw new ArgumentException($"The version number '{version2}' is not valid.", nameof(version2));

            var version1Parts = version1.Split(new[] { '-', '+' }, 2)[0].Split('.');
            var version2Parts = version2.Split(new[] { '-', '+' }, 2)[0].Split('.');

            for (int i = 0; i < Math.Max(version1Parts.Length, version2Parts.Length); i++)
            {
                int version1Part = i < version1Parts.Length ? int.Parse(version1Parts[i]) : 0;
                int version2Part = i < version2Parts.Length ? int.Parse(version2Parts[i]) : 0;

                if (version1Part < version2Part) return false;
                else if (version1Part > version2Part) return true;
            }

            var version1PreRelease = GetPreReleaseTag(version1);
            var version2PreRelease = GetPreReleaseTag(version2);

            int preReleaseComparison = ComparePreRelease(version1PreRelease, version2PreRelease);
            if (preReleaseComparison < 0) return false;
            else if (preReleaseComparison > 0) return true;

            return equalIsNewer;
        }
        static string GetPreReleaseTag(string version)
        {
            var parts = version.Split('-');
            if (parts.Length > 1)
            {
                var preReleasePart = parts[1];
                return preReleasePart.Split('+')[0];
            }
            return string.Empty;
        }
        static int ComparePreRelease(string pre1, string pre2)
        {
            if (string.IsNullOrEmpty(pre1) && !string.IsNullOrEmpty(pre2)) return 1;
            if (!string.IsNullOrEmpty(pre1) && string.IsNullOrEmpty(pre2)) return -1;
            if (string.IsNullOrEmpty(pre1) && string.IsNullOrEmpty(pre2)) return 0;

            var pre1Parts = pre1.Split('.');
            var pre2Parts = pre2.Split('.');

            for (int i = 0; i < Math.Max(pre1Parts.Length, pre2Parts.Length); i++)
            {
                var part1 = i < pre1Parts.Length ? pre1Parts[i] : string.Empty;
                var part2 = i < pre2Parts.Length ? pre2Parts[i] : string.Empty;

                int result;
                if (int.TryParse(part1, out var int1) && int.TryParse(part2, out var int2))
                    result = int1.CompareTo(int2);
                else
                    result = string.Compare(part1, part2, StringComparison.Ordinal);

                if (result != 0) return result;
            }
            return 0;
        }



        public static string AppendUriQueryString(this string url, string queryString)
        {
            return url.Contains("?") ? $"{url}&{queryString}" : $"{url}?{queryString}";
        }
        public static string AppendUriQueryString(this string url, string key, string value)
        {
            return url.Contains("?") ? $"{url}&{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}" : $"{url}?{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}";
        }

        public static string RemoveUriQueryString(this string url, string key)
        {
            if (!url.Contains("?") || string.IsNullOrEmpty(key))
            {
                return url;
            }

            var baseUrl = url.Substring(0, url.IndexOf("?"));
            var queryString = url.Substring(url.IndexOf("?") + 1);
            var newQueryString = string.Empty;

            var queryParams = HttpUtility.ParseQueryString(queryString);
            queryParams.Remove(key);

            if (queryParams.HasKeys())
            {
                newQueryString = string.Join("&", queryParams.AllKeys
                    .Select(k => $"{HttpUtility.UrlEncode(k)}={HttpUtility.UrlEncode(queryParams[k])}"));
            }

            return $"{baseUrl}?{newQueryString}";
        }
        public static string GetUriQueryStringValueFromUrl(this string url, string key)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key)) return null;
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            return query[key];
        }
        public static string GetUriQueryStringValueFromString(this string url, string key)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key)) return null;
            var query = HttpUtility.ParseQueryString(url);
            return query[key];
        }
        /// <summary>
        /// Default encoding is UTF8
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str, Encoding encoding = null)
        {
            if (encoding is null) encoding = Encoding.UTF8;
            return encoding.GetBytes(str);
        }

        public static bool IsHexString(this string value)
        {
            bool isHex;
            value = value.Substring(value.StartsWith("0x") ? 2 : 0);
            foreach (var c in value)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex) return false;
            }
            return true;
        }
        public static StateSet<bool, byte[]> HexStringToByteArray(this string hex)
        {
            if (hex is null) return false.ToStateSet<byte[]>(null, "hex string value is null");
            if (!IsHexString(hex)) return false.ToStateSet<byte[]>(null, "hex string value not is HexString.");

            if (hex.StartsWith("0x")) hex = hex.Substring(2);
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];

            if (numberChars % 2 != 0)
            {
                hex = "0" + hex;
                numberChars = hex.Length;
            }
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return true.ToStateSet(bytes);
        }
        public static BigInteger HexStringToBigInteger(this string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            if (value.StartsWith("0x")) value = value.Substring(2);

            return BigInteger.Parse(value, NumberStyles.HexNumber);
        }


        public static string Base64UrlToBase64(this string base64Url)
        {
            // 将 Base64URL 格式的字符串转换为标准 Base64
            string base64 = base64Url.Replace('-', '+').Replace('_', '/');
            // 补充 `=` 使长度是4的倍数
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return base64;
        }
        public static string Base64ToBase64Url(this string base64)
        {
            // 将标准 Base64 格式的字符串转换为 Base64URL
            return base64.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

    }
}
