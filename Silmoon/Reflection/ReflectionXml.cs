using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Silmoon.Reflection
{
    [Obsolete("类型将被删除", true)]
    public class ReflectionXml
    {
        [Obsolete("方法将被删除", true)]
        public static object LoadFile(Type type, string path)
        {
            FileStream fs = null;
            object result = null;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                result = serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return result;
        }
        [Obsolete("方法将被删除", true)]
        public static void SaveFile(string path, object obj)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                { fs.Close(); }
            }
        }
    }

}
