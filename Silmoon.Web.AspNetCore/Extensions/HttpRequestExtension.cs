using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Silmoon.Web.AspNetCore.Extensions
{
    public static class HttpRequestExtension
    {
        public static string Url { get; set; }
        public static string GetRawUrl(this HttpRequest httpRequest)
        {
            return httpRequest.GetEncodedUrl();
        }
        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!(httpRequest.Query["X-Requested-With"] == "XMLHttpRequest"))
            {
                if (httpRequest.Headers != null)
                {
                    return httpRequest.Headers["X-Requested-With"] == "XMLHttpRequest";
                }

                return false;
            }

            return true;
        }
        public static string UserAgent(this HttpRequest httpRequest)
        {
            return httpRequest.Headers["User-Agent"];
        }
        public static JObject ReadToJson(this HttpRequest request)
        {
            var requestBody = request.Body.MakeToString();
            return JsonConvert.DeserializeObject<JObject>(requestBody);

        }
        public static XmlDocument ReadToXml(this HttpRequest request)
        {
            var requestBody = request.Body.MakeToString();
            XmlDocument xml = new XmlDocument();
            xml.LoadXmlWithoutException(requestBody);
            return xml;
        }
        public static bool IsMobileDevice(this HttpRequest request)
        {
            string[] mobileAgents = { "iphone", "android", "phone", "mobile", "wap", "netfront", "java", "opera mobi", "opera mini", "ucweb", "windows ce", "symbian", "series", "webos", "sony", "blackberry", "dopod", "nokia", "samsung", "palmsource", "xda", "pieplus", "meizu", "midp", "cldc", "motorola", "foma", "docomo", "up.browser", "up.link", "blazer", "helio", "hosin", "huawei", "novarra", "coolpad", "webos", "techfaith", "palmsource", "alcatel", "amoi", "ktouch", "nexian", "ericsson", "philips", "sagem", "wellcom", "bunjalloo", "maui", "smartphone", "iemobile", "spice", "bird", "zte-", "longcos", "pantech", "gionee", "portalmmm", "jig browser", "hiptop", "benq", "haier", "^lct", "320x320", "240x320", "176x220", "w3c ", "acs-", "alav", "alca", "amoi", "audi", "avan", "benq", "bird", "blac", "blaz", "brew", "cell", "cldc", "cmd-", "dang", "doco", "eric", "hipt", "inno", "ipaq", "java", "jigs", "kddi", "keji", "leno", "lg-c", "lg-d", "lg-g", "lge-", "maui", "maxo", "midp", "mits", "mmef", "mobi", "mot-", "moto", "mwbp", "nec-", "newt", "noki", "oper", "palm", "pana", "pant", "phil", "play", "port", "prox", "qwap", "sage", "sams", "sany", "sch-", "sec-", "send", "seri", "sgh-", "shar", "sie-", "siem", "smal", "smar", "sony", "sph-", "symb", "t-mo", "teli", "tim-", "tosh", "tsm-", "upg1", "upsi", "vk-v", "voda", "wap-", "wapa", "wapi", "wapp", "wapr", "webc", "winw", "winw", "xda", "xda-", "googlebot-mobile" };

            bool isMoblie = false;

            string userAgent = request.UserAgent().ToLower();

            //排除 Windows 桌面系统或苹果桌面系统 
            if (!string.IsNullOrEmpty(userAgent) && !userAgent.Contains("macintosh") && (!userAgent.Contains("windows nt") || (userAgent.Contains("windows nt") && userAgent.Contains("compatible; msie 9.0;"))))
            {
                for (int i = 0; i < mobileAgents.Length; i++)
                {
                    if (userAgent.ToLower().IndexOf(mobileAgents[i]) >= 0)
                    {
                        isMoblie = true;
                        break;
                    }
                }
            }

            return isMoblie;
        }
        public static NameValueCollection GetQueryStringNameValues(this HttpRequest request)
        {
            var queries = request.Query;
            //var forms = request.Form;

            NameValueCollection nameValueCollection = new NameValueCollection();

            foreach (var item in queries)
            {
                nameValueCollection.Add(item.Key, item.Value);
            }
            //foreach (var item in forms)
            //{
            //    nameValueCollection.Add(item.Key, item.Value);
            //}

            return nameValueCollection;
        }
    }
}
