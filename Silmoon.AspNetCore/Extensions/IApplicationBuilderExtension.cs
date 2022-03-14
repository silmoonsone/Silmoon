using Microsoft.AspNetCore.Builder;
using Silmoon.Models.Identities;
using static Silmoon.AspNetCore.UserSessionManager;

namespace Silmoon.AspNetCore.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseUserSession<TUser>(this IApplicationBuilder app, UserSessionHanlder<IDefaultUserIdentity> OnRecoveryUserData, UserSessionHanlder<IDefaultUserIdentity> OnRequestRefreshUserSession) where TUser : IDefaultUserIdentity
        {
            OnRequestUserData += OnRecoveryUserData;
            UserSessionManager.OnRequestRefreshUserSession += OnRequestRefreshUserSession;
            return app;
        }
    }
}
