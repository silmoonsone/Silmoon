using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Silmoon
{
    public class SDebug
    {
        public static string GetWebExceptionHtml(WebException exception)
        {
            var response = exception.Response;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string s = reader.ReadToEnd();
                return s;
            }
        }
    }
}
