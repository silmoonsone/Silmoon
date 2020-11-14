using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Security
{
    /// <summary>
    /// 表示一个用户表示和认证密码
    /// </summary>
    public struct IdentityAuthInfo
    {
        /// <summary>
        /// 新建立的一个表示信息
        /// </summary>
        /// <param name="identityString"></param>
        /// <param name="passwordCode"></param>
        public IdentityAuthInfo(string identityString, string passwordCode)
        {
            IdentityString = identityString;
            PasswordCode = passwordCode;
        }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string IdentityString;
        /// <summary>
        /// 用于认证的密码
        /// </summary>
        public string PasswordCode;
    }
}
