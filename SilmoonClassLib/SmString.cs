using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace Silmoon
{
    public class ComparisonArrayed
    {
        private static string[] missed;
        private static string[] excessed;

        public static string[] Missed
        {
            get { return ComparisonArrayed.missed; }
            set { ComparisonArrayed.missed = value; }
        }
        public static string[] Excessed
        {
            get { return ComparisonArrayed.excessed; }
            set { ComparisonArrayed.excessed = value; }
        }
    }
    public class SmString
    {
        /// <summary>
        /// 剪裁字符串
        /// </summary>
        /// <param name="s">要处理的字符串</param>
        /// <param name="c">保留长度</param>
        /// <returns></returns>
        public static string CutString(string s, int c)
        {
            if (s.Length > c) { s = s.Substring(0, c); }
            return s;
        }
        /// <summary>
        /// 剪裁字符串，并对多出的字符做处理。
        /// </summary>
        /// <param name="s">要处理的字符串</param>
        /// <param name="c">保留长度</param>
        /// <param name="output">多出来的字符替换成的字符</param>
        /// <returns></returns>
        public static string CutString(string s, int c, string output)
        {
            if (s.Length > c) { s = s.Substring(0, c) + output; }
            return s;
        }
        /// <summary>
        /// 检查字段是否为空，如果不是，返回字段，否则抛出异常。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CheckEmpty(string s)
        {
            if (s.Length < 1)
            {
                throw new Exception("参数s为空！");
            }
            return s;
        }
        /// <summary>
        /// 如果字符串为NULL，则把字符串变成零长度的字符串。
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static string FixNullString(object s)
        {
            return FixNullString(s.ToString());
        }
        /// <summary>
        /// 如果字符串为NULL，则把字符串变成零长度的字符串。
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="onNullReturn">当字符串为NULL返回字符串</param>
        /// <returns></returns>
        public static string FixNullString(string s, string onNullReturn = "")
        {
            if (s == null) return onNullReturn;
            else return s.ToString();
        }
        /// <summary>
        /// 将一个字符串转化成BOOL类型
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns></returns>
        public static bool StringToBool(string s)
        {
            switch (s.ToLower())
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
                    return false;
            }
        }
        /// <summary>
        /// 在一个字符串数组里面寻找一个字符串
        /// </summary>
        /// <param name="sArr">字符串数组</param>
        /// <param name="findString">字符串</param>
        /// <returns></returns>
        public static bool FindFormStringArray(string[] sArr, string findString)
        {
            foreach (string s in sArr)
            {
                if (s == findString)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 保持一个字符串的长度
        /// </summary>
        /// <param name="s">原字符串</param>
        /// <param name="maxlen">最大长度</param>
        /// <param name="str">衔接字符串</param>
        /// <returns></returns>
        public static string KeepStringLenght(string s, int maxlen, string str)
        {
            if (s.Length > maxlen)
            {
                int halflen = (maxlen - str.Length) / 2;
                string result = s.Substring(0, halflen);
                result += str + s.Substring(s.Length - halflen, halflen);
                return result;
            }
            else return s;
        }
        /// <summary>
        /// 从一个字符串中获取一个指定索引号的元素的字符串。
        /// </summary>
        /// <param name="array">字符串数组</param>
        /// <param name="index">索引号</param>
        /// <param name="outIndexReturnNull">如果超出索引，是否返回null</param>
        /// <returns></returns>
        public static string FormArrayGetString(string[] array, int index, bool outIndexReturnNull = false)
        {
            if (array.Length < (index + 1))
            {
                if (outIndexReturnNull) return null; else return "";
            }
            else return array[index];
        }
        /// <summary>
        /// 把字符串数组中的所有字符全部合并。
        /// </summary>
        /// <param name="array">字符串数组。</param>
        /// <param name="perfixString">每个元素的前缀</param>
        /// <param name="suffixString">每个元素的后缀</param>
        /// <param name="SplitString">分隔符</param>
        /// <param name="RemoveLastSplitString">是否移除最后一个分隔符</param>
        /// <returns></returns>
        public static string MergeStringArray(string[] array, string SplitString, bool RemoveLastSplitString = true, string perfixString = "", string suffixString = "")
        {
            string result = "";
            if (array == null || array.Length == 0) return result;
            foreach (string s in array)
                result += perfixString + s + suffixString + SplitString;
            if (RemoveLastSplitString)
                result = result.Substring(0, result.Length - SplitString.Length);
            return result;
        }
        /// <summary>
        /// 对ArrayList进行排序
        /// </summary>
        /// <param name="array">以排序的ArrayList</param>
        /// <returns></returns>
        public static ArrayList SortArray(ArrayList array)
        {
            //Comparer c = new Comparer(System.Globalization.CultureInfo.CurrentCulture);
            array.Sort();
            return array;
        }
        /// <summary>
        /// 从数组中分析出名字与值的集合
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="nameValueSeparator">名字与值之间的分隔符</param>
        /// <param name="perNameChar">名字前缀</param>
        /// <returns></returns>
        public static NameValueCollection AnalyzeNameValue(string[] array, string nameValueSeparator = ":", string perNameChar = "")
        {
            NameValueCollection result = new NameValueCollection();
            if (array == null || array.Length == 0) return result;
            foreach (string s1 in array)
            {
                //if (perNameChar != "")
                //    if (s1.Length > perNameChar.Length)
                //        if (s1.Substring(0, perNameChar.Length) != perNameChar)

                if (perNameChar != "" && s1.Length > perNameChar.Length && s1.Substring(0, perNameChar.Length) != perNameChar)
                    continue;
                string[] sArr = s1.Split(new string[] { nameValueSeparator }, 2, StringSplitOptions.None);
                if (sArr.Length == 2)
                    result.Add(sArr[0], sArr[1]);
            }

            return result;
        }
        /// <summary>
        /// 填充或者保持字符串长度
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="length">长度</param>
        /// <param name="fillStr">补充字符串</param>
        /// <param name="onAfter">是否在后面补充</param>
        /// <returns></returns>
        public static string FillLength(string s, int length, string fillStr, bool onAfter)
        {
            int fInChrC = length - s.Length;
            if (fInChrC < 1) return s;

            for (int i = 0; i < fInChrC; i++)
            {
                if (onAfter)
                    s += fillStr;
                else s = fillStr + s;
            }
            return s;
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
                if (!listString.Contains(eachString))
                    listString.Add(eachString);
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
            ArrayList list2 = new ArrayList();

            foreach (string item in array1)
            {
                bool itemFound = false;
                foreach (string item1 in array2)
                {
                    itemFound = (item == item1);
                    if (itemFound) break;
                }
                if (!itemFound)
                    list2.Add(item);
            }
            return (string[])list2.ToArray(typeof(string));
        }
        /// <summary>
        /// 重复某个字符串N次。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="repeateTimes">次数N</param>
        /// <returns></returns>
        public static string RepeaterString(string str, int repeateTimes)
        {
            string s = "";
            for (int i = 0; i < repeateTimes; i++)
            {
                s += str;
            }
            return s;
        }
    }
}
