using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Silmoon.AspNetCore.Extensions;
using Silmoon.Extension;
using Silmoon.Models;
using Silmoon.Models.Types;

namespace Silmoon.AspNetCore.Filters
{
    public class ParameterSigntureAttribute : ActionFilterAttribute
    {
        public string KeyFieldName { get; set; }
        public string Key { get; set; }
        public string SignFieldName { get; set; }
        public bool Require { get; set; }
        public ParameterSigntureAttribute(string KeyFieldName, string Key, string SignFieldName, bool Require) : base()
        {
            this.KeyFieldName = KeyFieldName;
            this.Key = Key;
            this.SignFieldName = SignFieldName;
            this.Require = Require;
        }


        public async override Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            if (((string)filterContext.HttpContext.Request.Headers["ApiEncrypt"]).ToBool())
            {
                await base.OnActionExecutionAsync(filterContext, next);
                return;
            }
            else
            {
                string Sign;
                NameValueCollection nameValueCollection;

                if (filterContext.HttpContext.Request.Method == "GET")
                {
                    Sign = filterContext.HttpContext.Request.Query[SignFieldName];
                    nameValueCollection = filterContext.HttpContext.Request.GetQueryStringNameValues();
                }
                else
                {
                    Sign = filterContext.HttpContext.Request.Form[SignFieldName];
                    nameValueCollection = filterContext.HttpContext.Request.GetFormNameValues();
                }

                if (Require || !Sign.IsNullOrEmpty())
                {
                    var sign = nameValueCollection.GetSign(KeyFieldName, Key, SignFieldName);
                    if (Sign != sign)
                        filterContext.Result = new ContentResult() { Content = ApiResult<string>.CreateToJsonString(ResultState.Fail, null, "Sign error."), ContentType = "application/json" };
                    else await base.OnActionExecutionAsync(filterContext, next);
                }
                else await base.OnActionExecutionAsync(filterContext, next);
            }
        }
    }
}
