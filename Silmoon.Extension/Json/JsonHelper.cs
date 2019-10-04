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
        public static JObject FromUrlGetJsonObjectByGet(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                JObject jo = (JObject)JsonConvert.DeserializeObject(s);
                return jo;
            }
        }

        public static T FromUrlGetObjectByGet<T>(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                T jo = JsonConvert.DeserializeObject<T>(s);
                return jo;
            }
        }

    }
}
