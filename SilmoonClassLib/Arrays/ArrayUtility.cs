using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

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
        public static IID FindIIDFromArray(ArrayList array, int id)
        {
            IID o = null;
            lock (array)
            {
                foreach (object item in array)
                {
                    if (item != null)
                    {
                        IID iID = item as IID;
                        if (iID != null)
                        {
                            if (iID.ID == id)
                            {
                                o = iID;
                                break;
                            }
                        }
                    }
                }
            }
            return o;
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
        /// <summary>
        /// 分组数据，将一段数据按照指定的长度进行分割，最后不足的长度将不会冲零。
        /// </summary>
        /// <param name="data">需要分组的数据</param>
        /// <param name="slen">每段数据的长度</param>
        /// <returns></returns>
        public static byte[][] GroupData(byte[] data, int slen)
        {
            int scount = data.Length / slen;
            int remainder = 0;
            if ((remainder = (data.Length % slen)) != 0) scount++;

            byte[][] result = new byte[scount][];

            for (int i = 0; i < scount; i++)
            {
                if (i != (scount - 1))
                {
                    result[i] = new byte[slen];
                    Array.Copy(data, i * slen, result[i], 0, slen);
                }
                else
                {
                    result[i] = new byte[remainder];
                    Array.Copy(data, i * slen, result[i], 0, remainder);
                }
            }
            return result;
        }

    }
}
