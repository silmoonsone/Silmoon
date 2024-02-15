using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Silmoon.Extension.Network
{
    public class HttpClientEx : HttpClient
    {
        public HttpClientEx()
        {
            Timeout = TimeSpan.FromSeconds(30);
        }
        public HttpClientEx(TimeSpan timeout)
        {
            Timeout = timeout;
        }

    }
}
