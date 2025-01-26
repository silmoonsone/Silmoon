using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Silmoon.Extension.Models.Identities.Enums
{
    [Flags]
    public enum IdentityStatus
    {
        [Display(Name = "被封禁")]
        Banned = -2,
        [Display(Name = "已锁定")]
        Locked = -1,
        [Display(Name = "未定义")]
        None = 0,
        [Display(Name = "正常")]
        Ok = 1,
    }
}
