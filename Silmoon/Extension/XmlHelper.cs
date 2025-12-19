using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace Silmoon.Extension
{
    public static class XmlHelper
    {
        public static XmlDocument GetXml(string url, string data)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                string s = response.Content.ReadAsStringAsync().Result;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(s);
                return xml;
            }
        }
        public static XmlDocument GetXmlByPost(string url, NameValueCollection data)
        {
            using (HttpClient client = new HttpClient())
            {
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                foreach (string key in data.Keys)
                {
                    list.Add(new KeyValuePair<string, string>(key, data[key]));
                }
                HttpContent content = new FormUrlEncodedContent(list);
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                string s = response.Content.ReadAsStringAsync().Result;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(s);
                return xml;
            }
        }
    }
}
