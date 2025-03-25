using Newtonsoft.Json;
using Silmoon.Extension;
using Silmoon.Extension.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.SdkApi.Warpper.Bark
{
    public class Bark
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public Bark(string url, string key)
        {
            Url = url;
            Key = key;
        }
        public async Task<JsonRequestResult<BarkResponse>> Send(BarkRequest request)
        {
            var url = $"{Url}/{Key}";
            var result = await JsonRequest.SendAsyncWithBody<BarkResponse, BarkRequest>(url, HttpMethod.Post, request, null);
            return result;
        }
        public static async Task<JsonRequestResult<BarkResponse>> Send(string url, string key, BarkRequest request)
        {
            url = $"{url.TrimEnd('/')}/{key}";
            var result = await JsonRequest.SendAsyncWithBody<BarkResponse, BarkRequest>(url, HttpMethod.Post, request, null, new JsonRequestSetting() { JsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore } });
            return result;
        }
    }
}
