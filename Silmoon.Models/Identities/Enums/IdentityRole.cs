using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Silmoon.Models.Identities.Enums
{
    public enum IdentityRole
    {
        [Display(Name = "未定义")]
        Undefined = 0,
        [Display(Name = "普通用户")]
        User = 1,
        [Display(Name = "一般管理")]
        GeneralAdmin = 8,
        [Display(Name = "管理员")]
        Admin = 16,
        [Display(Name = "超管")]
        Creater = 32,
    }
}
