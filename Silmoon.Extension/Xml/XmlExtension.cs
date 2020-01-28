using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Silmoon.Extension.Xml
{
    public static class XmlExtension
    {
        public static string ToXmlString(this XmlDocument xmlDocument)
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

    }
}
