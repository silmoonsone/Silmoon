using Silmoon.AspNetCore.Services;
using Silmoon.AspNetCore.Test.Models;
using Silmoon.Models.Identities;

namespace Silmoon.AspNetCore.Test.Services
{
    public class SilmoonAuthServiceImpl : SilmoonAuthService
    {
        public SilmoonAuthServiceImpl(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, httpContextAccessor)
        {

        }
        public override IDefaultUserIdentity GetUserData(string Username, string NameIdentifier)
        {
            return new User() { Username = "silmoon", Password = "pwd" };
        }
        public override IDefaultUserIdentity GetUserDataByUserToken(string Username, string NameIdentifier, string UserToken)
        {
            return new User() { Username = "silmoon", Password = "pwd" };
        }
    }
}
