using Silmoon.AspNetCore.Services;
using Silmoon.AspNetCore.Test.Models;

namespace Silmoon.AspNetCore.Test.Services
{
    public class SilmoonUserServiceImpl : SilmoonUserService<User>
    {
        public SilmoonUserServiceImpl(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        public override User GetUserData(string Username, string NameIdentifier, User User)
        {
            return new User() { Username = "silmoon", Password = "pwd" };
        }
        public override User GetUserDataByUserToken(string Username, string NameIdentifier, string UserToken)
        {
            return new User() { Username = "silmoon", Password = "pwd" };
        }
    }
}
