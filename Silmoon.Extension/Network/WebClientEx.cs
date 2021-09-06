using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Extension.Network
{
    public class WebClientEx : WebClient
    {
        public int Timeout { get; set; } = 60000;
        public WebClientEx()
        {

        }
        public WebClientEx(int timeout)
        {
            Timeout = timeout;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var result = base.GetWebRequest(address);
            result.Timeout = Timeout;
            return result;
        }
    }
}
