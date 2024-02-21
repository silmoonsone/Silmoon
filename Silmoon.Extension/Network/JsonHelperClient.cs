using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Extension.Network
{
    [Obsolete]
    public class JsonHelperClient : WebClient
    {
        public int Timeout { get; set; } = 60000;
        public JsonHelperClient()
        {
            Timeout = 60000;
        }
        public JsonHelperClient(int timeout)
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            foreach (var item in JsonHelper.JsonHelperRequestHeaders)
            {
                if (address.Host == item.Key)
                {
                    foreach (var item2 in item.Value)
                    {
                        Headers.Add(item2.Key, item2.Value);
                    }
                }
            }

            var result = base.GetWebRequest(address);
            result.Timeout = Timeout;
            return result;
        }
    }
}
