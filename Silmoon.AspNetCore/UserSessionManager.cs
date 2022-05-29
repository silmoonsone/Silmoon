using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Silmoon.Extension;
using Silmoon.Models;
using Silmoon.Models.Identities;
using Silmoon.AspNetCore.Extensions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Silmoon.AspNetCore
{
    public static class UserSessionManager
    {
        public static event UserSessionHanlder<IDefaultUserIdentity> OnRequestUserData;
        public static event UserTokenHanlder<IDefaultUserIdentity> OnRequestUserToken;

        public static async Task<bool> IsSignin(this HttpContext httpContext)
        {
            var result = await httpContext.AuthenticateAsync();
            return result.Succeeded;
        }
        public static async Task Signin<TUser>(this HttpContext httpContext, TUser User, string NameIdentifier = null) where TUser : IDefaultUserIdentity, new()
        {
            if (User is null) throw new ArgumentNullException(nameof(User));
            if (User.Username.IsNullOrEmpty() && NameIdentifier.IsNullOrEmpty()) throw new ArgumentNullException(nameof(User.Username), "Username或者NameIdentifier必选最少一个参数。");
            NameIdentifier = NameIdentifier.IsNullOrEmpty() ? User.Username : NameIdentifier;

            var claimsIdentity = new ClaimsIdentity("Customer");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, NameIdentifier));
            claimsIdentity.AddClaim(new Claim(nameof(IDefaultUserIdentity.Username), User.Username ?? ""));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            string json = JsonSerializer.Serialize(User);
            httpContext.Session.SetString("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + User.Username, json);

            await httpContext.SignInAsync(claimsPrincipal);
        }
        public static async Task<bool> Signout(this HttpContext httpContext)
        {
            if (await IsSignin(httpContext))
            {
                var NameIdentifier = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                var Name = httpContext.User.Claims.Where(c => c.Type == nameof(IDefaultUserIdentity.Username)).FirstOrDefault()?.Value;
                httpContext.Session.Remove("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + Name);
                await httpContext.SignOutAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<TUser> GetUser<TUser>(this HttpContext httpContext) where TUser : IDefaultUserIdentity, new()
        {
            if (await IsSignin(httpContext))
            {
                var NameIdentifier = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                var Name = httpContext.User.Claims.Where(c => c.Type == nameof(IDefaultUserIdentity.Username)).FirstOrDefault()?.Value;

                string json = httpContext.Session.GetString("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + Name);
                TUser user = default;
                if (json.IsNullOrEmpty())
                {
                    user = (TUser)OnRequestUserData?.Invoke(Name, NameIdentifier, user);
                    if (user is null)
                    {
                        await Signout(httpContext);
                        return default;
                    }
                    SetUserCache(httpContext, user);
                }
                else
                {
                    user = JsonSerializer.Deserialize<TUser>(json);
                }
                return user;
            }
            else
            {
                return default;
            }
        }
        public static TUser GetUser<TUser>(this HttpContext httpContext, string UserToken, bool SessionSignin, string Name = null, string NameIdentifier = null) where TUser : IDefaultUserIdentity, new()
        {
            var result = (TUser)OnRequestUserToken?.Invoke(Name, NameIdentifier, UserToken);
            if (SessionSignin && result is not null) SetUserCache(httpContext, result);
            return result;
        }
        static void SetUserCache<TUser>(this HttpContext httpContext, TUser User) where TUser : IDefaultUserIdentity, new()
        {
            var NameIdentifier = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            string json = JsonSerializer.Serialize(User);
            httpContext.Session.SetString("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + User.Username, json);
        }

        public delegate TUser UserSessionHanlder<TUser>(string Username, string NameIdentifier, TUser User);
        public delegate TUser UserTokenHanlder<TUser>(string Username, string NameIdentifier, string UserToken);
    }
}
