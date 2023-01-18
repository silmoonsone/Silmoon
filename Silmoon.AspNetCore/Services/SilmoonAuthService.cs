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
    public abstract class SilmoonAuthService : ISilmoonAuthService
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public SilmoonAuthService(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> IsSignIn()
        {
            var result = await HttpContextAccessor.HttpContext.AuthenticateAsync();
            return result.Succeeded;
        }
        public async Task SignIn<TUser>(TUser User, string NameIdentifier = null) where TUser : class, IDefaultUserIdentity
        {
            if (User is null) throw new ArgumentNullException(nameof(User));
            if (User.Username.IsNullOrEmpty() && NameIdentifier.IsNullOrEmpty()) throw new ArgumentNullException(nameof(User.Username), "Username或者NameIdentifier必选最少一个参数。");

            NameIdentifier = NameIdentifier.IsNullOrEmpty() ? User.Username : NameIdentifier;

            var claimsIdentity = new ClaimsIdentity("Customer");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, NameIdentifier));
            claimsIdentity.AddClaim(new Claim(nameof(IDefaultUserIdentity.Username), User.Username ?? ""));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContextAccessor.HttpContext.SignInAsync(claimsPrincipal);

            var aaa = HttpContextAccessor.HttpContext.User.Claims;
            SetUserCache(User, NameIdentifier);
        }
        public async Task<bool> SignOut()
        {
            if (await IsSignIn())
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
        public async Task<TUser> GetUser<TUser>() where TUser : class, IDefaultUserIdentity
        {
            if (await IsSignIn())
            {
                var NameIdentifier = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                var Name = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == nameof(IDefaultUserIdentity.Username)).FirstOrDefault()?.Value;

                string json = HttpContextAccessor.HttpContext.Session.GetString("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + Name);
                if (json.IsNullOrEmpty())
                {
                    TUser user = (TUser)GetUserData(Name, NameIdentifier);
                    if (user is null)
                    {
                        await SignOut();
                        return default;
                    }
                    else
                    {
                        SetUserCache(user, NameIdentifier);
                        return user;
                    }
                }
                else
                    return JsonConvert.DeserializeObject<TUser>(json);
            }
            else
                return null;
        }
        public async Task<TUser> GetUser<TUser>(string UserToken, bool SessionSignin, string Name = null, string NameIdentifier = null) where TUser : class, IDefaultUserIdentity
        {
            TUser result = (TUser)GetUserDataByUserToken(Name, NameIdentifier, UserToken);
            if (SessionSignin && result is not null)
            {
                await SignIn(result);
                SetUserCache(result, NameIdentifier);
            }
            return result;
        }
        public async Task ReloadUser<TUser>() where TUser : class, IDefaultUserIdentity
        {
            var NameIdentifier = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            var Name = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == nameof(IDefaultUserIdentity.Username)).FirstOrDefault()?.Value;
            TUser user = default;
            user = (TUser)GetUserData(Name, NameIdentifier);
            if (user is null) await SignOut();
            else SetUserCache(user, NameIdentifier);
        }


        void SetUserCache<TUser>(TUser User, string NameIdentifier) where TUser : class, IDefaultUserIdentity
        {
            //var NameIdentifier = HttpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            string userJson = User.ToJsonString();
            HttpContextAccessor.HttpContext.Session.SetString("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + User.Username, userJson);
        }

        public abstract IDefaultUserIdentity GetUserData(string Username, string NameIdentifier);
        public abstract IDefaultUserIdentity GetUserDataByUserToken(string Username, string NameIdentifier, string UserToken);
    }
}
