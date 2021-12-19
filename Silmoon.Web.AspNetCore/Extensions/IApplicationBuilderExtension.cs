﻿using Microsoft.AspNetCore.Builder;
using Silmoon.Models.Identities;
using static Silmoon.Web.AspNetCore.UserSessionManager;

namespace Silmoon.Web.AspNetCore.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseUserSession<TUser>(this IApplicationBuilder app, UserSessionHanlder<TUser> OnRequestUserData) where TUser : IDefaultUserIdentityV2
        {
            return app;
        }
    }
}