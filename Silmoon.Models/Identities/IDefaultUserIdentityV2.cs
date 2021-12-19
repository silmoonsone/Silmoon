using Silmoon.Models.Identities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models.Identities
{
    public interface IDefaultUserIdentityV2
    {
        string Name { get; set; }
        string Password { get; set; }
        string Nickname { get; set; }
        IdentityRole Role { get; set; }
        IdentityStatus Status { get; set; }
        DateTime created_at { get; set; }
    }
}
