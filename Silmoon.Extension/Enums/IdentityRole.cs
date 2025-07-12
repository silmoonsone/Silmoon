using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Silmoon.Extension.Enums
{
    [Flags]
    public enum IdentityRole
    {
        /// <summary>
        /// 0 (0x00) - Undefined
        /// </summary>
        [Display(Name = "未定义")]
        Undefined = 0b00000,
        /// <summary>
        /// 1 (0x01) - User
        /// </summary>
        [Display(Name = "普通用户")]
        User = 0b00001,
        /// <summary>
        /// 8 (0x08) - General Admin
        /// </summary>
        [Display(Name = "一般管理")]
        GeneralAdmin = 0b01000,
        /// <summary>
        /// 16 (0x10) - Admin
        /// </summary>
        [Display(Name = "管理员")]
        Admin = 0b10000,
        /// <summary>
        /// 32 (0x20) - Creator
        /// </summary>
        [Display(Name = "超管")]
        Creator = 0b100000,
    }
}