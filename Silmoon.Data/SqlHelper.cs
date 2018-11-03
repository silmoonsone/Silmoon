using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Silmoon.Data
{
    public class SqlHelper
    {
        public static string SqlInjectStringFilter(string srcString)
        {
            string s = srcString.Replace("'", "''");
            return s;
        }
        public static string SqlInjectStringFilter(string srcString, bool checkempty)
        {
            if (srcString == "" && checkempty)
            {
                throw new Exception("参数srcString不能为空");
            }
            string s = srcString.Replace("'", "''");
            return s;
        }
        public static string SqlInjectHtmlStringFilter(string srcString)
        {
            string s = srcString.Replace("'", "''");
            s = s.Replace("\"", "\"\"");
            return s;
        }
        public static string SqlReadHtmlStringFilted(string srcString)
        {
            string s = srcString.Replace("''", "''");
            s = s.Replace("\"\"", "\"");
            return s;
        }

        public static T MakeObject<T>(DataRow row, T obj)
        {
            var propertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo item in propertyInfos)
            {
                string name = item.Name;
                Type type = item.PropertyType;
                if (row[name] != DBNull.Value)
                {
                    item.SetValue(obj, row[name], null);
                }
            }

            return obj;
        }

    }
}
