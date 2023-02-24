using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Web.Mvc;
using Silmoon.Extension;

namespace Silmoon.Web
{
    public class HttpHelper
    {
        public static string UrlPathEncode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            int index = str.IndexOf('?');
            if (index >= 0)
            {
                return (UrlPathEncode(str.Substring(0, index), e) + str.Substring(index));
            }
            return UrlEncodeSpaces(UrlEncodeNonAscii(str, e));
        }
        /// <summary>
        /// 获取应用程序相对路径，含有根路径“/”，若根路径不是应用程序路径，最后不是以斜杠“/”结束，如/apps/myapp。
        /// </summary>
        public static string AppWebPath => HttpRuntime.AppDomainAppVirtualPath;
        /// <summary>
        /// 获取站点物理路径，后面以/结束。
        /// </summary>
        public static string AppLocalPath => HttpRuntime.AppDomainAppPath;
        /// <summary>
        /// 获取应用程序完整HTTP路径，含有根路径“/”，若根路径不是应用程序路径，最后不是以斜杠“/”结束，如https://www.silmoon.com/apps/myapp。
        /// </summary>
        /// <returns></returns>
        public static string GetWebAppRootUrl()
        {
            string http = "http";
            if (HttpContext.Current.Request.ServerVariables["HTTPS"] != null && HttpContext.Current.Request.ServerVariables["HTTPS"].ToBool())
                http = "https";

            http = http + "://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + AppWebPath;
            return http;
        }
        public static IPAddress GetClientIPAddress()
        {
            IPAddress result = null;

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-Forwarded-For"]))
                return IPAddress.Parse(HttpContext.Current.Request.Headers["X-Forwarded-For"].Split(new string[] { " ", ",", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Headers["CF-Connecting-IP"]))
                return IPAddress.Parse(HttpContext.Current.Request.Headers["CF-Connecting-IP"].Split(new string[] { " ", ",", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                return IPAddress.Parse(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new string[] { " ", ",", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]))
                return IPAddress.Parse(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].Split(new string[] { " ", ",", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);

            return result;
        }

        public static string ReturnShortURL()
        {
            return HttpContext.Current.Request.FilePath.ToString() + "?" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
        }


        static string UrlEncodeSpaces(string str)
        {
            if ((str != null) && (str.IndexOf(' ') >= 0))
            {
                str = str.Replace(" ", "%20");
            }
            return str;
        }
        static string UrlEncodeNonAscii(string str, Encoding e)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (e == null)
            {
                e = Encoding.UTF8;
            }
            byte[] bytes = e.GetBytes(str);
            bytes = UrlEncodeBytesToBytesInternalNonAscii(bytes, 0, bytes.Length, false);
            return Encoding.ASCII.GetString(bytes);
        }
        static byte[] UrlEncodeBytesToBytesInternalNonAscii(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                if (IsNonAsciiByte(bytes[offset + i]))
                {
                    num++;
                }
            }
            if (!alwaysCreateReturnValue && (num == 0))
            {
                return bytes;
            }
            byte[] buffer = new byte[count + (num * 2)];
            int num3 = 0;
            for (int j = 0; j < count; j++)
            {
                byte b = bytes[offset + j];
                if (IsNonAsciiByte(b))
                {
                    buffer[num3++] = 0x25;
                    buffer[num3++] = (byte)IntToHex((b >> 4) & 15);
                    buffer[num3++] = (byte)IntToHex(b & 15);
                }
                else
                {
                    buffer[num3++] = b;
                }
            }
            return buffer;
        }
        static bool IsNonAsciiByte(byte b)
        {
            if (b < 0x7f)
            {
                return (b < 0x20);
            }
            return true;
        }
        static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 0x30);
            }
            return (char)((n - 10) + 0x61);
        }

    }
}
