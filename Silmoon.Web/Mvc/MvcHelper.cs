using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Silmoon.Web.Mvc
{
    public class MvcHelper
    {
        [Obsolete]
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
            if (senderId is null) senderId = "";
            if (onSuccess is null) onSuccess = "null";
            if (onBegin is null) onBegin = "null";
            if (onFailed is null) onFailed = "null";
            if (onError is null) onError = "null";
            if (url is null) url = "";

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
            if (senderId is null) senderId = "";
            if (onSuccess is null) onSuccess = "null";
            if (onBegin is null) onBegin = "null";
            if (onFailed is null) onFailed = "null";
            if (onError is null) onError = "null";

            ajaxOptions.OnBegin = "(function(sender, onBegin){ _ajax_on_begin(sender, onBegin); })('" + senderId + "', " + onBegin + ")";
            ajaxOptions.OnComplete = "(function(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage){ _ajax_on_complete(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage); })('" + senderId + "', arguments[0], " + onSuccess + ", " + onFailed + ", " + onError + ", " + onSuccessNeedRefreshPage.ToString().ToLower() + ")";

            return ajaxOptions;
        }
        public static AjaxOptions GetAjaxOptionsV1(string senderId = "", string onCompleted = "null", string onBegin = "null", string onError = "null", string flag = "", string url = "")
        {
            return GetAjaxOptionsV1(new AjaxOptions(), senderId, onCompleted, onBegin, onError, flag, url);
        }
        public static AjaxOptions GetAjaxOptionsV1(AjaxOptions ajaxOptions, string senderId = "", string onCompleted = "null", string onBegin = "null", string onError = "null", string flag = "", string url = "")
        {
            if (senderId is null) senderId = "";
            if (onCompleted is null) onCompleted = "null";
            if (onBegin is null) onBegin = "null";
            if (onError is null) onError = "null";
            if (flag is null) flag = "null";
            if (url is null) url = "";

            ajaxOptions.OnBegin = "(function(sender, onBegin, flag){ _ajax_on_begin_v1(sender, onBegin, flag); })('" + senderId + "', " + onBegin + ", '" + flag + "')";
            ajaxOptions.OnComplete = "(function(senderId, e, onCompleted, onError, flag){ _ajax_on_complete_v1(senderId, e, onCompleted, onError, flag); })('" + senderId + "', arguments[0], " + onCompleted + ", " + onError + ", '" + flag + "')";
            if (!string.IsNullOrEmpty(url))
            {
                ajaxOptions.Url = url;
            }
            return ajaxOptions;
        }
        public static AjaxOptions GetAjaxOptionsV2(string onCompleted = "null", string onBegin = "null", string onError = "null", string flag = "")
        {
            return GetAjaxOptionsV2(new AjaxOptions(), onCompleted, onBegin, onError, flag);
        }
        public static AjaxOptions GetAjaxOptionsV2(AjaxOptions ajaxOptions, string onCompleted = "null", string onBegin = "null", string onError = "null", string flag = "")
        {
            if (onCompleted is null) onCompleted = "null";
            if (onBegin is null) onBegin = "null";
            if (onError is null) onError = "null";

            ajaxOptions.OnBegin = "(function(e, onBegin){ _ajax_on_begin_v2(e, onBegin, flag); })(arguments[0], " + onBegin + ", '" + flag + "')";
            ajaxOptions.OnComplete = "(function(e, onCompleted, onError, flag){ _ajax_on_complete_v2(e, onCompleted, onError, flag); })(arguments[0], " + onCompleted + ", " + onError + ", '" + flag + "')";
            return ajaxOptions;
        }



        public static RouteValueDictionary MakeNewRouteValue(NameValueCollection collection, string additionQueryString = "")
        {
            RouteValueDictionary result = new RouteValueDictionary();

            collection = new NameValueCollection(collection);
            var nameValueCollection = HttpUtility.ParseQueryString(additionQueryString);
            //var tp = StringHelper.AnalyzeNameValue(additionQueryString.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries), "=");

            for (int i = 0; i < nameValueCollection.Count; i++)
            {
                string key = nameValueCollection.GetKey(i);
                string value = nameValueCollection[i];

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
