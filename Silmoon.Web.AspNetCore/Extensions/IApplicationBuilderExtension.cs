using Microsoft.AspNetCore.Builder;
using Silmoon.Models.Identities;
using static Silmoon.Web.AspNetCore.UserSessionManager;

namespace Silmoon.Web.AspNetCore.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseUserSession<TUser>(this IApplicationBuilder app, UserSessionHanlder<IDefaultUserIdentity> OnRequestUserData, UserSessionHanlder<IDefaultUserIdentity> OnRequestRefreshUserSession) where TUser : IDefaultUserIdentity
        {
            UserSessionManager.OnRequestUserData += OnRequestUserData;
            UserSessionManager.OnRequestRefreshUserSession += OnRequestRefreshUserSession;
            return app;
        }
    }
}
