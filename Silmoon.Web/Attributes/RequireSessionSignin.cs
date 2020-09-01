using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Web.Attributes
{
    /// <summary>
    /// 控制器方法需要登录状态属性
    /// </summary>
    public class RequireSessionSignin : Attribute
    {
        /// <summary>
        /// 实例化登录状态属性
        /// </summary>
        public RequireSessionSignin()
        {

        }

    }
}
