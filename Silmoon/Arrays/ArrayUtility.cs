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
        /// 从标有ID的类型数组中找出指定的对象
        /// </summary>
        /// <param name="array">可变的数组</param>
        /// <param name="id">IID的ID</param>
        /// <returns></returns>
        public static IId FindIIDFromArray(List<IId> array, int id)
        {
            lock (array)
            {
                foreach (IId item in array)
                {
                    if (item != null && item.Id == id)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
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
