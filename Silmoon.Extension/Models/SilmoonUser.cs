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
        /// EUID是根据系统密钥+AppId加密过的用户ID，相当于微信的OpenId，对AppId来说唯一。
        /// </summary>
        public string EUID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
    }
}
