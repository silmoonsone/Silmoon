using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Silmoon.AspNetCore.Authentication
{
    public class RequireAuthenticationAttribute : AuthorizeAttribute
    {
        public RequireAuthenticationAttribute() : base()
        {
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme;
        }
    }
}
