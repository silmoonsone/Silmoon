using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Silmoon.Extension;

namespace Silmoon.AspNetCore.Filters
{
    /// <summary>
    /// 实现使用UserToken登录的时候，在当前请求刷新登录状态，登录标志为UserTokenSignin，UserToken为UserToken。
    /// </summary>
    public class UserTokenSigninAutoRefreshAttribute : ActionFilterAttribute
    {
        public async override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (await filterContext.HttpContext.IsSignin() is not true && filterContext.HttpContext.Request.Query["UserTokenSignin"].ToString().ToBool() && !filterContext.HttpContext.Request.Query["UserToken"].ToString().IsNullOrEmpty())
                filterContext.Result = new RedirectResult((string)filterContext.HttpContext.Request.RouteValues["action"] + filterContext.HttpContext.Request.QueryString);

            base.OnActionExecuting(filterContext);
        }
    }
}
