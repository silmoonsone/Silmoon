using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Arrays
{
    /// <summary>
    /// 表示一个对象应该有一个以ID标识的符号
    /// </summary>
    public interface IID
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        /// <returns></returns>
        int ID
        {
            get;
            set;
        }
    }
}
