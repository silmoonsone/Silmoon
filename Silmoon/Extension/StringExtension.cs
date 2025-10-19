using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Silmoon.Extension
{
    public static class StringExtension
    {
        // 编译时正则表达式缓存，提升性能
        private static readonly Regex EmailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled);
        private static readonly Regex MobilePhoneRegex = new Regex(@"^((1[3,5,6,8][0-9])|(14[5,7])|(17[0,1,2,3,5,6,7,8])|(19[1,8,9]))\d{8}$", RegexOptions.Compiled);
        private static readonly Regex PhoneRegex = new Regex(@"^(\d{3,4}-)?\d{6,8}$", RegexOptions.Compiled);
        private static readonly Regex BusinessLicenseRegex = new Regex(@"^[0-9A-Z]{8}-[0-9A-Z]$", RegexOptions.Compiled);
        private static readonly Regex UrlRegex = new Regex(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})(:[0-9]{1,5})?([\/\w \.-]*)*\/?$", RegexOptions.Compiled);
        private static readonly Regex HttpsUrlRegex = new Regex(@"^https:\/\/([\da-z\.-]+)\.([a-z\.]{2,6})(:[0-9]{1,5})?([\/\w \.-]*)*\/?$", RegexOptions.Compiled);
        private static readonly Regex HtmlTagRegex = new Regex("<.*?>", RegexOptions.Compiled);
        /// <summary>
        /// 将字符串数组中的所有元素使用指定的分隔符合并成一个字符串。
        /// </summary>
        /// <param name="array">要合并的字符串数组</param>
        /// <param name="split">用于分隔元素的字符串</param>
        /// <param name="removeLastSplit">是否移除最后一个分隔符，默认为 true</param>
        /// <param name="prefix">每个元素的前缀，默认为空字符串</param>
        /// <param name="suffix">每个元素的后缀，默认为空字符串</param>
        /// <returns>合并后的字符串</returns>
        /// <example>
        /// <code>
        /// string[] items = {"apple", "banana", "cherry"};
        /// string result = items.Join(", "); // 结果: "apple, banana, cherry"
        /// 
        /// string[] tags = {"C#", "JavaScript", "Python"};
        /// string html = tags.Join("", true, "&lt;", "&gt;"); // 结果: "&lt;C#&gt;&lt;JavaScript&gt;&lt;Python&gt;"
        /// </code>
        /// </example>
        public static string Join(this string[] array, string split, bool removeLastSplit = true, string prefix = StringHelper.EmptyString, string suffix = StringHelper.EmptyString)
        {
            if (array == null || array.Length == 0) return string.Empty;

            var stringBuilder = new StringBuilder();
            foreach (object s in array)
            {
                stringBuilder.Append(prefix);
                stringBuilder.Append(s);
                stringBuilder.Append(suffix);
                stringBuilder.Append(split);
            }

            if (removeLastSplit && stringBuilder.Length > 0)
            {
                stringBuilder.Length -= split.Length;
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 从字符串数组中移除重复的元素，返回一个新的不包含重复元素的数组。
        /// 保持元素的原始顺序。
        /// </summary>
        /// <param name="array">要去重的字符串数组</param>
        /// <returns>不包含重复元素的新数组，如果输入为 null 或空数组则返回空数组</returns>
        /// <example>
        /// <code>
        /// string[] items = {"apple", "banana", "apple", "cherry", "banana"};
        /// string[] unique = items.Distinct(); // 结果: {"apple", "banana", "cherry"}
        /// 
        /// string[] empty = null;
        /// string[] result = empty.Distinct(); // 结果: {}
        /// </code>
        /// </example>
        public static string[] Distinct(this string[] array)
        {
            if (array == null || array.Length == 0) return array ?? new string[0];

            var seen = new HashSet<string>();
            var result = new List<string>(array.Length); // 预分配容量

            foreach (string item in array)
            {
                if (seen.Add(item)) // HashSet.Add 返回 true 表示元素不存在
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 将字符串数组转换为 BigInteger 数组。
        /// </summary>
        /// <param name="strArray">要转换的字符串数组</param>
        /// <returns>BigInteger 数组，如果输入为 null 则返回 null</returns>
        /// <exception cref="FormatException">当字符串无法解析为数字时抛出</exception>
        /// <example>
        /// <code>
        /// string[] numbers = {"12345678901234567890", "98765432109876543210"};
        /// BigInteger[] bigInts = numbers.ToBigIntegerArray();
        /// 
        /// string[] invalid = {"123", "abc", "456"}; // 会抛出 FormatException
        /// </code>
        /// </example>
        public static BigInteger[] ToBigIntegerArray(this string[] strArray)
        {
            if (strArray is null) return null;
            List<BigInteger> bigIntegers = new List<BigInteger>();
            foreach (var item in strArray)
            {
                bigIntegers.Add(BigInteger.Parse(item));
            }
            return bigIntegers.ToArray();
        }

        /// <summary>
        /// 检查字符串是否为 null 或空字符串。
        /// </summary>
        /// <param name="value">要检查的字符串</param>
        /// <returns>如果字符串为 null 或空字符串则返回 true，否则返回 false</returns>
        /// <example>
        /// <code>
        /// string text1 = null;
        /// bool result1 = text1.IsNullOrEmpty(); // 结果: true
        /// 
        /// string text2 = "";
        /// bool result2 = text2.IsNullOrEmpty(); // 结果: true
        /// 
        /// string text3 = "Hello";
        /// bool result3 = text3.IsNullOrEmpty(); // 结果: false
        /// </code>
        /// </example>
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
        /// <summary>
        /// 获取字符串在指定编码下的字节长度。
        /// </summary>
        /// <param name="value">要计算长度的字符串</param>
        /// <param name="encoding">用于编码的字符编码</param>
        /// <returns>字符串在指定编码下的字节数</returns>
        /// <example>
        /// <code>
        /// string text = "Hello 世界";
        /// int utf8Length = text.GetLengthEncoded(Encoding.UTF8); // 结果: 11 (UTF-8编码)
        /// int asciiLength = text.GetLengthEncoded(Encoding.ASCII); // 结果: 7 (ASCII编码，中文字符会被替换)
        /// </code>
        /// </example>
        public static int GetLengthEncoded(this string value, Encoding encoding) => encoding.GetByteCount(value);
        /// <summary>
        /// 获取字符串的显示长度，考虑中文字符占两个显示位置。
        /// 用于在等宽字体环境下计算字符串的实际显示宽度。
        /// </summary>
        /// <param name="s">要计算显示长度的字符串</param>
        /// <returns>字符串的显示长度，中文字符计为2，英文字符计为1</returns>
        /// <example>
        /// <code>
        /// string text1 = "Hello";
        /// int length1 = text1.GetDisplayLength(); // 结果: 5
        /// 
        /// string text2 = "你好";
        /// int length2 = text2.GetDisplayLength(); // 结果: 4
        /// 
        /// string text3 = "Hello世界";
        /// int length3 = text3.GetDisplayLength(); // 结果: 9 (5 + 4)
        /// </code>
        /// </example>
        public static int GetDisplayLength(this string s)
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


        /// <summary>
        /// 检查字符串是否只包含数字字符。
        /// </summary>
        /// <param name="value">要检查的字符串</param>
        /// <returns>如果字符串只包含数字字符则返回 true，否则返回 false</returns>
        /// <example>
        /// <code>
        /// string text1 = "12345";
        /// bool result1 = text1.IsNumber(); // 结果: true
        /// 
        /// string text2 = "123.45";
        /// bool result2 = text2.IsNumber(); // 结果: false (包含小数点)
        /// 
        /// string text3 = "abc123";
        /// bool result3 = text3.IsNumber(); // 结果: false (包含字母)
        /// </code>
        /// </example>
        public static bool IsNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.All(char.IsDigit);
        }
        /// <summary>
        /// 检查字符串是否可以解析为 decimal 类型的数值。
        /// </summary>
        /// <param name="value">要检查的字符串</param>
        /// <returns>如果字符串可以解析为 decimal 则返回 true，否则返回 false</returns>
        /// <example>
        /// <code>
        /// string text1 = "123.45";
        /// bool result1 = text1.IsDecimal(); // 结果: true
        /// 
        /// string text2 = "-99.99";
        /// bool result2 = text2.IsDecimal(); // 结果: true
        /// 
        /// string text3 = "abc";
        /// bool result3 = text3.IsDecimal(); // 结果: false
        /// 
        /// string text4 = "123,456.78";
        /// bool result4 = text4.IsDecimal(); // 结果: false (包含千位分隔符)
        /// </code>
        /// </example>
        public static bool IsDecimal(this string value)
        {
            decimal result;
            return decimal.TryParse(value, out result);
        }

        /// <summary>
        /// 按指定编码的字节长度截取字符串。
        /// 主要用于处理包含中文字符的字符串，确保截取后的字符串不会破坏字符的完整性。
        /// </summary>
        /// <param name="s">要截取的字符串</param>
        /// <param name="length">要截取的字节长度</param>
        /// <param name="encoding">用于编码的字符编码</param>
        /// <returns>截取后的字符串</returns>
        /// <example>
        /// <code>
        /// string text = "Hello世界";
        /// string result = text.SubstringEncoded(7, Encoding.UTF8); // 结果: "Hello世" (7个字节)
        /// 
        /// string chinese = "你好世界";
        /// string sub = chinese.SubstringEncoded(6, Encoding.UTF8); // 结果: "你好" (6个字节，3个字符)
        /// </code>
        /// </example>
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
        /// <summary>
        /// 按显示长度截取字符串，考虑中文字符占两个显示位置。
        /// 用于在等宽字体环境下按显示宽度截取字符串。
        /// </summary>
        /// <param name="s">要截取的字符串</param>
        /// <param name="startIndex">开始索引（按显示长度计算）</param>
        /// <param name="length">要截取的显示长度</param>
        /// <returns>截取后的字符串</returns>
        /// <example>
        /// <code>
        /// string text = "Hello世界";
        /// string result = text.SubstringByDisplayLength(0, 7); // 结果: "Hello世" (7个显示位置)
        /// 
        /// string chinese = "你好世界";
        /// string sub = chinese.SubstringByDisplayLength(2, 4); // 结果: "好世" (从第2个位置开始，4个显示位置)
        /// </code>
        /// </example>
        public static string SubstringByDisplayLength(this string s, int startIndex, int length)
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


        /// <summary>
        /// 在字符串中按指定间隔插入分隔符。
        /// 常用于格式化数字、银行卡号等需要分组显示的字符串。
        /// </summary>
        /// <param name="input">要处理的字符串</param>
        /// <param name="groupSize">每组字符的数量</param>
        /// <param name="separator">分隔符，默认为空格</param>
        /// <returns>插入分隔符后的字符串</returns>
        /// <example>
        /// <code>
        /// string number = "1234567890";
        /// string formatted = number.InsertSeparator(4, " "); // 结果: "1234 5678 90"
        /// 
        /// string card = "1234567890123456";
        /// string cardFormatted = card.InsertSeparator(4, "-"); // 结果: "1234-5678-9012-3456"
        /// </code>
        /// </example>
        public static string InsertSeparator(this string input, int groupSize, string separator = " ")
        {
            if (string.IsNullOrEmpty(input)) return input;

            // 去除输入字符串中可能存在的分隔符
            //input = input.Replace(separator, "");

            var stringBuilder = new StringBuilder(input.Length + (input.Length / groupSize) * separator.Length);
            for (int i = 0; i < input.Length; i += groupSize)
            {
                int length = Math.Min(groupSize, input.Length - i);
                stringBuilder.Append(input.Substring(i, length));

                if (i + length < input.Length)
                {
                    stringBuilder.Append(separator);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 从字符串末尾开始截取指定长度的子字符串。
        /// </summary>
        /// <param name="s">要截取的字符串</param>
        /// <param name="endIndex">从末尾开始截取的字符数量</param>
        /// <param name="strictMode">是否启用严格模式，严格模式下会检查参数有效性</param>
        /// <returns>截取后的字符串</returns>
        /// <exception cref="ArgumentOutOfRangeException">当 strictMode 为 true 且 endIndex 超出字符串长度时抛出</exception>
        /// <example>
        /// <code>
        /// string text = "Hello World";
        /// string result = text.EndSubstring(5); // 结果: "World"
        /// 
        /// string shortText = "Hi";
        /// string result2 = shortText.EndSubstring(5); // 结果: "Hi" (超出长度时返回原字符串)
        /// </code>
        /// </example>
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
        /// <summary>
        /// 将字符串转换为布尔值。
        /// 支持多种表示真/假的字符串格式，包括中英文。
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defaultResult">当字符串不匹配任何已知格式时的默认返回值</param>
        /// <param name="stringNullOrEmptyResult">当字符串为 null 或空时的返回值</param>
        /// <returns>转换后的布尔值</returns>
        /// <example>
        /// <code>
        /// string text1 = "true";
        /// bool result1 = text1.ToBool(); // 结果: true
        /// 
        /// string text2 = "是";
        /// bool result2 = text2.ToBool(); // 结果: true
        /// 
        /// string text3 = "false";
        /// bool result3 = text3.ToBool(); // 结果: false
        /// 
        /// string text4 = "unknown";
        /// bool result4 = text4.ToBool(); // 结果: false (默认值)
        /// 
        /// string text5 = "unknown";
        /// bool result5 = text5.ToBool(true); // 结果: true (自定义默认值)
        /// </code>
        /// </example>
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
        /// <summary>
        /// 重复字符串指定次数。
        /// </summary>
        /// <param name="value">要重复的字符串</param>
        /// <param name="repeatCount">重复次数</param>
        /// <returns>重复后的字符串，如果输入为空或重复次数小于等于0则返回空字符串</returns>
        /// <example>
        /// <code>
        /// string text = "Hello";
        /// string result = text.Repeat(3); // 结果: "HelloHelloHello"
        /// 
        /// string dash = "-";
        /// string line = dash.Repeat(10); // 结果: "----------"
        /// 
        /// string empty = "test".Repeat(0); // 结果: ""
        /// </code>
        /// </example>
        public static string Repeat(this string value, int repeatCount)
        {
            if (string.IsNullOrEmpty(value) || repeatCount <= 0) return string.Empty;
            if (repeatCount == 1) return value;

            var stringBuilder = new StringBuilder(value.Length * repeatCount);
            for (int i = 0; i < repeatCount; i++)
            {
                stringBuilder.Append(value);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 使用指定字符串填充字符串到指定长度。
        /// 如果字符串长度不足，则在前面或后面添加填充字符；如果长度超出，则返回原字符串。
        /// </summary>
        /// <param name="str">要填充的字符串</param>
        /// <param name="Length">目标长度</param>
        /// <param name="FillStr">用于填充的字符串</param>
        /// <param name="Append">是否在字符串后面填充，true 为后面，false 为前面</param>
        /// <returns>填充后的字符串</returns>
        /// <example>
        /// <code>
        /// string text = "Hello";
        /// string padded = text.Fill(10, "0", true); // 结果: "Hello00000"
        /// 
        /// string number = "123";
        /// string paddedNumber = number.Fill(6, "0", false); // 结果: "000123"
        /// </code>
        /// </example>
        public static string Fill(this string str, int Length, string FillStr, bool Append = true)
        {
            int clength = Length - str.Length;
            if (clength < 1) return str;

            var stringBuilder = new StringBuilder(Length);
            if (Append)
            {
                stringBuilder.Append(str);
                for (int i = 0; i < clength; i++)
                {
                    stringBuilder.Append(FillStr);
                }
            }
            else
            {
                for (int i = 0; i < clength; i++)
                {
                    stringBuilder.Append(FillStr);
                }
                stringBuilder.Append(str);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 截断过长的字符串，在中间插入省略符。
        /// 如果字符串长度超过指定长度，则保留前后部分并在中间插入省略符。
        /// </summary>
        /// <param name="value">要处理的字符串</param>
        /// <param name="maxlen">最大允许长度</param>
        /// <param name="str">省略符字符串，如 "..."</param>
        /// <returns>截断后的字符串，如果原字符串长度不超过 maxlen 则返回原字符串</returns>
        /// <example>
        /// <code>
        /// string longText = "这是一个很长的字符串，需要被截断";
        /// string truncated = longText.KeepLessStringLength(10, "..."); // 结果: "这是一个...截断"
        /// 
        /// string shortText = "短文本";
        /// string result = shortText.KeepLessStringLength(10, "..."); // 结果: "短文本" (未截断)
        /// </code>
        /// </example>
        public static string KeepLessStringLength(this string value, int maxlen, string str)
        {
            if (value.Length > maxlen)
            {
                int halflen = (maxlen - str.Length) / 2;
                var stringBuilder = new StringBuilder(maxlen);
                stringBuilder.Append(value.Substring(0, halflen));
                stringBuilder.Append(str);
                stringBuilder.Append(value.Substring(value.Length - halflen, halflen));
                return stringBuilder.ToString();
            }
            else return value;
        }
        /// <summary>
        /// 调整字符串长度，支持截断和填充操作。
        /// 如果字符串长度超出目标长度则截断，如果不足则填充。
        /// </summary>
        /// <param name="str">要调整的字符串</param>
        /// <param name="Length">目标长度</param>
        /// <param name="SubStringFromStart">截断时是否从开头截取，true 为从开头，false 为从末尾</param>
        /// <param name="PadChar">填充字符</param>
        /// <param name="Append">填充时是否在字符串后面填充，true 为后面，false 为前面</param>
        /// <returns>调整长度后的字符串</returns>
        /// <example>
        /// <code>
        /// string text = "Hello World";
        /// string adjusted = text.AdjustStringLength(5, true, '0', true); // 结果: "Hello" (截断)
        /// 
        /// string shortText = "Hi";
        /// string padded = shortText.AdjustStringLength(5, true, '0', true); // 结果: "Hi000" (填充)
        /// </code>
        /// </example>
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
                var stringBuilder = new StringBuilder(Length);
                var padding = new string(PadChar, Length - str.Length);
                if (Append)
                {
                    stringBuilder.Append(str);
                    stringBuilder.Append(padding);
                }
                else
                {
                    stringBuilder.Append(padding);
                    stringBuilder.Append(str);
                }
                str = stringBuilder.ToString();
            }
            return str;
        }
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
        public static string StripHtml(this string str) => HtmlTagRegex.Replace(str, string.Empty);
        public static bool HasLengthGreaterThanOrEqual(this string str, int length) => str?.Length >= length;

        /// <summary>
        /// 检查字符串长度是否大于等于指定长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        [Obsolete("方法名 'CheckStringLengthGte' 不够清晰，请使用 'HasLengthGreaterThanOrEqual' 方法", false)]
        public static bool CheckStringLengthGte(this string str, int length) => HasLengthGreaterThanOrEqual(str, length);


        /// <summary>
        /// 检查字符串是否为有效的电子邮件地址格式。
        /// </summary>
        /// <param name="Email">要检查的电子邮件地址字符串</param>
        /// <returns>如果字符串符合电子邮件地址格式则返回 true，否则返回 false</returns>
        /// <example>
        /// <code>
        /// string email1 = "user@example.com";
        /// bool result1 = email1.IsEmail(); // 结果: true
        /// 
        /// string email2 = "invalid-email";
        /// bool result2 = email2.IsEmail(); // 结果: false
        /// 
        /// string email3 = "test@domain.co.uk";
        /// bool result3 = email3.IsEmail(); // 结果: true
        /// </code>
        /// </example>
        public static bool IsEmail(this string Email)
        {
            if (string.IsNullOrEmpty(Email)) return false;
            return EmailRegex.IsMatch(Email);
        }
        /// <summary>
        /// 检查字符串是否为有效的中国大陆手机号码格式。
        /// 支持主流的手机号段：13x、14x、15x、16x、17x、18x、19x。
        /// </summary>
        /// <param name="MobilePhone">要检查的手机号码字符串</param>
        /// <returns>如果字符串符合中国大陆手机号码格式则返回 true，否则返回 false</returns>
        /// <example>
        /// <code>
        /// string phone1 = "13812345678";
        /// bool result1 = phone1.IsMobilePhone(); // 结果: true
        /// 
        /// string phone2 = "12345678901";
        /// bool result2 = phone2.IsMobilePhone(); // 结果: false (不是有效号段)
        /// 
        /// string phone3 = "15912345678";
        /// bool result3 = phone3.IsMobilePhone(); // 结果: true
        /// </code>
        /// </example>
        public static bool IsMobilePhone(this string MobilePhone)
        {
            if (string.IsNullOrEmpty(MobilePhone)) return false;
            return MobilePhoneRegex.IsMatch(MobilePhone);
        }
        /// <summary>
        /// 字符串是否是固定电话号码
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static bool IsPhone(this string Phone)
        {
            if (string.IsNullOrEmpty(Phone)) return false;
            return PhoneRegex.IsMatch(Phone);
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
            return BusinessLicenseRegex.IsMatch(BusinessLicense);
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
            return RequireHttps ? HttpsUrlRegex.IsMatch(Url) : UrlRegex.IsMatch(Url);
        }
        /// <summary>
        /// 检查字符串是否是IPv4地址
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIPv4Address(this string ipAddress)
        {
            //if (string.IsNullOrEmpty(ipAddress)) return false;
            //Regex regex = new Regex(@"^(\d+)\.(\d+)\.(\d+)\.(\d+)$");
            //if (regex.IsMatch(ipAddress))
            //{
            //    if (Regex.IsMatch(ipAddress, @"^0\.\d+\.0\.\d+$")) return false;
            //    string[] arr = ipAddress.Split('.');
            //    if (int.Parse(arr[0]) < 256 && int.Parse(arr[1]) < 256 && int.Parse(arr[2]) < 256 && int.Parse(arr[3]) < 256) return true;
            //}
            //return false;
            if (ipAddress.IsNullOrEmpty()) return false;
            var result = IPAddress.TryParse(ipAddress, out var address);
            return result && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
        }
        /// <summary>
        /// 检查字符串是否是IPv6地址
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIPv6Address(this string ipAddress)
        {
            //if (string.IsNullOrEmpty(IP)) return false;
            //Regex regex = new Regex(@"^([\da-fA-F]{1,4}:){7}[\da-fA-F]{1,4}$");
            //return regex.IsMatch(IP);
            if (ipAddress.IsNullOrEmpty()) return false;
            ipAddress = ipAddress.TrimStart('[').TrimEnd(']');
            var result = IPAddress.TryParse(ipAddress, out var address);
            return result && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
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
                var stringBuilder = new StringBuilder(numberChars + 1);
                stringBuilder.Append('0');
                stringBuilder.Append(hex);
                hex = stringBuilder.ToString();
                numberChars = hex.Length;
            }
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return true.ToStateSet(bytes);
        }
        /// <summary>
        /// 把十六进制字符串转换成 <see cref="BigInteger"/>。
        /// </summary>
        /// <param name="hex">可以带或不带 0x / 0X 前缀，允许大小写混写。</param>
        /// <param name="signed">
        /// true  (默认) ➜ 按 **二补码** 解析，最高位是 1 时得到负数；  
        /// false ➜ 把整个十六进制串视作 **无符号正数**（即使最高位是 1 也保证结果为正）。
        /// </param>
        /// <exception cref="ArgumentNullException">hex 为 null/空/全空白。</exception>
        /// <remarks>
        /// - .NET 5+ 直接使用 <c>Convert.FromHexString</c> + <c>new BigInteger(span, isUnsigned: …)</c>，  
        ///   全路径向量化实现，性能最佳。  
        /// - 如果只能在早于 .NET 5 的环境运行，会自动 fallback 到“补 00 前缀”技巧，不影响调用代码。
        /// </remarks>
        public static BigInteger HexStringToBigInteger(this string hex, bool signed = true)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentNullException(nameof(hex));

            // 去掉 0x / 0X 前缀
            if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                hex = hex.Substring(2);

#if NET5_0_OR_GREATER
        if (!signed)
        {
            // —— 高性能无符号路径 ——
            // 1. 如果字符数为奇数，先补一个 0；Convert.FromHexString 需要偶数字符数
            if ((hex.Length & 1) == 1)
                hex = '0' + hex;

            // 2. 十六进制 → byte[]（大端）—— Convert.FromHexString 是硬件加速的
            byte[] bytes = Convert.FromHexString(hex);

            // 3. BigInteger 构造函数：isUnsigned = true 保证正数，isBigEndian = true 直接省去反转
            return new BigInteger(bytes, /* isUnsigned */ true, /* isBigEndian */ true);  // :contentReference[oaicite:0]{index=0}
        }
#endif
            // —— 有符号路径，或老版本框架 fallback —— 
            // NumberStyles.HexNumber = AllowHexSpecifier + Leading/TrailingWhite
            BigInteger result = BigInteger.Parse(hex, NumberStyles.HexNumber);

#if !NET5_0_OR_GREATER
            if (!signed && result.Sign < 0)
            {
                // 老框架又要求无符号？用补位法：+ 2^(4*digitCount)
                // digitCount = 去掉前导 0 后的位数
                int bitCount = 4 * hex.TrimStart('0').Length;
                result += BigInteger.One << bitCount;
            }
#endif
            return result;
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
        #region Obsolete Methods - 兼容性保留，将在未来版本中删除

        /// <summary>
        /// 重复字符串指定次数
        /// </summary>
        /// <param name="value">要重复的字符串</param>
        /// <param name="repeateTimes">重复次数（已废弃，请使用 repeatCount 参数的新版本方法）</param>
        /// <returns></returns>
        [Obsolete("方法名 'RepeatString' 不够清晰，请使用 'Repeat' 方法", false)]
        public static string RepeatString(this string value, int repeateTimes) => Repeat(value, repeateTimes);

        /// <summary>
        /// 获取字符串的显示长度（考虑中文字符占两个位置）
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        [Obsolete("方法名 'GetLengthSpecial' 不够清晰，请使用 'GetDisplayLength' 方法", false)]
        public static int GetLengthSpecial(this string s) => GetDisplayLength(s);

        /// <summary>
        /// 按显示长度截取字符串（考虑中文字符占两个位置）
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="length">显示长度</param>
        /// <returns></returns>
        [Obsolete("方法名 'SubstringSpecial' 不够清晰，请使用 'SubstringByDisplayLength' 方法", false)]
        public static string SubstringSpecial(this string s, int startIndex, int length) => SubstringByDisplayLength(s, startIndex, length);

        #endregion

        public enum Strength
        {
            Invalid = 0,
            Weak = 1,
            Normal = 2,
            Strong = 3,
        };
    }
}
