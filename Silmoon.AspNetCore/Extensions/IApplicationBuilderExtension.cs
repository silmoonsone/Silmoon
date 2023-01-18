using Microsoft.AspNetCore.Builder;
using Silmoon.Models.Identities;
using System;
using Silmoon.AspNetCore;

namespace Silmoon.AspNetCore.Extensions
{
    [Obsolete]
    public static class IApplicationBuilderExtension
    {
        [Obsolete]
        public static IApplicationBuilder UseUserSession<TUser>(this IApplicationBuilder app, UserSessionManager.UserSessionHanlder<IDefaultUserIdentity> OnRecoveryUserData, UserSessionManager.UserTokenHanlder<IDefaultUserIdentity> OnRequestUserToken) where TUser : IDefaultUserIdentity
        {
            UserSessionManager.OnRequestUserData += OnRecoveryUserData;
            UserSessionManager.OnRequestUserToken += OnRequestUserToken;
            return app;
        }
    }
}
