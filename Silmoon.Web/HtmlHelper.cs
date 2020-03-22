using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Silmoon.Web
{
    public class HtmlHelper
    {
        /// <summary>
        /// 返回已经设计好的asp.net错误信息的HTML。
        /// </summary>
        /// <param name="s">信息</param>
        /// <returns></returns>
        public static string ErrorMsgHtml(Exception exception)
        {
            string msg = "<style type=\"text/css\"><!--";
            msg += "body,td,th {font-family: Verdana, Arial, Helvetica, sans-serif;	font-size: 12px;}";
            msg += "--></style>";
            msg += "<p style=\"background:#CCCCCC\"><strong>" + exception.Message + "：</strong></p>";
            msg += "<pre>" + exception.ToString() + "</pre>";
            return msg;
        }
        public static string ErrorMsgHtml(string ErrTitle, string ErrText)
        {
            string msg = "<style type=\"text/css\"><!--";
            msg += "body,td,th {font-family: Verdana, Arial, Helvetica, sans-serif;	font-size: 12px;}";
            msg += "--></style>";
            msg += "<p style=\"background:#CCCCCC\"><strong>" + ErrTitle + "：</strong></p>";
            msg += "<p>" + ErrText + "</p>";
            return msg;
        }
        public static string ErrorMsgHtml(string ErrTitle, string ErrText, string ErrTrace)
        {
            string msg = "<style type=\"text/css\"><!--";
            msg += "body,td,th {font-family: Verdana, Arial, Helvetica, sans-serif;	font-size: 12px;}";
            msg += "--></style>";
            msg += "<p style=\"background:#CCCCCC\"><strong>" + ErrTitle + "：</strong></p>";
            msg += "<p>" + ErrText + "</p>";
            msg += "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#000000\" bgcolor=\"#CCCCCC\">";
            msg += "<tr><td>跟踪：<br /><pre>" + ErrTrace + "</pre></td></tr></table>";
            return msg;
        }
        public static string ErrorMsgHtml(string ErrTitle, string ErrText, string ErrSrc, string ErrTrace)
        {
            string msg = "<style type=\"text/css\"><!--";
            msg += "body,td,th {font-family: Verdana, Arial, Helvetica, sans-serif;	font-size: 12px;}";
            msg += "--></style>";
            msg += "<p style=\"background:#CCCCCC\"><strong>" + ErrTitle + "：</strong></p>";
            msg += "<p>" + ErrText + "</p>";
            msg += "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#000000\" bgcolor=\"#CCCCCC\">";
            msg += "<tr><td>源：<br />" + ErrSrc + "</td></tr></table>";
            msg += "<br /><table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#000000\" bgcolor=\"#CCCCCC\">";
            msg += "<tr><td>跟踪：<br /><pre>" + ErrTrace + "</pre></td></tr></table>";
            return msg;
        }
        /// <summary>
        /// 返回已经设计好的A标签。
        /// </summary>
        /// <param name="text">显示的文字</param>
        /// <param name="url">导航的URL</param>
        /// <param name="target">目标</param>
        /// <returns></returns>
        public static string MakeAMark(string text, string url, string target)
        {
            return "<a href=\"" + url + "\" target=\"" + target + "\">" + text + "</a>";
        }
        /// <summary>
        /// 返回已经设计好的A标签。
        /// </summary>
        /// <param name="text">显示的文字</param>
        /// <param name="url">导航的URL</param>
        /// <param name="target">目标</param>
        /// <param name="alt">Tip文字</param>
        /// <returns></returns>
        public static string MakeAMark(string text, string url, string target, string alt)
        {
            return "<a href=\"" + url + "\" target=\"" + target + "\" alt=\"" + alt + "\">" + text + "</a>";
        }
        /// <summary>
        /// 返回已经设计好的A标签。
        /// </summary>
        /// <param name="text">显示的文字</param>
        /// <param name="url">导航的URL</param>
        /// <returns></returns>
        public static string MakeAMark(string text, string url)
        {
            return "<a href=\"" + url + "\">" + text + "</a>";
        }
        public static string MakeImgMark(string imgUrl, string linkTo, string target)
        {
            return MakeAMark("<img border=\"0\" src=\"" + imgUrl + "\"></img>", linkTo, target);
        }

        /// <summary>
        /// 解码加密的HTML代码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecodeHTMLFilter(string s)
        {
            s = s.Replace("{//LeftMark}", "<");
            s = s.Replace("{//RightMark}", ">");
            s = s.Replace("''", "'");
            s = s.Replace("{//_}", " ");
            return s;
        }
        /// <summary>
        /// 加密HTML代码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeHTMLFilter(string s)
        {
            s = s.Replace("<", "{//LeftMark}");
            s = s.Replace(">", "{//RightMark}");
            s = s.Replace("'", "''");
            s = s.Replace(" ", "{//_}");
            return s;
        }
        /// <summary>
        /// 将Textarea中的HTML写入数据库的前置处理
        /// </summary>
        /// <param name="html">HTML</param>
        /// <returns></returns>
        public static string TextareaInHTML(string html)
        {
            html = html.Replace(" ", "&nbsp;");
            html = html.Replace("\r\n", "<br />");
            return html;
        }
        /// <summary>
        /// 将HTML回写到Textarea的前置处理
        /// </summary>
        /// <param name="html">HTML</param>
        /// <returns></returns>
        public static string TextareaOutHTML(string html)
        {
            html = html.Replace("&nbsp;", " ");
            html = html.Replace("<br />", "\r\n");
            return html;
        }
        public static string MakePostFormHtml(NameValueCollection values, string postTo, string name, bool submit = false)
        {
            string result = "<form method=\"post\" action=\"" + postTo + "\" name=\"" + name + "\" id=\"" + name + "\">\r\n";
            for (int i = 0; i < values.Count; i++)
            {
                result += "<input type='hidden' name='" + values.GetKey(i) + "' value='" + values[i] + "'/>\r\n";
            }
            result += "</form>\r\n";
            if (submit) result += "<script>document.forms['" + name + "'].submit();</script>";
            return result;
        }
        public static string MakeNewQueryString(NameValueCollection collection, string additionQueryString = "")
        {
            collection = new NameValueCollection(collection);
            string s = "";
            var tp = StringHelper.AnalyzeNameValue(additionQueryString.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries), "=");

            for (int i = 0; i < tp.Count; i++)
            {
                string key = tp.GetKey(i);
                string value = tp[i];

                collection[key] = value;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                string key = collection.GetKey(i);
                string value = collection[i];

                s += $"{key}={value}&";
            }
            if (s != "")
            {
                s = s.Remove(s.Length - 1);
            }
            //if (s[0] != '?')
            //    s = "?" + s;

            return s;
        }


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
        public static AjaxOptions GetAjaxOptions(string senderId, string onSuccess = "null", string onFailed = "null", string onError = "null")
        {
            var result = new AjaxOptions()
            {
                OnBegin = "(function(sender){ _ajax_on_begin(sender); })('" + senderId + "')",
                OnComplete = "(function(senderId, e, onSuccess, onFailed, onError){ _ajax_on_complete(senderId, e, onSuccess, onFailed, onError); })('" + senderId + "', arguments[0], " + onSuccess + ", " + onFailed + ", " + onError + ")"
            };
            return result;
        }
    }
}
