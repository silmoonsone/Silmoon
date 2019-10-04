using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Extension.Php
{
    public class PhpFunction
    {
        public static string file_get_content(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string s = wc.DownloadString(url);
                return s;
            }
        }
    }
}
