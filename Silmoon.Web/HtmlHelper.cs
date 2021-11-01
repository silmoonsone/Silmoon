using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Silmoon.Web
{
    public static class HtmlHelper
    {
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
        public static string MakeImage(string url, int width, int height)
        {
            return "<img src=\"" + url + "\" width=\"" + width + "\" height=\"" + height + "\" />";
        }
        public static string MakeImage(string url)
        {
            return "<img src=\"" + url + "\" />";
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




        /// <summary>
        /// 弹出一个消息框
        /// </summary>
        /// <param name="Message">要显示的消息</param>
        public static void MessageBox(string Message)
        {
            Message = Message.Replace("\r", "\\r");
            Message = Message.Replace("\n", "\\n");
            Message = Message.Replace("\"", "\"\"\"\"");
            HttpContext.Current.Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            HttpContext.Current.Response.Write("<script type=\"text/javascript\">alert('" + Message + "');</script>");
        }
        /// <summary>
        /// 使用脚本的方式转向到另一个URL
        /// </summary>
        /// <param name="URL">要转向的URL</param>
        public static void ScriptRedirect(string URL)
        {
            HttpContext.Current.Response.Write("<script type=\"text/javascript\">location.href='" + URL + "';</script>"); ;
        }
        /// <summary>
        /// 使用脚本打开一个新的IE窗口
        /// </summary>
        /// <param name="URL">要打开IE窗口的URL</param>
        public static void OpenWindow(string URL)
        {
            HttpContext.Current.Response.Write("<script type=\"text/javascript\">window.open('" + URL + "');</script>");
        }
        /// <summary>
        /// 刷新当前网页。
        /// </summary>
        public static void RefreshPage()
        {
            HttpContext.Current.Response.Write("<script type=\"text/javascript\">location.reload(location.href);</script>"); ;
        }
        /// <summary>
        /// 使用脚本关闭当前页面。
        /// </summary>
        public static void ClosePage()
        {
            HttpContext.Current.Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.opener=null;window.open('','_self');window.close();</script>");
        }

        public static void RefreshParentWindow()
        {
            HttpContext.Current.Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.opener.location.reload();</script>");
        }


        public static void MessageBox(this Page page, string Message)
        {
            Message = Message.Replace("\r", "\\r");
            Message = Message.Replace("\n", "\\n");
            Message = Message.Replace("\"", "\"\"\"\"");
            page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), new Random().Next().ToString(), "alert('" + Message + "');", true);
        }
        public static void ScriptRedirect(this Page page, string URL)
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), new Random().Next().ToString(), "location.href='" + URL + "';", true);
        }
        public static void OpenWindow(this Page page, string URL)
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), new Random().Next().ToString(), "window.open('" + URL + "');", true);
        }
        public static void RefreshPage(this Page page)
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), new Random().Next().ToString(), "location.reload(location.href);", true);
        }
        public static void ClosePage(this Page page)
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), new Random().Next().ToString(), "window.opener=null;window.open('','_self');\r\nwindow.close();", true);
        }
        public static void RefreshParentWindow(this Page page)
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), new Random().Next().ToString(), "window.opener.location.reload();", true);
        }


    }
}
