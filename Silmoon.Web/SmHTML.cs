using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Web
{
    public class SmHTML
    {
        /// <summary>
        /// 返回已经设计好的asp.net错误信息的HTML。
        /// </summary>
        /// <param name="s">信息</param>
        /// <returns></returns>
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
        public static string PrintMark_A(string text, string url, string target)
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
        public static string PrintMark_A(string text, string url, string target, string alt)
        {
            return "<a href=\"" + url + "\" target=\"" + target + "\" alt=\"" + alt + "\">" + text + "</a>";
        }
        /// <summary>
        /// 返回已经设计好的A标签。
        /// </summary>
        /// <param name="text">显示的文字</param>
        /// <param name="url">导航的URL</param>
        /// <returns></returns>
        public static string PrintMark_A(string text, string url)
        {
            return "<a href=\"" + url + "\">" + text + "</a>";
        }
        public static string PrintMark_Img(string imgUrl, string linkTo, string target)
        {
            return PrintMark_A("<img border=\"0\" src=\"" + imgUrl + "\"></img>", linkTo, target);
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
    }
}
