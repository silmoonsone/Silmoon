﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Silmoon.Web.Core.Extension
{
    [Obsolete]
    public static class HttpContextBaseExtension
    {
        [Obsolete]
        public static async void Signin(this HttpContext httpContext, ClaimsPrincipal principal)
        {
            await httpContext.SignInAsync(principal);

        }
    }
}