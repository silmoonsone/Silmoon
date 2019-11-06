using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Xml;

namespace Silmoon.Extension.Json
{
    public static class XmlHelper
    {
        public static XmlDocument GetXml(string url, string data)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string s = wc.UploadString(url, data);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(s);
                return xml;
            }
        }
        public static XmlDocument GetXmlByPost(string url, NameValueCollection data)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                byte[] bytes = wc.UploadValues(url, data);
                string s = Encoding.UTF8.GetString(bytes);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(s);
                return xml;
            }
        }
    }
}
