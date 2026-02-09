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
        /// <summary>ANSI 转义序列（CSI 与 OSC），用于 StripConsoleStyle</summary>
        private static readonly Regex AnsiEscapeRegex = new Regex(@"\x1b(?:\[[\x30-\x3f]*[\x20-\x2f]*[\x40-\x7e]|\][^\x07]*(?:\x07|\x1b\\))?", RegexOptions.Compiled);

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
            foreach (string s in array)
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
        /// 获取文本经过指定编码后的字节长度。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static int GetEncodingByteCount(this string value, Encoding encoding) => encoding.GetByteCount(value);
        /// <summary>
        /// 获取字符串按照实际显示出来的占用文本空间的宽度（列数）。如英文占用1宽度，中文占用2宽度。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetDisplayWidth(this string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;

            var e = StringInfo.GetTextElementEnumerator(value);
            int width = 0;

            while (e.MoveNext())
            {
                width += GetElementWidth(e.GetTextElement());
            }

            return width;
        }
        /// <summary>
        /// 按照实际显示出来的占用文本空间的宽度（列数）截取字符串片段，如英文占用1，中文占用2的宽度进行截取。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns>返回截取的字符串片段</returns>
        public static string SubstringDisplayWidth(this string value, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (length <= 0) return string.Empty;
            if (startIndex < 0) startIndex = 0;

            var e = StringInfo.GetTextElementEnumerator(value);
            var stringBuilder = new StringBuilder();

            int pos = 0;      // 已走过的显示列数
            int taken = 0;    // 已截取的显示列数

            while (e.MoveNext())
            {
                string te = e.GetTextElement();
                int w = GetElementWidth(te);

                int nextPos = pos + w;

                // 关键点：只要“当前元素起点 < startIndex”，就跳过（即 startIndex 落在元素内部也会跳过它）
                if (pos < startIndex)
                {
                    pos = nextPos;
                    continue;
                }

                if (taken + w > length) break;

                stringBuilder.Append(te);
                taken += w;
                pos = nextPos;
            }

            return stringBuilder.ToString();
        }

        #region 辅助方法：计算文本元素宽度
        /// <summary>
        /// 计算一个“文本元素”(text element)的显示宽度（列数）。
        /// </summary>
        /// <remarks>
        /// - 控制字符 / 组合附加符号 / VS / ZWJ 等视为 0 列；
        /// - 普通字符视为 1 列；
        /// - CJK/全角/部分 Emoji 视为 2 列（近似规则，和终端/字体可能存在差异）。
        /// - 若一个 textElement 全由零宽码点组成，为避免外层循环不前进，返回 1 作为兜底。
        /// </remarks>
        private static int GetElementWidth(string textElement)
        {
            if (string.IsNullOrEmpty(textElement)) return 0;

#if NET5_0_OR_GREATER
            int width = 0;

            foreach (var rune in textElement.EnumerateRunes())
            {
                int cp = rune.Value;

                // 控制字符：通常按 0（你也可以按 1，视场景）
                if (cp <= 0x1F || (cp >= 0x7F && cp <= 0x9F)) continue;

                // 变体选择符 / ZWJ / 组合附加符号：零宽
                if (IsZeroWidth(cp)) continue;

                width = Math.Max(width, IsWide(cp) ? 2 : 1);
                if (width == 2) break; // 我们模型里最大就是 2
            }

            // 防止出现“全是零宽码点”的 textElement 导致宽度为 0 -> 外层循环 pos 不前进
            return width == 0 ? 1 : width;
#else
            // 老框架没有 Rune：按 UTF-16 遍历 textElement 的码点（代理项合并）
            int width = 0;

            for (int i = 0; i < textElement.Length; i++)
            {
                int cp;
                // 合并 surrogate pair
                if (char.IsHighSurrogate(textElement[i]) && i + 1 < textElement.Length && char.IsLowSurrogate(textElement[i + 1]))
                {
                    cp = char.ConvertToUtf32(textElement[i], textElement[i + 1]);
                    i++; // 消耗掉低代理项
                }
                else cp = textElement[i];

                // 控制字符：按 0 列
                if (cp <= 0x1F || (cp >= 0x7F && cp <= 0x9F)) continue;

                // 变体选择符 / ZWJ / 组合附加符号：零宽
                if (IsZeroWidth(cp)) continue;

                width = Math.Max(width, IsWide(cp) ? 2 : 1);
                if (width == 2) break;
            }

            // 防止全是零宽导致宽度=0 -> 外层 pos 不前进
            return width == 0 ? 1 : width;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsZeroWidth(int cp)
        {
            // Combining marks（常见组合附加符号范围）
            if ((cp >= 0x0300 && cp <= 0x036F) ||
                (cp >= 0x1AB0 && cp <= 0x1AFF) ||
                (cp >= 0x1DC0 && cp <= 0x1DFF) ||
                (cp >= 0x20D0 && cp <= 0x20FF) ||
                (cp >= 0xFE20 && cp <= 0xFE2F))
                return true;

            // Variation Selectors
            if ((cp >= 0xFE00 && cp <= 0xFE0F) || (cp >= 0xE0100 && cp <= 0xE01EF))
                return true;

            // ZWJ / ZWNJ
            if (cp == 0x200D || cp == 0x200C)
                return true;

            return false;
        }

        /// <summary>
        /// 判断码点在等宽终端中是否更可能显示为 2 列（近似规则，非绝对）。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsWide(int cp)
        {
            // 近似 wcwidth 的“宽字符”范围（更接近等宽终端效果）
            if (cp >= 0x1100 && cp <= 0x115F) return true;           // Hangul Jamo init
            if (cp == 0x2329 || cp == 0x232A) return true;
            if (cp >= 0x2E80 && cp <= 0xA4CF) return true;           // CJK / Yi / 等
            if (cp >= 0xAC00 && cp <= 0xD7A3) return true;           // Hangul syllables
            if (cp >= 0xF900 && cp <= 0xFAFF) return true;           // CJK compatibility ideographs
            if (cp >= 0xFE10 && cp <= 0xFE19) return true;
            if (cp >= 0xFE30 && cp <= 0xFE6F) return true;
            if (cp >= 0xFF00 && cp <= 0xFF60) return true;           // Fullwidth forms
            if (cp >= 0xFFE0 && cp <= 0xFFE6) return true;

            // CJK 扩展区（超出 BMP）
            if (cp >= 0x20000 && cp <= 0x2FFFD) return true;
            if (cp >= 0x30000 && cp <= 0x3FFFD) return true;

            // Emoji（实际显示很多环境按 2 列更贴近）
            if (cp >= 0x1F000 && cp <= 0x1FAFF) return true;
            if (cp >= 0x2600 && cp <= 0x27BF) return true;           // 常见符号/ dingbats（不少终端按 2）

            return false;
        }
        #endregion




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
        public static bool IsDecimal(this string value) => decimal.TryParse(value, out decimal result);

        /// <summary>
        /// 按指定 <paramref name="encoding"/> 的“字节长度上限”截取字符串片段（从 <paramref name="startIndex"/> 开始），
        /// 并保证不会在多字节字符（如 UTF-8 中文、Emoji 等）中间截断，避免产生乱码或替换字符（�）。
        /// </summary>
        /// <param name="s">要截取的字符串。</param>
        /// <param name="startIndex">
        /// 起始位置（按 .NET 字符索引，即 UTF-16 的 char 索引；不是字节索引）。
        /// </param>
        /// <param name="maxBytes">
        /// 截取后的内容在 <paramref name="encoding"/> 编码下的最大字节数（byte count 上限）。
        /// </param>
        /// <param name="encoding">用于计算字节长度与解码的编码（如 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>
        /// 从 <paramref name="startIndex"/> 开始截取的字符串片段，满足：
        /// 1) 其在 <paramref name="encoding"/> 下编码后的字节长度 &lt;= <paramref name="maxBytes"/>；
        /// 2) 不会破坏字符边界（不会截断一个字符的编码字节序列）。
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="string.Substring(int,int)"/> 按“字符个数（UTF-16 char）”截取，并不关心 UTF-8 等编码后的字节长度；
        /// 当你需要“字段/协议/存储”按字节限长时（例如最多 N 字节），应使用本方法。
        /// </para>
        /// <para>
        /// 注意：<paramref name="startIndex"/> 是字符索引，不是字节索引。若你手头只有“字节偏移”，需要另行转换。
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// // UTF-8 下：汉字通常 3 字节；ASCII 1 字节
        /// string text = "你好世界"; // 4 个 char，UTF-8 共 12 字节
        ///
        /// // Substring 按字符截取：从 0 开始取 3 个字符 => "你好世"
        /// // 但它的 UTF-8 字节长度是 9（并不会自动限制到 3 字节）
        /// string a = text.Substring(0, 3);                     // "你好世"
        ///
        /// // SubstringEncoded 按 UTF-8 字节截取：最多 3 字节 => 只能容纳 1 个汉字
        /// string b = text.SubstringEncoded(0, 3, Encoding.UTF8); // "你"
        ///
        /// // 带 startIndex：从 "世界" 开始最多 3 字节 => "世"
        /// string c = text.SubstringEncoded(2, 3, Encoding.UTF8); // "世"
        ///
        /// // 混合更直观：
        /// string mixed = "A你B"; // UTF-8 字节：A(1) + 你(3) + B(1) = 5
        /// string d1 = mixed.Substring(0, 2);                      // "A你"（字符截取）UTF-8=4字节
        /// string d2 = mixed.SubstringEncoded(0, 2, Encoding.UTF8); // "A"（2字节只够放 A）
        /// </code>
        /// </example>
        public static string SubstringEncoded(this string s, int startIndex, int maxBytes, Encoding encoding)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (startIndex == s.Length) return string.Empty;
            if (startIndex > s.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (maxBytes <= 0) return string.Empty;
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            // 仅对需要截取的尾部片段进行编码（避免整串 GetBytes 的大分配）
            string tail = (startIndex == 0) ? s : s.Substring(startIndex);

            byte[] bytes = encoding.GetBytes(tail);
            if (bytes.Length <= maxBytes) return tail;

            Decoder decoder = encoding.GetDecoder();

            // 输出字符数不可能超过输入字节数，因此用 maxBytes 作为 char 缓冲上界（偏保守但简单可靠）。
            char[] chars = new char[maxBytes];

            decoder.Convert(bytes, 0, maxBytes, chars, 0, chars.Length, flush: false, out int _, out int charsUsed, out bool _);

            return new string(chars, 0, charsUsed);
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
            if (groupSize <= 0) throw new ArgumentOutOfRangeException(nameof(groupSize));

            // 去除输入字符串中可能存在的分隔符
            //input = input.Replace(separator, "");

            var stringBuilder = new StringBuilder(input.Length + (input.Length / groupSize) * separator.Length);
            ReadOnlySpan<char> span = input.AsSpan();

            for (int i = 0; i < span.Length; i += groupSize)
            {
                int length = Math.Min(groupSize, span.Length - i);

#if NETCOREAPP2_1_OR_GREATER || NET5_0_OR_GREATER
                stringBuilder.Append(span.Slice(i, length));
#else
                stringBuilder.Append(input, i, length);
#endif

                if (i + length < span.Length) stringBuilder.Append(separator);
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
            catch (Exception)
            {
                if (throwException) throw;  // 使用 throw 而不是 throw ex，以保留原始堆栈跟踪
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
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (replacement == null) throw new ArgumentNullException(nameof(replacement));
            if (index < 0 || index >= value.Length || index + length > value.Length || length < 0) throw new ArgumentOutOfRangeException("参数start或length的值不合法");

            // 生成用于替换的字符串
            string replaceWith = replacement.Length > 0 ? new string(replacement[0], length) : string.Empty;

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
                // 优化：一次性追加重复的填充字符串，而不是循环追加
                if (FillStr.Length == 1)
                    stringBuilder.Append(FillStr[0], clength);
                else
                {
                    for (int i = 0; i < clength; i++)
                    {
                        stringBuilder.Append(FillStr);
                    }
                }
            }
            else
            {
                // 优化：一次性追加重复的填充字符串，而不是循环追加
                if (FillStr.Length == 1)
                    stringBuilder.Append(FillStr[0], clength);
                else
                {
                    for (int i = 0; i < clength; i++)
                    {
                        stringBuilder.Append(FillStr);
                    }
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

        /// <summary>
        /// 去掉字符串中的控制台样式（ANSI 转义序列，如 \x1b[31m、\x1b[38;2;r;g;b;m 等）。
        /// 适用于将带样式的控制台输出写入日志文件前脱去样式，避免日志中出现乱码或控制字符。
        /// </summary>
        /// <param name="str">可能包含 ANSI 转义序列的字符串</param>
        /// <returns>移除所有 ANSI 转义序列后的纯文本；若 <paramref name="str"/> 为 null 则返回 null</returns>
        /// <example>
        /// <code>
        /// string styled = "\x1b[31mError\x1b[0m: file not found";
        /// string plain = styled.StripConsoleStyle(); // "Error: file not found"
        /// string line = Console.ReadLine(); // 若控制台输出带样式，写入日志前脱样式
        /// File.AppendAllText("app.log", line.StripConsoleStyle() + "\n");
        /// </code>
        /// </example>
        public static string StripConsoleStyle(this string str)
        {
            if (str.IsNullOrEmpty()) return null;
            return AnsiEscapeRegex.Replace(str, string.Empty);
        }

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
        // 身份证校验码对照表（符合GB11643-1999标准）
        private static readonly string[] CardIdVerifyCodes = { "1", "0", "x", "9", "8", "7", "6", "5", "4", "3", "2" };
        // 身份证加权因子
        private static readonly int[] CardIdWeights = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        // 身份证省份代码（前两位）
        private static readonly string CardIdProvinceCodes = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

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
            string provinceCode = CardId.Substring(0, 2);
            if (!CardIdProvinceCodes.Contains(provinceCode)) return false;

            //生日验证 - 身份证第7-14位是出生日期，格式YYYYMMDD
            string birthYear = CardId.Substring(6, 4);
            string birthMonth = CardId.Substring(10, 2);
            string birthDay = CardId.Substring(12, 2);
            string birth = $"{birthYear}-{birthMonth}-{birthDay}";
            if (!DateTime.TryParse(birth, out DateTime _)) return false;

            //校验码验证
            ReadOnlySpan<char> cardIdSpan = CardId.AsSpan(0, 17);
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                if (!char.IsDigit(cardIdSpan[i])) return false;
                sum += CardIdWeights[i] * (cardIdSpan[i] - '0');
            }
            int remainder = sum % 11;
            char expectedCheckCode = CardIdVerifyCodes[remainder][0];
            char actualCheckCode = char.ToLowerInvariant(CardId[17]);
            if (expectedCheckCode != actualCheckCode) return false;
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



        public static string AppendUriQueryString(this string url, string queryString) => url.Contains("?") ? $"{url}&{queryString}" : $"{url}?{queryString}";
        public static string AppendUriQueryString(this string url, string key, string value) => url.Contains("?") ? $"{url}&{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}" : $"{url}?{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}";

        public static string RemoveUriQueryString(this string url, string key)
        {
            if (string.IsNullOrEmpty(url) || !url.Contains("?") || string.IsNullOrEmpty(key)) return url;

            var baseUrl = url.Substring(0, url.IndexOf("?"));
            var queryString = url.Substring(url.IndexOf("?") + 1);

            var queryParams = HttpUtility.ParseQueryString(queryString);
            queryParams.Remove(key);

            var newQueryString = string.Empty;
            if (queryParams.HasKeys())
            {
                newQueryString = string.Join("&", queryParams.AllKeys.Select(k => $"{HttpUtility.UrlEncode(k)}={HttpUtility.UrlEncode(queryParams[k])}"));
                return $"{baseUrl}?{newQueryString}";
            }
            else return baseUrl;

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
        public static byte[] GetBytes(this string str, Encoding encoding = null) => (encoding ?? Encoding.UTF8).GetBytes(str);

        public static bool IsHexString(this string value) => !string.IsNullOrEmpty(value) && IsHexString(value.AsSpan());
        public static bool IsHexString(this ReadOnlySpan<char> span)
        {
            // 支持 "0x" 和 "0X" 前缀
            if (span.Length >= 2 && span[0] == '0' && (span[1] == 'x' || span[1] == 'X')) span = span.Slice(2);
            if (span.Length == 0) return false;

            foreach (var c in span)
            {
                bool isHex = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
                if (!isHex) return false;
            }
            return true;
        }
        public static StateSet<bool, byte[]> HexStringToByteArray(this ReadOnlySpan<char> hexSpan)
        {
            // 重用 IsHexString 方法进行校验
            if (!IsHexString(hexSpan)) return false.ToStateSet<byte[]>(null, "hex string value not is HexString.");

            // 去掉 "0x" 或 "0X" 前缀（IsHexString 已经验证过格式，这里安全处理）
            if (hexSpan.Length >= 2 && hexSpan[0] == '0' && (hexSpan[1] == 'x' || hexSpan[1] == 'X')) hexSpan = hexSpan.Slice(2);

            // 计算字节数组长度：奇数长度需要向上取整
            int numberChars = hexSpan.Length;
            int byteCount = (numberChars + 1) / 2;  // 对于奇数长度，正确计算：3->2, 5->3
            byte[] byteArrayResult = new byte[byteCount];

            int byteIndex = 0;
            int charIndex = 0;

            // 如果是奇数长度，第一个字节只处理一个字符（高位补0）
            if (numberChars % 2 != 0) byteArrayResult[byteIndex++] = (byte)GetHexValue(hexSpan[charIndex++]);

            // 处理剩余的字符对
            while (charIndex < numberChars)
            {
                byte high = (byte)GetHexValue(hexSpan[charIndex++]);
                byte low = (byte)GetHexValue(hexSpan[charIndex++]);
                byteArrayResult[byteIndex++] = (byte)((high << 4) | low);
            }
            return true.ToStateSet(byteArrayResult);
        }
        public static StateSet<bool, byte[]> HexStringToByteArray(this string hex)
        {
            if (hex is null) return false.ToStateSet<byte[]>(null, "hex string value is null");
            return HexStringToByteArray(hex.AsSpan());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHexValue(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            if (c >= 'a' && c <= 'f') return c - 'a' + 10;
            throw new ArgumentException($"Invalid hex character: {c}");
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
        [Obsolete("方法名 'GetLengthSpecial' 不够清晰，请使用 'GetDisplayWidth' 方法", false)]
        public static int GetLengthSpecial(this string s) => GetDisplayWidth(s);

        /// <summary>
        /// 按显示长度截取字符串（考虑中文字符占两个位置）
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="length">显示长度</param>
        /// <returns></returns>
        [Obsolete("方法名 'SubstringSpecial' 不够清晰，请使用 'SubstringByDisplayLength' 方法", false)]
        public static string SubstringSpecial(this string s, int startIndex, int length) => SubstringDisplayWidth(s, startIndex, length);

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
