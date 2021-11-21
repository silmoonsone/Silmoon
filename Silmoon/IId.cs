using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    /// <summary>
    /// 表示一个对象应该有一个以ID标识的符号
    /// </summary>
    public interface IId
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        /// <returns></returns>
        int Id
        {
            get;
            set;
        }
    }
}
