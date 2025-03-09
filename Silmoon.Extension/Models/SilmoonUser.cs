using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Extension.Models
{
    [Serializable]
    public class SilmoonUser : DefaultUserIdentity
    {
        /// <summary>
        /// Sid是一个安全ID，是以App为唯一标识的用户ID，对App来说唯一。
        /// </summary>
        public string Sid { get; set; }
        [Obsolete]
        /// <summary>
        /// EUID是根据系统密钥+AppId加密过的用户ID，相当于微信的OpenId，对AppId来说唯一。
        /// 已过期，请使用Sid
        /// </summary>
        public string EUID { get; set; }
        [Obsolete]
        public string Name { get; set; }
        [Obsolete]
        public string Email { get; set; }
        [Obsolete]
        public string MobilePhone { get; set; }
    }
}
