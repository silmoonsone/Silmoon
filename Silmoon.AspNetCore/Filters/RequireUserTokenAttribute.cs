using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Silmoon.AspNetCore.Services.Interfaces;
using Silmoon.Extension;
using Silmoon.Models;
using Silmoon.Models.Identities;
using Silmoon.Models.Identities.Enums;
using Silmoon.Models.Types;

namespace Silmoon.AspNetCore.Filters
{
    public class RequireUserTokenAttribute : ActionFilterAttribute
    {
        public IdentityRole Role { get; set; }
        public string UserTokenKey { get; set; }
        public string UserTokenSigninKey { get; set; }
        public bool IsRequire { get; set; } = true;
        public bool AllowSession { get; set; } = false;

        public RequireUserTokenAttribute(IdentityRole Role, string UserTokenKey = "UserToken", string UserTokenSigninKey = "UserTokenSignin", bool IsRequire = true, bool AllowSession = false) : base()
        {
            this.Role = Role;
            this.UserTokenKey = UserTokenKey;
            this.UserTokenSigninKey = UserTokenSigninKey;
            this.IsRequire = IsRequire;
            this.AllowSession = AllowSession;
        }
        public RequireUserTokenAttribute(string UserTokenKey = "UserToken", string UserTokenSigninKey = "UserTokenSignin", bool IsRequire = true, bool AllowSession = false) : base()
        {
            Role = IdentityRole.Undefined;
            this.UserTokenKey = UserTokenKey;
            this.UserTokenSigninKey = UserTokenSigninKey;
            this.IsRequire = IsRequire;
            this.AllowSession = AllowSession;
        }

        public async override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var UserToken = filterContext.HttpContext.Request.Query[UserTokenKey].ToStringOrNull();
            var UserTokenSignin = filterContext.HttpContext.Request.Query[UserTokenSigninKey].ToStringOrNull().ToBool();
            if (filterContext.HttpContext.Request.Method == "POST" && UserToken.IsNullOrEmpty()) UserToken = filterContext.HttpContext.Request.Form[UserTokenKey].ToStringOrNull();
            if (filterContext.HttpContext.Request.Method == "POST" && !UserTokenSignin) UserTokenSignin = filterContext.HttpContext.Request.Form[UserTokenSigninKey].ToStringOrNull().ToBool();

            var silmoonAuthService = filterContext.HttpContext.RequestServices.GetService<ISilmoonAuthService>();

            IDefaultUserIdentity tokenUser = default;
            if (!UserToken.IsNullOrEmpty()) tokenUser = await silmoonAuthService.GetUser<IDefaultUserIdentity>(UserToken, UserTokenSignin);
            if (tokenUser != null)
            {
                if (filterContext.Controller is Controller controller) controller.ViewBag._User = tokenUser;
            }
            else
            {
                if (AllowSession)
                {
                    if (!await silmoonAuthService.IsSignIn())
                    {
                        filterContext.Result = new ContentResult() { Content = ApiResult.Create(ResultState.Fail, "Require signin", -9999).ToJsonString(), ContentType = "application/json" };
                    }
                    else
                    {
                        if (filterContext.Controller is Controller controller) controller.ViewBag._User = await silmoonAuthService.GetUser<IDefaultUserIdentity>();
                    }
                }
                else
                {
                    if (IsRequire)
                    {
                        filterContext.Result = new ContentResult() { Content = ApiResult.Create(ResultState.Fail, "Require signin", -9999).ToJsonString(), ContentType = "application/json" };
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
