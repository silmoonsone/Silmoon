using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Extension.Network;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Silmoon.Extension
{
    [Obsolete("Use JsonHelperV2 instead")]
    public static class JsonHelper
    {
        public static Dictionary<string, Dictionary<string, string>> JsonHelperRequestHeaders { get; set; } = new Dictionary<string, Dictionary<string, string>>();
        public static void RegisterHostHeader(string Host, string HeaderName, string HeaderValue)
        {
            var hostHeaders = JsonHelperRequestHeaders.Get(Host);
            if (hostHeaders is null)
            {
                hostHeaders = new Dictionary<string, string>();
                JsonHelperRequestHeaders.Add(Host, hostHeaders);
            }

            hostHeaders[HeaderName] = HeaderValue;
        }
        public static void UnregisterHostHeaders(string Host)
        {
            if (Host.IsNullOrEmpty())
                JsonHelperRequestHeaders.Clear();
            else
                JsonHelperRequestHeaders.Remove(Host);
        }

        public static int RequestTimeout { get; set; } = 30000;
        public static JObject GetJson(string url)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<JObject> GetJsonAsync(string url)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.DownloadStringTaskAsync(url);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static JArray GetJsons(string url)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                JArray jo = (JArray)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<JArray> GetJsonsAsync(string url)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.DownloadStringTaskAsync(url);
                JArray jo = (JArray)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static JObject GetJsonByPost(string url, string data, bool jsonContent = false)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                if (jsonContent) wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.UploadString(url, data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<JObject> GetJsonByPostAsync(string url, string data, bool jsonContent = false)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                if (jsonContent) wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.UploadStringTaskAsync(url, data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static JObject GetJsonByPost(string url, NameValueCollection values)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                var data = wc.UploadValues(url, values);
                var s = Encoding.UTF8.GetString(data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<JObject> GetJsonByPostAsync(string url, NameValueCollection values)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                var data = await wc.UploadValuesTaskAsync(url, values);
                var s = Encoding.UTF8.GetString(data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }


        public static object GetObject(string url)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                var jo = JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<object> GetObjectAsync(string url)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.DownloadStringTaskAsync(url);
                var jo = JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static T GetObject<T>(string url)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public async static Task<T> GetObjectAsync<T>(string url)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.DownloadStringTaskAsync(url);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }

        public static T GetObjectByPost<T>(string url, string data, bool jsonContent = false)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                if (jsonContent) wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.UploadString(url, data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public async static Task<T> GetObjectByPostAsync<T>(string url, string data, bool jsonContent = false)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                if (jsonContent) wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.UploadStringTaskAsync(url, data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }

        public static T GetObjectByPost<T>(string url, NameValueCollection values)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                var data = wc.UploadValues(url, values);
                var s = Encoding.UTF8.GetString(data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public async static Task<T> GetObjectByPostAsync<T>(string url, NameValueCollection values)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                var data = await wc.UploadValuesTaskAsync(url, values);
                var s = Encoding.UTF8.GetString(data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public async static Task<T> GetObjectByPostAsync<T>(string url, UrlDataCollection values)
        {
            using (JsonHelperClient wc = new JsonHelperClient(RequestTimeout))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Encoding = Encoding.UTF8;
                var data = await wc.UploadStringTaskAsync(url, values.ToQueryString());
                T jo = JsonConvert.DeserializeObject<T>(data);
                return jo;
            }
        }




        public static JObject LoadJsonFromFile(string path) => JsonHelperV2.LoadJsonFromFile<JObject>(path);
        public static T LoadJsonFromFile<T>(string path) => JsonHelperV2.LoadJsonFromFile<T>(path);


    }
}
