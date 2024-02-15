using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Extension.Network;
using Silmoon.Models;
using Silmoon.Runtime.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Silmoon.Extension
{
    public static class JsonHelperV2
    {
        public static DictionaryEx<string, Dictionary<string, string>> JsonHelperV2RequestHeaders { get; set; } = new DictionaryEx<string, Dictionary<string, string>>();
        public static void RegisterHostHeader(string Host, string HeaderName, string HeaderValue)
        {
            var hostHeaders = JsonHelperV2RequestHeaders.Get(Host);
            if (hostHeaders is null)
            {
                hostHeaders = new Dictionary<string, string>();
                JsonHelperV2RequestHeaders.Add(Host, hostHeaders);
            }

            hostHeaders[HeaderName] = HeaderValue;
        }
        public static void UnregisterHostHeaders(string Host)
        {
            if (Host.IsNullOrEmpty())
                JsonHelperV2RequestHeaders.Clear();
            else
                JsonHelperV2RequestHeaders.Remove(Host);
        }

        public static TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);

        static HttpRequestMessage getBaseRequest(string url, HttpMethod httpMethod)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, url);
            request.Headers.Add("User-Agent", "Silmoon.Extension_JsonHelper/1.0");
            foreach (var host in JsonHelperV2RequestHeaders)
            {
                if (url.Contains(host.Key))
                {
                    foreach (var header in host.Value)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
            }
            return request;
        }

        public async static Task<StateSet<bool, T>> GetObjectAsync<T>(string url)
        {
            using (HttpClientEx client = new HttpClientEx(RequestTimeout))
            {
                var request = getBaseRequest(url, HttpMethod.Get);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    T jo = JsonConvert.DeserializeObject<T>(s);
                    return StateSet<bool, T>.Create(true, jo);
                }
                else
                {
                    return StateSet<bool, T>.Create(false, default(T), "IsSuccessStatusCode is false.");
                }
            }
        }

        public async static Task<StateSet<bool, ResultT>> PostObjectAsync<PostObjectT, ResultT>(string url, PostObjectT jsonData)
        {
            using (HttpClientEx client = new HttpClientEx(RequestTimeout))
            {
                var request = getBaseRequest(url, HttpMethod.Post);
                request.Content = new StringContent(jsonData.ToJsonString(), Encoding.UTF8, "application/json"); // 传递 JSON 数据
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ResultT jo = JsonConvert.DeserializeObject<ResultT>(s);
                    return StateSet<bool, ResultT>.Create(true, jo);
                }
                else
                {
                    return StateSet<bool, ResultT>.Create(false, default(ResultT), "IsSuccessStatusCode is false.");
                }
            }
        }

        public async static Task<StateSet<bool, T>> PostFormDataAsync<T>(string url, UrlDataCollection urlDataCollection)
        {
            using (HttpClientEx client = new HttpClientEx(RequestTimeout))
            {
                var request = getBaseRequest(url, HttpMethod.Post);
                request.Content = new FormUrlEncodedContent(urlDataCollection.GetKeyValuePairs());
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    T jo = JsonConvert.DeserializeObject<T>(s);
                    return StateSet<bool, T>.Create(true, jo);
                }
                else
                {
                    return StateSet<bool, T>.Create(false, default(T), "IsSuccessStatusCode is false.");
                }
            }
        }

        public async static Task<StateSet<bool, T>> PostMultipartFormDataAsync<T>(string url, UrlDataCollection urlDataCollection)
        {
            using (HttpClientEx client = new HttpClientEx(RequestTimeout))
            {
                var request = getBaseRequest(url, HttpMethod.Post);
                request.Content = new FormUrlEncodedContent(urlDataCollection.GetKeyValuePairs());
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    T jo = JsonConvert.DeserializeObject<T>(s);
                    return StateSet<bool, T>.Create(true, jo);
                }
                else
                {
                    return StateSet<bool, T>.Create(false, default(T), "IsSuccessStatusCode is false.");
                }
            }
        }



        public static JObject LoadJsonFromFile(string path)
        {
            string s = File.ReadAllText(path);
            JObject jo = (JObject)JsonConvert.DeserializeObject(s);
            return jo;
        }
        public static T LoadJsonFromFile<T>(string path)
        {
            string s = File.ReadAllText(path);
            T jo = JsonConvert.DeserializeObject<T>(s);
            return jo;
        }


    }
}
