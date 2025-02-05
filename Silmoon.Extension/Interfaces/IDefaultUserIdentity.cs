using Silmoon.Extension.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension.Interfaces
{
    public interface IDefaultUserIdentity
    {
        string Username { get; set; }
        string Password { get; set; }
        string Nickname { get; set; }
        IdentityRole Role { get; set; }
        IdentityStatus Status { get; set; }
        DateTime created_at { get; set; }
    }
}
