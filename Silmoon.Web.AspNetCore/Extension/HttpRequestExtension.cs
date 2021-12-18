using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Extension;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Silmoon.Web.AspNetCore.Extension
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
    }
}
