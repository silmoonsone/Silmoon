using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Arrays
{
    public class DictionarySortV2 : System.Collections.IComparer
    {
        /// <summary>
        /// 比较两个字符串的字典顺序。
        /// </summary>
        /// <param name="x">第一个比较对象。</param>
        /// <param name="y">第二个比较对象。</param>
        /// <returns>小于零：x 在 y 之前；零：x 等于 y；大于零：x 在 y 之后。</returns>
        public int Compare(object x, object y)
        {
            // 确保两个对象都是字符串
            if (x is string leftString && y is string rightString)
            {
                return string.Compare(leftString, rightString, StringComparison.Ordinal);
            }

            throw new ArgumentException("Both parameters must be strings.");
        }
    }
}