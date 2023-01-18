using Silmoon.AspNetCore.Services;
using Silmoon.AspNetCore.Test.Models;
using Silmoon.Models.Identities;

namespace Silmoon.AspNetCore.Test.Services
{
    public class SilmoonUserServiceImpl : SilmoonAuthService
    {
        public SilmoonUserServiceImpl(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        public override IDefaultUserIdentity GetUserData(string Username, string NameIdentifier, IDefaultUserIdentity User)
        {
            return new User() { Username = "silmoon", Password = "pwd" };
        }
        public override IDefaultUserIdentity GetUserDataByUserToken(string Username, string NameIdentifier, string UserToken)
        {
            return new User() { Username = "silmoon", Password = "pwd" };
        }
    }
}
