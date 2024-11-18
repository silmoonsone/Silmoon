using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Silmoon.Extension;

namespace Silmoon
{
    public static class StringHelper
    {
        public const string EmptyString = "";
        /// <summary>
        /// 把数组中的所有元素作为字符串使用一个指定的分隔符合并。
        /// </summary>
        /// <param name="array">数组，其中元素会作为字符串使用</param>
        /// <param name="perfixString">每个元素的前缀</param>
        /// <param name="suffixString">每个元素的后缀</param>
        /// <param name="SplitString">分隔符</param>
        /// <param name="RemoveLastSplitString">是否移除最后一个分隔符</param>
        /// <returns></returns>
        public static string MergeStringArray(Array array, string SplitString, bool RemoveLastSplitString = true, string perfixString = EmptyString, string suffixString = EmptyString)
        {
            string result = string.Empty;
            if (array == null || array.Length == 0) return result;
            foreach (object s in array)
                result += perfixString + s + suffixString + SplitString;
            if (RemoveLastSplitString)
                result = result.Substring(0, result.Length - SplitString.Length);
            return result;
        }
        /// <summary>
        /// 从数组中分析出名字与值的集合
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="nameValueSeparator">名字与值之间的分隔符</param>
        /// <param name="perNameChar">名字前缀</param>
        /// <returns></returns>
        public static NameValueCollection AnalyzeNameValue(string[] array, string nameValueSeparator = ":", string perNameChar = EmptyString)
        {
            NameValueCollection result = new NameValueCollection();
            if (array.IsNullOrEmpty()) return result;
            foreach (string s1 in array)
            {
                if (s1.StartsWith(perNameChar))
                {
                    string[] sArr = s1.Split(new string[] { nameValueSeparator }, 2, StringSplitOptions.None);
                    if (sArr.Length == 2) result.Add(sArr[0], sArr[1]);
                    else result.Add(sArr[0], null);
                }
            }

            return result;
        }
        /// <summary>
        /// 从数组中取出重复的字符串，组合成新的数组。
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] RemoveRepeaterString(string[] array)
        {
            List<string> listString = new List<string>();
            foreach (string eachString in array)
            {
                if (!listString.Contains(eachString)) listString.Add(eachString);
            }
            return listString.ToArray();
        }
        /// <summary>
        /// 根据第一个数组，找出第二个数组缺少的项目
        /// </summary>
        /// <param name="array1">作为比对的数组的范例</param>
        /// <param name="array2">需要找出缺少项目的数组</param>
        /// <returns></returns>
        public static string[] MissedItems(string[] array1, string[] array2)
        {
            //ComparisonArrayed result = new ComparisonArrayed();
            List<string> result = new List<string>();

            foreach (string item in array1)
            {
                bool itemFound = false;
                foreach (string item1 in array2)
                {
                    itemFound = item == item1;
                    if (itemFound) break;
                }
                if (!itemFound) result.Add(item);
            }
            return result.ToArray();
        }
    }
}
