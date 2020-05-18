using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Silmoon.Web.Mvc
{
    public class MvcHelper
    {
        public static List<SelectListItem> MakeMvcSelectListItems(Type type, object enumObj)
        {
            if (type.IsEnum)
            {
                var names = type.GetEnumNames();
                var values = type.GetEnumValues();
                var result = new List<SelectListItem>();
                for (int i = 0; i < names.Length; i++)
                {
                    result.Add(new SelectListItem() { Text = names[i], Value = values.GetValue(i).ToString() });
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        public static string GetAjaxOptionJavascriptFunctions()
        {
            return js.ajaxOptionFunctions;
        }
        public static AjaxOptions GetAjaxOptions(string senderId = "", string onSuccess = "null", string onBegin = "null", string onFailed = "null", string onError = "null", string url = "", bool onSuccessNeedRefreshPage = false)
        {
            var result = new AjaxOptions()
            {
                OnBegin = "(function(sender, onBegin){ _ajax_on_begin(sender, onBegin); })('" + senderId + "', " + onBegin + ")",
                OnComplete = "(function(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage){ _ajax_on_complete(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage); })('" + senderId + "', arguments[0], " + onSuccess + ", " + onFailed + ", " + onError + ", " + onSuccessNeedRefreshPage.ToString().ToLower() + ")"
            };
            if (!string.IsNullOrEmpty(url))
            {
                result.Url = url;
            }
            return result;
        }
        public static AjaxOptions GetAjaxOptions(AjaxOptions ajaxOptions, string senderId = "", string onSuccess = "null", string onBegin = "null", string onFailed = "null", string onError = "null", bool onSuccessNeedRefreshPage = false)
        {
            ajaxOptions.OnBegin = "(function(sender, onBegin){ _ajax_on_begin(sender, onBegin); })('" + senderId + "', " + onBegin + ")";
            ajaxOptions.OnComplete = "(function(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage){ _ajax_on_complete(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage); })('" + senderId + "', arguments[0], " + onSuccess + ", " + onFailed + ", " + onError + ", " + onSuccessNeedRefreshPage.ToString().ToLower() + ")";

            return ajaxOptions;
        }

    }
}
