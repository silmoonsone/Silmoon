using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Silmoon.Extension
{
    /// <summary>
    /// 字符串辅助工具类，提供字符串和数组相关的实用方法。
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 空字符串常量，用于替代 string.Empty 以提高可读性。
        /// </summary>
        public const string EmptyString = "";

        [Obsolete("请使用Array.Join方法")]
        public static string MergeStringArray(Array array, string SplitString, bool RemoveLastSplitString = true, string perfixString = EmptyString, string suffixString = EmptyString) => array.Join(SplitString, RemoveLastSplitString, perfixString, suffixString);

        /// <summary>
        /// 从字符串数组中解析出键值对集合。
        /// 支持指定分隔符和前缀过滤。
        /// </summary>
        /// <param name="array">要解析的字符串数组</param>
        /// <param name="nameValueSeparator">键值之间的分隔符，默认为冒号</param>
        /// <param name="prefixString">键名前缀，只有以此前缀开头的项才会被解析</param>
        /// <returns>解析后的键值对集合</returns>
        /// <example>
        /// <code>
        /// string[] config = {"name:John", "age:25", "city:New York", "comment:This is a comment"};
        /// var nameValues = config.ParseNameValuePairs(); // 解析所有项
        /// // 结果: name=John, age=25, city=New York, comment=This is a comment
        /// 
        /// string[] settings = {"#name:John", "#age:25", "temp:data"};
        /// var filtered = settings.ParseNameValuePairs(":", "#"); // 只解析以#开头的项
        /// // 结果: name=John, age=25
        /// </code>
        /// </example>
        public static NameValueCollection ParseNameValuePairs(this string[] array, string nameValueSeparator = ":", string prefixString = EmptyString)
        {
            NameValueCollection result = new NameValueCollection();
            if (array.IsNullOrEmpty()) return result;

            foreach (string item in array)
            {
                if (string.IsNullOrEmpty(prefixString) || item.StartsWith(prefixString))
                {
                    string[] parts = item.Split(new string[] { nameValueSeparator }, 2, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        result.Add(parts[0], parts[1]);
                    }
                    else if (parts.Length == 1)
                    {
                        result.Add(parts[0], null);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 找出第一个数组中在第二个数组中缺失的项目。
        /// 使用 HashSet 优化性能，避免 O(n²) 的嵌套循环。
        /// </summary>
        /// <param name="sourceArray">源数组（作为比对的范例）</param>
        /// <param name="targetArray">目标数组（需要检查的数组）</param>
        /// <returns>在目标数组中缺失的项目数组</returns>
        /// <example>
        /// <code>
        /// string[] source = {"apple", "banana", "cherry", "date"};
        /// string[] target = {"banana", "date"};
        /// string[] missing = StringHelper.FindMissingItems(source, target); // 结果: {"apple", "cherry"}
        /// 
        /// string[] numbers1 = {"1", "2", "3", "4", "5"};
        /// string[] numbers2 = {"2", "4"};
        /// string[] missingNumbers = StringHelper.FindMissingItems(numbers1, numbers2); // 结果: {"1", "3", "5"}
        /// </code>
        /// </example>
        public static string[] FindMissingItems(string[] sourceArray, string[] targetArray)
        {
            if (sourceArray == null || sourceArray.Length == 0) return new string[0];
            if (targetArray == null || targetArray.Length == 0) return sourceArray;

            var targetSet = new HashSet<string>(targetArray);
            var result = new List<string>(sourceArray.Length);

            foreach (string item in sourceArray)
            {
                if (!targetSet.Contains(item))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        #region Obsolete Methods - 兼容性保留，将在未来版本中删除

        /// <summary>
        /// 从数组中分析出名字与值的集合
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="nameValueSeparator">名字与值之间的分隔符</param>
        /// <param name="perNameChar">名字前缀</param>
        /// <returns></returns>
        [Obsolete("方法名 'AnalyzeNameValue' 不够清晰，请使用 'ParseNameValuePairs' 方法", false)]
        public static NameValueCollection AnalyzeNameValue(this string[] array, string nameValueSeparator = ":", string perNameChar = EmptyString) => ParseNameValuePairs(array, nameValueSeparator, perNameChar);

        /// <summary>
        /// 根据第一个数组，找出第二个数组缺少的项目
        /// </summary>
        /// <param name="array1">作为比对的数组的范例</param>
        /// <param name="array2">需要找出缺少项目的数组</param>
        /// <returns></returns>
        [Obsolete("方法名 'MissedItems' 不够清晰，请使用 'FindMissingItems' 方法", false)]
        public static string[] MissedItems(string[] array1, string[] array2) => FindMissingItems(array1, array2);

        #endregion
    }
}
