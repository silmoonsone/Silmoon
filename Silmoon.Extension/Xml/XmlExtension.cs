using Silmoon.Extension.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Silmoon.Extension.Xml
{
    public static class XmlExtension
    {
        public static string GetXmlString(this XmlDocument xmlDocument)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                xmlDocument.Save(ms);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    string resp = sr.ReadToEnd();
                    return resp;
                }
            }
        }
        public static bool LoadXmlWithoutException(this XmlDocument xmlDocument, string xml)
        {
            try
            {
                xmlDocument.LoadXml(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static (XmlDocument Document, string XmlString) ToXml(this object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    var document = new XmlDocument();
                    var s = reader.ReadToEnd();
                    document.LoadXml(s);
                    return (document, s);
                }
            }
        }
        public static T ToObject<T>(this XmlDocument document)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream);
                stream.Position = 0;
                return (T)serializer.Deserialize(stream);
            }
        }

    }
}
