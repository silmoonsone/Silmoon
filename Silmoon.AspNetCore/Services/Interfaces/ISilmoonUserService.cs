using Silmoon.Models.Identities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Silmoon.AspNetCore.Services.Interfaces
{
    public interface ISilmoonAuthService
    {
        Task<bool> IsSignIn();
        Task SignIn<TUser>(TUser User, string NameIdentifier = null) where TUser : IDefaultUserIdentity;
        Task<bool> SignOut();
        Task<TUser> GetUser<TUser>() where TUser : IDefaultUserIdentity;
        Task<TUser> GetUser<TUser>(string UserToken, bool SessionSignin, string Name = null, string NameIdentifier = null) where TUser : IDefaultUserIdentity;
        Task ReloadUser<TUser>() where TUser : IDefaultUserIdentity;
    }
}
