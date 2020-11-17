using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

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
        public static string GetAjaxOptionJavascriptFunctionsV1()
        {
            return js.ajaxOptionFunctionsV1;
        }
        public static string GetAjaxOptionJavascriptFunctionsV2()
        {
            return js.ajaxOptionFunctionsV2;
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
        public static AjaxOptions GetAjaxOptionsV1(string senderId = "", string onCompleted = "null", string onBegin = "null", string onError = "null", string url = "")
        {
            var result = new AjaxOptions()
            {
                OnBegin = "(function(sender, onBegin){ _ajax_on_begin_v1(sender, onBegin); })('" + senderId + "', " + onBegin + ")",
                OnComplete = "(function(senderId, e, onCompleted, onError){ _ajax_on_complete_v1(senderId, e, onCompleted, onError); })('" + senderId + "', arguments[0], " + onCompleted + ", " + onError + ")"
            };
            if (!string.IsNullOrEmpty(url))
            {
                result.Url = url;
            }
            return result;
        }
        public static AjaxOptions GetAjaxOptionsV2(string onCompleted = "null", string onBegin = "null", string onError = "null")
        {
            var result = new AjaxOptions()
            {
                OnBegin = "(function(e, onBegin){ _ajax_on_begin_v2(e, onBegin); })(arguments[0], " + onBegin + ")",
                OnComplete = "(function(e, onCompleted, onError){ _ajax_on_complete_v2(e, onCompleted, onError); })(arguments[0], " + onCompleted + ", " + onError + ")"
            };
            return result;
        }

        public static AjaxOptions GetAjaxOptions(AjaxOptions ajaxOptions, string senderId = "", string onSuccess = "null", string onBegin = "null", string onFailed = "null", string onError = "null", bool onSuccessNeedRefreshPage = false)
        {
            ajaxOptions.OnBegin = "(function(sender, onBegin){ _ajax_on_begin(sender, onBegin); })('" + senderId + "', " + onBegin + ")";
            ajaxOptions.OnComplete = "(function(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage){ _ajax_on_complete(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage); })('" + senderId + "', arguments[0], " + onSuccess + ", " + onFailed + ", " + onError + ", " + onSuccessNeedRefreshPage.ToString().ToLower() + ")";

            return ajaxOptions;
        }


        public static RouteValueDictionary MakeNewRouteValue(NameValueCollection collection, string additionQueryString = "")
        {
            RouteValueDictionary result = new RouteValueDictionary();

            collection = new NameValueCollection(collection);
            var tp = StringHelper.AnalyzeNameValue(additionQueryString.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries), "=");

            for (int i = 0; i < tp.Count; i++)
            {
                string key = tp.GetKey(i);
                string value = tp[i];

                collection[key] = value;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection.GetKey(i) == null) continue;
                string key = collection.GetKey(i);
                string value = collection[i];

                result.Add(key, value);
            }

            return result;
        }

    }
}
