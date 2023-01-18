using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Silmoon.AspNetCore.Services.Interfaces;
using Silmoon.Extension;
using Silmoon.Models.Identities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Silmoon.AspNetCore.Services
{
    public abstract class SilmoonUserService<IUser> : ISilmoonUserService<IUser> where IUser : IDefaultUserIdentity
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public SilmoonUserService(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> IsSignin()
        {
            var result = await HttpContextAccessor.HttpContext.AuthenticateAsync();
            return result.Succeeded;
        }
        public async Task SignIn(IUser User, string NameIdentifier = null)
        {
            if (User is null) throw new ArgumentNullException(nameof(User));
            if (User.Username.IsNullOrEmpty() && NameIdentifier.IsNullOrEmpty()) throw new ArgumentNullException(nameof(User.Username), "Username或者NameIdentifier必选最少一个参数。");

            NameIdentifier = NameIdentifier.IsNullOrEmpty() ? User.Username : NameIdentifier;

            var claimsIdentity = new ClaimsIdentity("Customer");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, NameIdentifier));
            claimsIdentity.AddClaim(new Claim(nameof(IDefaultUserIdentity.Username), User.Username ?? ""));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            SetUserCache(User);

            await HttpContextAccessor.HttpContext.SignInAsync(claimsPrincipal);
        }
        public async Task<bool> SignOut()
        {
            if (await IsSignin())
            {
                var NameIdentifier = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                var Name = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == nameof(IDefaultUserIdentity.Username)).FirstOrDefault()?.Value;
                HttpContextAccessor.HttpContext.Session.Remove("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + Name);
                await HttpContextAccessor.HttpContext.SignOutAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<IUser> GetUser()
        {
            if (await IsSignin())
            {
                var NameIdentifier = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                var Name = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == nameof(IDefaultUserIdentity.Username)).FirstOrDefault()?.Value;

                string json = HttpContextAccessor.HttpContext.Session.GetString("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + Name);
                IUser user = default;
                if (json.IsNullOrEmpty())
                {
                    user = GetUserData(Name, NameIdentifier, user);
                    if (user is null)
                    {
                        await SignOut();
                        return default;
                    }
                    SetUserCache(user);
                }
                else
                    user = JsonConvert.DeserializeObject<IUser>(json);
                return user;
            }
            else
                return default;
        }
        public async Task<IUser> GetUser(string UserToken, bool SessionSignin, string Name = null, string NameIdentifier = null)
        {
            //if (OnRequestUserToken is null) throw new NullReferenceException("UserSessionManager.OnRequestUserToken 事件未注册");
            var result = GetUserDataByUserToken(Name, NameIdentifier, UserToken);
            if (SessionSignin && result is not null)
            {
                await SignIn(result);
                SetUserCache(result);
            }
            return result;
        }
        public async Task ReloadUser()
        {
            var NameIdentifier = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            var Name = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == nameof(IDefaultUserIdentity.Username)).FirstOrDefault()?.Value;
            IUser user = default;
            user = GetUserData(Name, NameIdentifier, user);
            if (user is null) await SignOut();
            else SetUserCache(user);
        }


        void SetUserCache(IUser User)
        {
            var NameIdentifier = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            string userJson = User.ToJsonString();
            HttpContextAccessor.HttpContext.Session.SetString("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + User.Username, userJson);
        }

        public abstract IUser GetUserData(string Username, string NameIdentifier, IUser User);
        public abstract IUser GetUserDataByUserToken(string Username, string NameIdentifier, string UserToken);
    }
}
