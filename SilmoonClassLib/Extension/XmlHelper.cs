using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Silmoon.Extension
{
    public static class XmlHelper
    {
        public static string ToXmlString(this XmlDocument xml)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                xml.Save(ms);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    string resp = sr.ReadToEnd();
                    return resp;
                }
            }
        }
    }
}
