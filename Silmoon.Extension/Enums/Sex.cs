using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Silmoon.Extension.Enums
{
    public enum Sex
    {
        [Display(Name = "未知")]
        Unknown = 0,
        [Display(Name = "男")]
        Male = 1,
        [Display(Name = "女")]
        Female = 2,
    }
}
