using Silmoon.Models.Identities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Silmoon.AspNetCore.Services.Interfaces
{
    public interface ISilmoonAuthService
    {
        Task<bool> IsSignIn();
        Task SignIn<TUser>(TUser User, string NameIdentifier = null) where TUser : class, IDefaultUserIdentity;
        Task<bool> SignOut();
        Task<TUser> GetUser<TUser>() where TUser : class, IDefaultUserIdentity;
        Task<TUser> GetUser<TUser>(string UserToken, string Name = null, string NameIdentifier = null) where TUser : class, IDefaultUserIdentity;
        Task ReloadUser<TUser>() where TUser : class, IDefaultUserIdentity;
    }
}
