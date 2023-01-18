using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Silmoon.AspNetCore.Extensions;
using Silmoon.Extension;
using Silmoon.Models;
using Silmoon.Models.Types;
using System.Security.Cryptography.Xml;
using Microsoft.Extensions.DependencyInjection;
using Silmoon.AspNetCore.Services;
using Silmoon.AspNetCore.Services.Interfaces;

namespace Silmoon.AspNetCore.Filters
{
    public class RequestSignatureValidationAttribute : ActionFilterAttribute
    {
        public string KeyFieldName { get; set; }
        public string AppIdFieldName { get; set; }
        public string SignFieldName { get; set; }
        public bool Require { get; set; }
        public ISilmoonDevAppService SilmoonDevAppService { get; set; }
        /// <summary>
        /// 使用指定的Key验证请求的签名，如果是GET请求，将验证所有GET参数，如果是POST请求，将验证全部POST参数，
        /// </summary>
        /// <param name="KeyFieldName">拼凑签名字符串时，使用Key的参数名称</param>
        /// <param name="AppIdFieldName">查找Key的根据参数，由GetKey(string)方法返回Key</param>
        /// <param name="SignFieldName">请求的签名所在的参数名</param>
        /// <param name="Require">是否为必须签名，在此参数为false，并且Sign为空的情况下，将忽略签名</param>
        public RequestSignatureValidationAttribute(string KeyFieldName = "Key", string AppIdFieldName = "AppId", string SignFieldName = "Signature", bool Require = true) : base()
        {
            this.KeyFieldName = KeyFieldName;
            this.AppIdFieldName = AppIdFieldName;
            this.SignFieldName = SignFieldName;
            this.Require = Require;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            if (((string)filterContext.HttpContext.Request.Headers["ApiEncrypt"]).ToBool()
                || ((string)filterContext.HttpContext.Request.Headers["SilmoonDevAppEncrypted"]).ToBool()
                || ((string)filterContext.HttpContext.Request.Headers["EncryptionApi"]).ToBool()
                || ((string)filterContext.HttpContext.Request.Headers["ApiEncryption"]).ToBool())
            {
                await base.OnActionExecutionAsync(filterContext, next);
            }
            else
            {
                if (SilmoonDevAppService == null)
                {
                    SilmoonDevAppService = filterContext.HttpContext.RequestServices.GetService<ISilmoonDevAppService>();
                    if (SilmoonDevAppService == null)
                    {
                        filterContext.Result = new ContentResult() { Content = ApiResult<string>.Create(ResultState.Fail, null, $"SilmoonDevAppService is not configured.").ToJsonString(), ContentType = "application/json" };
                        return;
                    }
                }

                string Signature;
                string AppId;
                NameValueCollection nameValueCollection;

                if (filterContext.HttpContext.Request.Method == "POST")
                {
                    Signature = filterContext.HttpContext.Request.Form[SignFieldName];
                    AppId = filterContext.HttpContext.Request.Form[AppIdFieldName];
                    nameValueCollection = filterContext.HttpContext.Request.GetFormNameValues();
                }
                else
                {
                    Signature = filterContext.HttpContext.Request.Query[SignFieldName];
                    AppId = filterContext.HttpContext.Request.Query[AppIdFieldName];
                    nameValueCollection = filterContext.HttpContext.Request.GetQueryStringNameValues();
                }

                if (Require || !Signature.IsNullOrEmpty())
                {
                    var keyResult = await SilmoonDevAppService.GetCachedKey(AppId);
                    if (keyResult.State)
                    {
                        var signature = nameValueCollection.GetSign(KeyFieldName, keyResult.Data.SignatureKey, SignFieldName);
                        if (Signature != signature)
                        {
                            filterContext.Result = new ContentResult() { Content = ApiResult<string>.Create(ResultState.Fail, null, $"Sign error (Request AppId is {{{AppId}}}, request signature is {{{Signature}(error)}} ).").ToJsonString(), ContentType = "application/json" };
                        }
                        else
                        {
                            await base.OnActionExecutionAsync(filterContext, next);
                        }
                    }
                    else
                    {
                        filterContext.Result = new ContentResult() { Content = ApiResult<string>.Create(ResultState.Fail, null, $"GetKey(string AppId) fail, Message: {{{keyResult.Message}}}").ToJsonString(), ContentType = "application/json" };
                    }
                }
                else await base.OnActionExecutionAsync(filterContext, next);
            }
        }
    }
}
