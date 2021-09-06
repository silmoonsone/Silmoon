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
    public static class JsonHelper
    {
        public static int WebClientTime { get; set; }
        public static JObject GetJson(string url)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<JObject> GetJsonAsync(string url)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.DownloadStringTaskAsync(url);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static JArray GetJsons(string url)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                JArray jo = (JArray)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<JArray> GetJsonsAsync(string url)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.DownloadStringTaskAsync(url);
                JArray jo = (JArray)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static JObject GetJsonByPost(string url, string data)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.UploadString(url, data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<JObject> GetJsonByPostAsync(string url, string data)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.UploadStringTaskAsync(url, data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static JObject GetJsonByPost(string url, NameValueCollection values)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
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
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                var data = await wc.UploadValuesTaskAsync(url, values);
                var s = Encoding.UTF8.GetString(data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }


        public static object GetJsonObject(string url)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                var jo = JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public async static Task<object> GetJsonObjectAsync(string url)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.DownloadStringTaskAsync(url);
                var jo = JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static T GetJsonObject<T>(string url)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public async static Task<T> GetJsonObjectAsync<T>(string url)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.DownloadStringTaskAsync(url);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }

        public static T GetJsonObjectByPost<T>(string url, string data)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = wc.UploadString(url, data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public async static Task<T> GetJsonObjectByPostAsync<T>(string url, string data)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                string s = await wc.UploadStringTaskAsync(url, data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }

        public static T GetJsonObjectByPost<T>(string url, NameValueCollection values)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                var data = wc.UploadValues(url, values);
                var s = Encoding.UTF8.GetString(data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public async static Task<T> GetJsonObjectByPostAsync<T>(string url, NameValueCollection values)
        {
            using (WebClientEx wc = new WebClientEx(WebClientTime))
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.UserAgent] = "Silmoon.Extension_JsonHelper/1.0";
                wc.Encoding = Encoding.UTF8;
                var data = await wc.UploadValuesTaskAsync(url, values);
                var s = Encoding.UTF8.GetString(data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }




        public static JObject LoadJsonFromFile(string path)
        {
            string s = File.ReadAllText(path);
            JObject jo = (JObject)JsonConvert.DeserializeObject(s);
            return jo;
        }
        public static T LoadJsonFromFile<T>(string path)
        {
            string s = File.ReadAllText(path);
            T jo = JsonConvert.DeserializeObject<T>(s);
            return jo;
        }


    }
}
