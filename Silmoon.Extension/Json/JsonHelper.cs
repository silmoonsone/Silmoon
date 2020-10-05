using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Xml;

namespace Silmoon.Extension.Json
{
    public static class JsonHelper
    {
        public static JObject GetJson(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public static JObject GetJsonByPost(string url, string data)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string s = wc.UploadString(url, data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }
        public static JObject GetJsonByPost(string url, NameValueCollection values)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var data = wc.UploadValues(url, values);
                var s = Encoding.UTF8.GetString(data);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static T GetJsonObject<T>(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public static T GetJsonObjectByPost<T>(string url, string data)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string s = wc.UploadString(url, data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
        public static T GetJsonObjectByPost<T>(string url, NameValueCollection values)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var data = wc.UploadValues(url, values);
                var s = Encoding.UTF8.GetString(data);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }
    }
}
