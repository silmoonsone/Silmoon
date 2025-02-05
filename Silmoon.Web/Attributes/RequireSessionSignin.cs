using Silmoon.Extension;
using Silmoon.Extension.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silmoon.Web.Attributes
{
    /// <summary>
    /// 控制器方法需要登录状态特性
    /// </summary>
    public class RequireSessionActionFilterAttribute : ActionFilterAttribute/* where TUser : IDefaultUserIdentity*/
    {
        public IdentityRole? Role { get; set; }
        public bool RequestRefreshUserSession { get; set; }
        public bool IsAppApiRequest { get; set; }
        public string SignInUrl { get; set; }
        public RequireSessionActionFilterAttribute(/*UserSessionManager<TUser> UserSession, */IdentityRole Role, bool RequestRefreshUserSession = false, bool IsAppApiRequest = false, string SignInUrl = "~/User/Signin?url=$SigninUrl") : base()
        {
            this.Role = Role != IdentityRole.Undefined ? Role : new IdentityRole?();
            this.RequestRefreshUserSession = RequestRefreshUserSession;
            this.IsAppApiRequest = IsAppApiRequest;
            this.SignInUrl = SignInUrl;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //UserSessionManager<TUser> userSession = new UserSessionManager<TUser>();
            //var checkResult = userSession.ActionFilterChecking(filterContext.HttpContext, Role, RequestRefreshUserSession, IsAppApiRequest, SignInUrl);
            //if (checkResult is JsonResult)
            //{
            //    var jsonResult = (JsonResult)checkResult;
            //    filterContext.HttpContext.Response.Write(jsonResult.Data.ToJsonString());
            //    filterContext.HttpContext.Response.ContentType = jsonResult.ContentType;
            //    filterContext.HttpContext.Response.End();
            //}
            //else if (checkResult is RedirectResult)
            //{
            //    filterContext.HttpContext.Response.Redirect(((RedirectResult)checkResult).Url);
            //    filterContext.HttpContext.Response.End();
            //}
            //else if (checkResult is ContentResult)
            //{
            //    var contentResult = (ContentResult)checkResult;
            //    filterContext.HttpContext.Response.Write(contentResult.Content);
            //    filterContext.HttpContext.Response.ContentType = contentResult.ContentType;
            //    filterContext.HttpContext.Response.End();
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}
