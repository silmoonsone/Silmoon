using Silmoon.Models.Identities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Silmoon.Models.Identities
{
    [Serializable]
    public class DefaultUserIdentityV2 : IDefaultUserIdentityV2
    {
        [DisplayName("登录名")]
        public string Name { get; set; }
        [DisplayName("密码密文")]
        public string Password { get; set; }
        [DisplayName("昵称")]
        public string Nickname { get; set; }
        [DisplayName("权限")]
        public IdentityRole Role { get; set; }
        [DisplayName("状态")]
        public IdentityStatus Status { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
