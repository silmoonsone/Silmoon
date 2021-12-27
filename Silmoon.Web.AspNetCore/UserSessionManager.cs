using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Silmoon.Extension;
using Silmoon.Models;
using Silmoon.Models.Identities;
using Silmoon.Web.AspNetCore.Extensions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Silmoon.Web.AspNetCore
{
    public static class UserSessionManager
    {
        public static event UserSessionHanlder<IDefaultUserIdentity> OnRequestRefreshUserSession;
        public static event UserSessionHanlder<IDefaultUserIdentity> OnRequestUserData;
        public static event UserTokenHanlder<IDefaultUserIdentity> OnRequestUserToken;

        public static async Task<bool> IsSignin(this HttpContext httpContext)
        {
            var result = await httpContext.AuthenticateAsync();
            return result.Succeeded;
        }
        public static async Task Signin<TUser>(this HttpContext httpContext, TUser User, string NameIdentifier = null) where TUser : IDefaultUserIdentity, new()
        {
            if (User.Username.IsNullOrEmpty() && NameIdentifier.IsNullOrEmpty()) throw new ArgumentNullException(nameof(User.Username));
            NameIdentifier = NameIdentifier.IsNullOrEmpty() ? User.Username : NameIdentifier;

            var claimsIdentity = new ClaimsIdentity("Customer");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, NameIdentifier));
            claimsIdentity.AddClaim(new Claim(nameof(IDefaultUserIdentity.Username), User.Username));

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
        public static async Task<IActionResult> MvcSessionChecking<TUser>(this ControllerBase controller, Models.Identities.Enums.IdentityRole? IsRole, bool requestRefreshUserSession = false, bool isAppApiRequest = false, string signInUrl = "~/User/Signin?url=$SigninUrl") where TUser : IDefaultUserIdentity, new()
        {
            var User = await GetCachedUser<TUser>(controller.HttpContext);

            signInUrl = signInUrl?.Replace("$SigninUrl", HttpUtility.UrlEncode(controller.HttpContext.Request.GetRawUrl()));
            var Name = controller.HttpContext.Request.Query["Name"].FirstOrDefault();
            var NameIdentifier = controller.HttpContext.Request.Query["NameIdentifier"].FirstOrDefault();
            var userToken = controller.HttpContext.Request.Query["UserToken"].FirstOrDefault() ?? controller.HttpContext.Request.Query["AppUserToken"].FirstOrDefault();
            var tokenNoSession = controller.HttpContext.Request.Query["TokenNoSession"].FirstOrDefault().ToBool(false, false);
            var ignoreUserToken = controller.HttpContext.Request.Query["ignoreUserToken"].FirstOrDefault().ToBool(false, false);

            if (!await IsSignin(controller.HttpContext) || (!userToken.IsNullOrEmpty() && !ignoreUserToken))
            {
                if (userToken.IsNullOrEmpty())
                {
                    ///不是登录状态，并且没有提供AppUserToken的情况下。
                    if (controller.Request.IsAjaxRequest())
                        return new JsonResult(StateFlag.Create(false, -9999, "no signin."));
                    else return new RedirectResult(signInUrl);
                }
                else
                {
                    ///提供了AppUserToken的情况下
                    if (userToken.ToLower() == "null")
                    {
                        ///提供的AppUserToken的字符串是null。
                        if (controller.Request.IsAjaxRequest() || isAppApiRequest)
                            return new JsonResult(StateFlag.Create(false, -9999, "usertoken is \"null\"."));
                        else return new RedirectResult(signInUrl);
                    }
                    else
                    {
                        ///调用UserToken登录验证处理过程，获取用户实体。
                        var userInfo = OnRequestUserToken(Name, NameIdentifier, userToken);
                        if (userInfo != null)
                        {
                            ///如果AppUserToken验证过程返回了用户实体。
                            User = (TUser)userInfo;
                            SetUserCache(controller.HttpContext, (TUser)userInfo);
                            if (!tokenNoSession) await Signin(controller.HttpContext, User);

                            ///使用UserToken登录后处理
                            if (IsRole.HasValue && User.Role < IsRole)
                            {
                                if (controller.Request.IsAjaxRequest() || isAppApiRequest)
                                    return new JsonResult(StateFlag.Create(false, -9999, "access denied."));
                                else return new ContentResult() { Content = "access denied", ContentType = "text/plain" };
                            }
                            if (controller is Controller) ((Controller)controller).ViewBag.User = User;
                            return null;
                        }
                        else
                        {
                            if (controller.Request.IsAjaxRequest() || isAppApiRequest)
                                return new JsonResult(StateFlag.Create(false, -9999, "OnRequestUserToken return null."));
                            else
                            {
                                ///这里存在一个冲突，如果当前是登录状态，并且使用AppUserToken登录，AppUserToken登录失败，会转跳到登录页面，但是又是由于登录状态，会再次跳回当前页面，会造成死循环。
                                return new RedirectResult(signInUrl);
                            }
                        }
                    }
                }
            }
            else
            {
                NameIdentifier = controller.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                Name = controller.HttpContext.User.Claims.Where(c => c.Type == nameof(IDefaultUserIdentity.Username)).FirstOrDefault()?.Value;

                if (requestRefreshUserSession)
                {
                    var userInfo = await onRequestRefreshUserSession<TUser>(controller.HttpContext, Name, NameIdentifier);
                    if (userInfo != null)
                    {
                        User = userInfo;
                        SetUserCache(controller.HttpContext, userInfo);
                    }
                    else
                    {
                        if (controller.Request.IsAjaxRequest() || isAppApiRequest)
                            return new JsonResult(StateFlag.Create(false, -9999, "onRequestRefreshUserSession return null."));
                        else return new RedirectResult(signInUrl);
                    }
                }

                if (IsRole.HasValue)
                {
                    if (User.Role < IsRole)
                    {
                        if (controller.Request.IsAjaxRequest() || isAppApiRequest)
                            return new JsonResult(StateFlag.Create(false, -9999, "access denied."));
                        else return new ContentResult() { Content = "access denied", ContentType = "text/plain" };
                    }
                }

                if (controller is Controller) ((Controller)controller).ViewBag.User = User;
                return null;
            }
        }

        public static async Task<TUser> GetCachedUser<TUser>(this HttpContext httpContext) where TUser : IDefaultUserIdentity, new()
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
        public static void SetUserCache<TUser>(this HttpContext httpContext, TUser User) where TUser : IDefaultUserIdentity, new()
        {
            var NameIdentifier = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            string json = JsonSerializer.Serialize(User);
            httpContext.Session.SetString("SessionCache:NameIdentifier+Username=" + NameIdentifier + "+" + User.Username, json);
        }



        static async Task<TUser> onRequestRefreshUserSession<TUser>(HttpContext httpContext, string Username, string NameIdentifier) where TUser : IDefaultUserIdentity, new()
        {

            if (OnRequestRefreshUserSession == null)
                return default;
            else
            {
                var user = await GetCachedUser<TUser>(httpContext);
                if (user == null) return default;
                return (TUser)OnRequestRefreshUserSession(Username, NameIdentifier, user);
            }
        }
        public delegate TUser UserSessionHanlder<TUser>(string Username, string NameIdentifier, TUser User);
        public delegate TUser UserTokenHanlder<TUser>(string Username, string NameIdentifier, string UserToken);
    }
}
