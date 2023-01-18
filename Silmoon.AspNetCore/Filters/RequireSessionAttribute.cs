using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Silmoon.AspNetCore.Extensions;
using Silmoon.AspNetCore.Services.Interfaces;
using Silmoon.Extension;
using Silmoon.Models;
using Silmoon.Models.Identities;
using Silmoon.Models.Identities.Enums;
using System.Web;

namespace Silmoon.AspNetCore.Filters
{
    public class RequireSessionAttribute : ActionFilterAttribute
    {
        public IdentityRole Role { get; set; }
        public bool RequestRefreshUserSession { get; set; }
        public string SignInUrl { get; set; } = "~/User/Signin?url=$SigninUrl";
        public bool IsApiRequest { get; set; } = false;
        public string UserTokenKey { get; set; }
        public string UserTokenSigninKey { get; set; }
        public bool IsRequire { get; set; } = true;
        public RequireSessionAttribute(IdentityRole Role, bool RequestRefreshUserSession = false, bool IsApiRequest = false, string UserTokenKey = "UserToken", string UserTokenSigninKey = "UserTokenSignin", bool IsRequire = true, string SignInUrl = "~/User/Signin?url=$SigninUrl") : base()
        {
            this.Role = Role;
            this.RequestRefreshUserSession = RequestRefreshUserSession;
            this.UserTokenKey = UserTokenKey;
            this.IsApiRequest = IsApiRequest;
            this.UserTokenSigninKey = UserTokenSigninKey;
            this.SignInUrl = SignInUrl;
            this.IsRequire = IsRequire;
        }
        public RequireSessionAttribute(bool RequestRefreshUserSession = false, bool IsApiRequest = false, string UserTokenKey = "UserToken", string UserTokenSigninKey = "UserTokenSignin", bool IsRequire = true, string SignInUrl = "~/User/Signin?url=$SigninUrl") : base()
        {
            Role = IdentityRole.Undefined;
            this.RequestRefreshUserSession = RequestRefreshUserSession;
            this.UserTokenKey = UserTokenKey;
            this.IsApiRequest = IsApiRequest;
            this.UserTokenSigninKey = UserTokenSigninKey;
            this.SignInUrl = SignInUrl;
            this.IsRequire = IsRequire;
        }

        public async override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var UserToken = filterContext.HttpContext.Request.Query[UserTokenKey].ToStringOrNull();
            var UserTokenSignin = filterContext.HttpContext.Request.Query[UserTokenSigninKey].ToStringOrNull().ToBool();
            var isAjax = filterContext.HttpContext.Request.IsAjaxRequest();
            var signUrl = SignInUrl?.Replace("$SigninUrl", HttpUtility.UrlEncode(filterContext.HttpContext.Request.GetRawUrl()));

            var silmoonAuthService = filterContext.HttpContext.RequestServices.GetService<ISilmoonAuthService>();

            if (!await silmoonAuthService.IsSignIn())
            {
                IDefaultUserIdentity tokenUser = default;
                if (!UserToken.IsNullOrEmpty()) tokenUser = await silmoonAuthService.GetUser<IDefaultUserIdentity>(UserToken, UserTokenSignin);
                if (tokenUser != null)
                {
                    if (filterContext.Controller is Controller controller) controller.ViewBag._User = tokenUser;
                }
                else if (IsRequire)
                {
                    if (isAjax || IsApiRequest)
                        filterContext.Result = new ContentResult() { Content = StateFlag.Create(Success: false, -9999, "no signin.").ToJsonString(), ContentType = "application/json" };
                    else
                        filterContext.Result = new RedirectResult(signUrl);
                }
            }
            else
            {
                var user = await silmoonAuthService.GetUser<IDefaultUserIdentity>();
                if (user is not null)
                {
                    if (filterContext.Controller is Controller controller) controller.ViewBag._User = user;
                    if (user.Role < Role)
                    {
                        if (isAjax || IsApiRequest)
                            filterContext.Result = new ContentResult() { Content = StateFlag.Create(Success: false, -9999, "permission error.").ToJsonString(), ContentType = "application/json" };
                        else
                            filterContext.Result = new ContentResult() { Content = "permission error." };
                    }
                }
                else
                {
                    await silmoonAuthService.SignOut();
                    if (IsRequire)
                    {
                        if (isAjax || IsApiRequest)
                            filterContext.Result = new ContentResult() { Content = StateFlag.Create(Success: false, -9999, "no signin.").ToJsonString(), ContentType = "application/json" };
                        else
                            filterContext.Result = new RedirectResult(signUrl);
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
