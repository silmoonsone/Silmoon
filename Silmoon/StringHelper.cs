using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

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
    /// <summary>
    /// 银月（老子叫宋维彬）的字符串类静态方法。
    /// </summary>
    public static class StringHelper
    {
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
        /// 把数组中的所有元素作为字符串使用一个指定的分隔符合并。
        /// </summary>
        /// <param name="array">数组，其中元素会作为字符串使用</param>
        /// <param name="perfixString">每个元素的前缀</param>
        /// <param name="suffixString">每个元素的后缀</param>
        /// <param name="SplitString">分隔符</param>
        /// <param name="RemoveLastSplitString">是否移除最后一个分隔符</param>
        /// <returns></returns>
        public static string MergeStringArray(Array array, string SplitString, bool RemoveLastSplitString = true, string perfixString = "", string suffixString = "")
        {
            string result = "";
            if (array == null || array.Length == 0) return result;
            foreach (object s in array)
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
        /// 检查邮件格式是否正确
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CheckEmail(string email)
        {
            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return regex.IsMatch(email);
        }
        /// <summary>
        /// 检查手机号码格式是否正确
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool CheckMobilePhone(string mobile)
        {
            Regex regex = new Regex(@"^((1[3,5,6,8][0-9])|(14[5,7])|(17[0,1,3,5,6,7,8])|(19[1,8,9]))\d{8}$");
            return regex.IsMatch(mobile);
        }
        /// <summary>
        /// 检查电话号码格式是否正确
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool CheckPhone(string phone)
        {
            Regex regex = new Regex(@"^(\d{3,4}-)?\d{6,8}$");
            return regex.IsMatch(phone);
        }
        /// <summary>
        /// 检查身份证号码格式是否正确
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        private static bool CheckCardId(string cardId)
        {
            if (cardId.Trim().Length != 18) return false;
            long n = 0;
            //数字验证
            if (long.TryParse(cardId.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(cardId.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;
            }

            //省份验证
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(cardId.Remove(2)) == -1)
            {
                return false;
            }

            //生日验证
            string birth = cardId.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;
            }
            //校验码验证
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = cardId.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != cardId.Substring(17, 1).ToLower())
            {
                return false;
            }
            return true;
            //符合GB11643-1999标准
        }
        /// <summary>
        /// 检查营业执照号码是否正确
        /// </summary>
        /// <param name="businessLicense"></param>
        /// <returns></returns>
        public static bool CheckBusinessLicense(string businessLicense)
        {
            Regex regex = new Regex(@"^[0-9A-Z]{8}-[0-9A-Z]$");
            return regex.IsMatch(businessLicense);
        }
    }
}
