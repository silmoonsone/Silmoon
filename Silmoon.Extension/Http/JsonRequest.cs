using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Collections;
using Silmoon.Compress;
using Silmoon.Extension.Network;
using Silmoon.Models;

namespace Silmoon.Extension.Http
{
    public class JsonRequest
    {
        public static Dictionary<string, Dictionary<string, string[]>> DefaultRequestHostHeaders { get; set; } = new Dictionary<string, Dictionary<string, string[]>>();
        static Func<HttpClientEx> InvokeCreateHttpClient;
        public static void OnRequestCreateHttpClient(Func<HttpClientEx> requestHttpClientAction) => InvokeCreateHttpClient = requestHttpClientAction;

        public static HttpClient CreateHttpClient(JsonRequestSetting jsonRequestSetting)
        {
            if (jsonRequestSetting is null) jsonRequestSetting = JsonRequestSetting.Default;
            var httpClient = InvokeCreateHttpClient?.Invoke();
            if (httpClient is null)
            {
                if (jsonRequestSetting.IgnoreCertificateValidation)
                {
                    httpClient = new HttpClientEx(jsonRequestSetting.RequestTimeout, new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    });
                }
                else
                    httpClient = new HttpClientEx(jsonRequestSetting.RequestTimeout);
            }
            return httpClient;
        }

        static HttpRequestMessage CreateRequest(string url, HttpMethod httpMethod, JsonRequestSetting jsonRequestSetting)
        {
            if (jsonRequestSetting is null) jsonRequestSetting = JsonRequestSetting.Default;
            Uri uri = new Uri(url);
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, url);
            request.Headers.Add("User-Agent", "Silmoon.Extension_JsonHelper/1.0");

            DefaultRequestHostHeaders.Each(x =>
            {
                if (x.Key == "*" || uri.Host.ToLower() == x.Key.ToLower())
                    x.Value.Each(y => y.Value.Each(z => request.Headers.Add(y.Key, z)));
            });

            jsonRequestSetting.RequestHeaders.Each(x => x.Value.Each(y => request.Headers.Add(x.Key, y)));
            return request;
        }
        static async Task<JsonRequestResult<T>> ExecuteAsync<T>(HttpRequestMessage httpRequestMessage, JsonRequestSetting jsonRequestSetting = null)
        {
            using (var client = CreateHttpClient(jsonRequestSetting))
            {
                try
                {
                    using (var response = await client.SendAsync(httpRequestMessage))
                    {
                        if (response.Headers.GetValues("HttpContentGzipCompression")?.FirstOrDefault().ToBool() ?? false)
                        {
                            byte[] compressedData = await response.Content.ReadAsByteArrayAsync();
                            string decodeString = CompressHelper.DecompressByteArrayToString(compressedData);
                            JsonRequestResult<T> result = new JsonRequestResult<T>(response.StatusCode, decodeString);
                            return result;
                        }
                        else
                        {
                            JsonRequestResult<T> result = new JsonRequestResult<T>(response.StatusCode, await response.Content.ReadAsStringAsync());
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new JsonRequestResult<T>(ex);
                }
            }
        }


        public static Task<JsonRequestResult<T>> GetAsync<T>(string url, JsonRequestSetting jsonRequestSetting = null) => GetAsync<T>(url, null, jsonRequestSetting);
        public static Task<JsonRequestResult<T>> GetAsync<T>(string url, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null) => SendAsyncWithoutBody<T>(url, HttpMethod.Get, queryStringUrlDataCollection, jsonRequestSetting);


        public static Task<JsonRequestResult<T>> HeadAsync<T>(string url, JsonRequestSetting jsonRequestSetting = null) => HeadAsync<T>(url, null, jsonRequestSetting);
        public static Task<JsonRequestResult<T>> HeadAsync<T>(string url, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null) => SendAsyncWithoutBody<T>(url, HttpMethod.Head, queryStringUrlDataCollection, jsonRequestSetting);




        public static Task<JsonRequestResult<ResponseT>> PostAsync<ResponseT, PostT>(string url, PostT obj, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null) => SendAsyncWithBody<ResponseT, PostT>(url, HttpMethod.Post, obj, queryStringUrlDataCollection, jsonRequestSetting);
        public static async Task<JsonRequestResult<T>> PostFormDataAsync<T>(string url, UrlDataCollection postUrlDataCollection, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null)
        {
            if (postUrlDataCollection is null) throw new ArgumentNullException(nameof(postUrlDataCollection));

            if (queryStringUrlDataCollection != null) url = queryStringUrlDataCollection.AppendToUrl(url);
            using (var request = CreateRequest(url, HttpMethod.Post, jsonRequestSetting))
            {
                if (postUrlDataCollection != null) request.Content = new FormUrlEncodedContent(postUrlDataCollection.GetKeyValuePairs());
                return await ExecuteAsync<T>(request, jsonRequestSetting);
            }
        }
        public static async Task<JsonRequestResult<T>> PostMultipartFormDataData<T>(string url, MultipartFormDataContent multipartFormDataContent, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null)
        {
            if (multipartFormDataContent is null) throw new ArgumentNullException(nameof(multipartFormDataContent));
            if (queryStringUrlDataCollection != null) url = queryStringUrlDataCollection.AppendToUrl(url);
            using (var request = CreateRequest(url, HttpMethod.Post, jsonRequestSetting))
            {
                if (multipartFormDataContent != null) request.Content = multipartFormDataContent;
                return await ExecuteAsync<T>(request, jsonRequestSetting);
            }
        }
        public static Task<JsonRequestResult<T>> PostMultipartFormDataData<T>(string url, UrlDataCollection postUrlDataCollection, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null)
        {
            var multipartFormDataContent = new MultipartFormDataContent();
            postUrlDataCollection.GetKeyValuePairs().Each(x => multipartFormDataContent.Add(new StringContent(x.Value), x.Key));
            return PostMultipartFormDataData<T>(url, multipartFormDataContent, queryStringUrlDataCollection, jsonRequestSetting);
        }


        public static Task<JsonRequestResult<ResponseT>> PutAsync<ResponseT, PutT>(string url, PutT obj, JsonRequestSetting jsonRequestSetting = null) => PutAsync<ResponseT, PutT>(url, obj, null, jsonRequestSetting);
        public static Task<JsonRequestResult<ResponseT>> PutAsync<ResponseT, PutT>(string url, PutT obj, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null) => SendAsyncWithBody<ResponseT, PutT>(url, HttpMethod.Put, obj, queryStringUrlDataCollection, jsonRequestSetting);


        public static Task<JsonRequestResult<ResponseT>> DeleteAsync<ResponseT>(string url, JsonRequestSetting jsonRequestSetting = null) => DeleteAsync<ResponseT>(url, null, jsonRequestSetting);
        public static Task<JsonRequestResult<ResponseT>> DeleteAsync<ResponseT>(string url, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null) => SendAsyncWithoutBody<ResponseT>(url, HttpMethod.Delete, queryStringUrlDataCollection, jsonRequestSetting);

        public static Task<JsonRequestResult<ResponseT>> DeleteAsync<ResponseT, DeleteT>(string url, DeleteT obj, JsonRequestSetting jsonRequestSetting = null) => DeleteAsync<ResponseT, DeleteT>(url, obj, null, jsonRequestSetting);
        public static Task<JsonRequestResult<ResponseT>> DeleteAsync<ResponseT, DeleteT>(string url, DeleteT obj, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null) => SendAsyncWithBody<ResponseT, DeleteT>(url, HttpMethod.Delete, obj, queryStringUrlDataCollection, jsonRequestSetting);


        public static Task<JsonRequestResult<ResponseT>> PatchAsync<ResponseT, PatchT>(string url, PatchT obj, JsonRequestSetting jsonRequestSetting = null) => PatchAsync<ResponseT, PatchT>(url, obj, null, jsonRequestSetting);
        public static Task<JsonRequestResult<ResponseT>> PatchAsync<ResponseT, PatchT>(string url, PatchT obj, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null) => SendAsyncWithBody<ResponseT, PatchT>(url, new HttpMethod("PATCH"), obj, queryStringUrlDataCollection, jsonRequestSetting);


        #region main methods
        public static async Task<JsonRequestResult<T>> SendAsyncWithoutBody<T>(string url, HttpMethod httpMethod, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null)
        {
            if (queryStringUrlDataCollection != null) url = queryStringUrlDataCollection.AppendToUrl(url);
            using (var request = CreateRequest(url, httpMethod, jsonRequestSetting))
            {
                return await ExecuteAsync<T>(request, jsonRequestSetting);
            }
        }
        public static async Task<JsonRequestResult<ResponseT>> SendAsyncWithBody<ResponseT>(string url, HttpMethod httpMethod, object obj, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null)
        {
            if (queryStringUrlDataCollection != null) url = queryStringUrlDataCollection.AppendToUrl(url);
            using (var request = CreateRequest(url, httpMethod, jsonRequestSetting))
            {
                request.Content = new StringContent(obj.ToJsonString(jsonRequestSetting?.JsonSerializerSettings), Encoding.UTF8, "application/json");
                return await ExecuteAsync<ResponseT>(request, jsonRequestSetting);
            }
        }
        public static async Task<JsonRequestResult<ResponseT>> SendAsyncWithBody<ResponseT, SendT>(string url, HttpMethod httpMethod, SendT obj, UrlDataCollection queryStringUrlDataCollection, JsonRequestSetting jsonRequestSetting = null)
        {
            if (queryStringUrlDataCollection != null) url = queryStringUrlDataCollection.AppendToUrl(url);
            using (var request = CreateRequest(url, httpMethod, jsonRequestSetting))
            {
                request.Content = new StringContent(obj.ToJsonString(jsonRequestSetting?.JsonSerializerSettings), Encoding.UTF8, "application/json");
                return await ExecuteAsync<ResponseT>(request, jsonRequestSetting);
            }
        }
        #endregion
    }
}
