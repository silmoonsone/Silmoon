using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;

namespace Silmoon.Arrays
{
    /// <summary>
    /// 操作数组的使用类型
    /// </summary>
    public class ArrayUtility
    {
        /// <summary>
        /// 将一个数据行的所有键值关系复制到NameValueCollection中。
        /// </summary>
        /// <param name="row">一个数据行</param>
        /// <param name="columns">要使用和引用的数据字段集合</param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(DataRow row, DataColumnCollection columns)
        {
            NameValueCollection result = new NameValueCollection();

            for (int i = 0; i < columns.Count; i++)
            {
                result[columns[i].ColumnName] = row[columns[i].ColumnName].ToString();
            }

            return result;
        }
    }
}
