using Silmoon.Models.Identities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Silmoon.AspNetCore.Services.Interfaces
{
    public interface ISilmoonUserService<IUser> where IUser : IDefaultUserIdentity
    {
        Task<bool> IsSignin();
        Task SignIn(IUser User, string NameIdentifier = null);
        Task<bool> SignOut();
        Task<IUser> GetUser();
        Task<IUser> GetUser(string UserToken, bool SessionSignin, string Name = null, string NameIdentifier = null);
        Task ReloadUser();
    }
}
