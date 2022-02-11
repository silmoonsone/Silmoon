using Microsoft.AspNetCore.Builder;
using Silmoon.Models.Identities;
using static Silmoon.Web.AspNetCore.UserSessionManager;

namespace Silmoon.Web.AspNetCore.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseUserSession<TUser>(this IApplicationBuilder app, UserSessionHanlder<IDefaultUserIdentity> OnRecoveryUserData, UserSessionHanlder<IDefaultUserIdentity> OnRequestRefreshUserSession) where TUser : IDefaultUserIdentity
        {
            UserSessionManager.OnRequestUserData += OnRecoveryUserData;
            UserSessionManager.OnRequestRefreshUserSession += OnRequestRefreshUserSession;
            return app;
        }
    }
}
