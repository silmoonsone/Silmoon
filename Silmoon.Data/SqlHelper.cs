using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data
{
    public class SqlOption
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
    }
}
