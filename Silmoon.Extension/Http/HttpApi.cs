using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Silmoon.Secure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Silmoon.Extension.Models;
using Silmoon.Collections;
using Silmoon.Extension.Enums;

namespace Silmoon.Extension.Http
{
    public class HttpApi
    {
        [Obsolete]
        public static ApiResult<T> GetRequest<T>(string Url, NameValueCollection Data)
        {
            try
            {
                var obj = JsonHelper.GetObject<ApiResult<T>>(Url + "?" + Data.ToQueryString());
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
            }
        }
        public static async Task<ApiResult<T>> GetRequestAsync<T>(string Url, NameValueCollection Data) => await GetRequestAsync<T>(Url, UrlDataCollection.FromNameValueCollection(Data));
        public static async Task<ApiResult<T>> GetRequestAsync<T>(string Url, UrlDataCollection Data)
        {
            try
            {
                var obj = await JsonHelper.GetObjectAsync<ApiResult<T>>(Url + "?" + Data.ToQueryString());
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
            }
        }

        [Obsolete]
        public static ApiResult<T> PostRequest<T>(string Url, NameValueCollection Data)
        {
            try
            {
                var obj = JsonHelper.GetObjectByPost<ApiResult<T>>(Url, Data);
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
            }
        }
        public static async Task<ApiResult<T>> PostRequestAsync<T>(string Url, NameValueCollection Data) => await PostRequestAsync<T>(Url, UrlDataCollection.FromNameValueCollection(Data));
        public static async Task<ApiResult<T>> PostRequestAsync<T>(string Url, UrlDataCollection Data)
        {
            try
            {
                var obj = await JsonHelper.GetObjectByPostAsync<ApiResult<T>>(Url, Data);
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
            }
        }




        [Obsolete]
        public static ApiResult GetRequest(string Url, NameValueCollection Data)
        {
            try
            {
                var obj = JsonHelper.GetObject<ApiResult>(Url + "?" + Data.ToQueryString());
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult.Create(ResultState.Exception, ex.Message, -1, ex);
            }
        }
        public static async Task<ApiResult> GetRequestAsync(string Url, NameValueCollection Data) => await GetRequestAsync(Url, UrlDataCollection.FromNameValueCollection(Data));
        public static async Task<ApiResult> GetRequestAsync(string Url, UrlDataCollection Data)
        {
            try
            {
                var obj = await JsonHelper.GetObjectAsync<ApiResult>(Url + "?" + Data.ToQueryString());
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult.Create(ResultState.Exception, ex.Message, -1, ex);
            }
        }

        [Obsolete]
        public static ApiResult PostRequest(string Url, NameValueCollection Data)
        {
            try
            {
                var obj = JsonHelper.GetObjectByPost<ApiResult>(Url, Data);
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult.Create(ResultState.Exception, ex.Message, -1, ex);
            }
        }
        public static async Task<ApiResult> PostRequestAsync(string Url, NameValueCollection Data) => await PostRequestAsync(Url, UrlDataCollection.FromNameValueCollection(Data));
        public static async Task<ApiResult> PostRequestAsync(string Url, UrlDataCollection Data)
        {
            try
            {
                var obj = await JsonHelper.GetObjectByPostAsync<ApiResult>(Url, Data);
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult.Create(ResultState.Exception, ex.Message, -1, ex);
            }
        }



        public static async Task<ApiResult<T>> PostRequestAsync<T>(string Url, UrlDataCollection QueryData, UrlDataCollection PostData)
        {
            try
            {
                var obj = await JsonHelper.GetObjectByPostAsync<ApiResult<T>>(Url + "?" + QueryData.ToQueryString(), PostData);
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
            }
        }



        [Obsolete]
        public static ApiResult<T> SignaturePostRequest<T>(RequestToken requestToken, string Url, NameValueCollection Data, string AppIdRenameTo = "AppId")
        {
            try
            {
                if (requestToken.AppId != null) Data.Add(AppIdRenameTo, requestToken.AppId);
                if (requestToken.Source != null) Data.Add("Source", requestToken.Source);
                if (requestToken.CallerName != null) Data.Add("CallerName", requestToken.CallerName);

                var signature = Data.GetSign("Key", requestToken.SignKey);

                if (!signature.IsNullOrEmpty()) Data.Add("Signature", signature);
                var obj = JsonHelper.GetObjectByPost<ApiResult<T>>(Url, Data);
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
            }
        }
        public static async Task<ApiResult<T>> SignaturePostRequestAsync<T>(RequestToken requestToken, string Url, UrlDataCollection Data, string AppIdRenameTo = "AppId")
        {
            try
            {
                if (requestToken.AppId != null) Data.Add(AppIdRenameTo, requestToken.AppId);
                if (requestToken.Source != null) Data.Add("Source", requestToken.Source);
                if (requestToken.CallerName != null) Data.Add("CallerName", requestToken.CallerName);

                var signature = Data.GetSign("Key", requestToken.SignKey);

                if (!signature.IsNullOrEmpty()) Data.Add("Signature", signature);
                var obj = await JsonHelper.GetObjectByPostAsync<ApiResult<T>>(Url, Data);
                return obj;
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
            }
        }


        public static ApiResult<T> RequestFormPostEncrypt<T>(RequestToken requestToken, string Url, object Data, string AppIdRenameTo = "AppId") => RequestFormPostEncrypt<T>(requestToken, Url, JObject.FromObject(Data), AppIdRenameTo);
        public static ApiResult<T> RequestFormPostEncrypt<T>(RequestToken requestToken, string Url, JObject Data, string AppIdRenameTo = "AppId")
        {
            if (requestToken.AppId != null) Data.Add(AppIdRenameTo, requestToken.AppId);
            if (requestToken.Source != null) Data.Add("Source", requestToken.Source);
            if (requestToken.CallerName != null) Data.Add("CallerName", requestToken.CallerName);
            Data.Add("TimeStamp", DateTime.Now.GetUnixTimestamp().ToString());
            Data.Add("NonceStr", HashHelper.RandomChars(16));
            //将JObject的对象转换成字符串
            var payload = Data.ToJsonString();

            //整体传输对象
            JObject obj = new JObject();
            //将正文和签名密钥拼接计算MD5签名
            obj["Signature"] = HashHelper.GetMD5Hash(payload + requestToken.SignKey);
            //将正文使用加密密钥加密
            obj["Data"] = EncryptHelper.AesEncryptStringToBase64String(payload, requestToken.EncryptKey);
            //将正文再次和加密密钥拼接使用MD5签名
            obj["Check"] = HashHelper.GetMD5Hash(obj["Data"].Value<string>() + requestToken.EncryptKey);
            //将AppId添加到传输对象
            obj[AppIdRenameTo] = requestToken.AppId;
            //整体传输对象转换为json字符串
            var str = obj.ToJsonString();

            using (var client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(Url);
                    httpRequestMessage.Headers.Add("ApiEncrypt", "true");
                    httpRequestMessage.Content = new StringContent(str);
                    httpRequestMessage.Content.Headers.Remove("Content-Type");
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    var response = client.SendAsync(httpRequestMessage).Result;
                    var stream = response.Content.ReadAsStreamAsync().Result;
                    str = stream.MakeToString();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<ApiResult<T>>(str);
                        return result;
                    }
                    else
                        return ApiResult<T>.Create(ResultState.NotHttpSuccess, default, "Server return not is HTTP 2XX status code", -1);
                }
                catch (Exception ex)
                {
                    return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
                }
            }
        }
        public static ApiResult<T> RequestFormPostEncrypt<T>(RequestToken requestToken, string Url, NameValueCollection Data, string AppIdRenameTo = "AppId")
        {
            if (requestToken.AppId != null) Data.Add(AppIdRenameTo, requestToken.AppId);
            if (requestToken.Source != null) Data.Add("Source", requestToken.Source);
            if (requestToken.CallerName != null) Data.Add("CallerName", requestToken.CallerName);
            Data.Add("TimeStamp", DateTime.Now.GetUnixTimestamp().ToString());
            Data.Add("NonceStr", HashHelper.RandomChars(16));
            Data.Add("Signature", Data.GetSign("Key", requestToken.SignKey));
            //将NameValueCollection的对象转换成字符串
            var payload = Data.ToQueryString();

            //整体传输对象
            JObject obj = new JObject();
            //将正文和签名密钥拼接计算签名
            obj["Signature"] = HashHelper.GetMD5Hash(payload + requestToken.SignKey);
            //将正文使用加密密钥加密
            obj["Data"] = EncryptHelper.AesEncryptStringToBase64String(payload, requestToken.EncryptKey);
            //将正文再次和加密密钥拼接使用MD5签名
            obj["Check"] = HashHelper.GetMD5Hash(obj["Data"].Value<string>() + requestToken.EncryptKey);
            //将AppId添加到传输对象
            obj[AppIdRenameTo] = requestToken.AppId;
            //整体传输对象转换为json字符串
            var str = obj.ToJsonString();

            using (var client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(Url);
                    httpRequestMessage.Headers.Add("ApiEncrypt", "true");
                    httpRequestMessage.Headers.Add("SilmoonDevAppEncrypted", "true");
                    httpRequestMessage.Content = new StringContent(str);
                    httpRequestMessage.Content.Headers.Remove("Content-Type");
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                    var response = client.SendAsync(httpRequestMessage).Result;
                    var stream = response.Content.ReadAsStreamAsync().Result;
                    str = stream.MakeToString();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<ApiResult<T>>(str);
                        return result;
                    }
                    else
                        return ApiResult<T>.Create(ResultState.NotHttpSuccess, default, "Server return not is HTTP 2XX status code", -1);
                }
                catch (Exception ex)
                {
                    return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
                }
            }
        }

        public static async Task<ApiResult<T>> RequestFormPostEncryptAsync<T>(RequestToken requestToken, string Url, object Data, string AppIdRenameTo = "AppId")
        {
            var jObj = JObject.FromObject(Data);
            return await RequestFormPostEncryptAsync<T>(requestToken, Url, jObj, AppIdRenameTo);
        }
        public static async Task<ApiResult<T>> RequestFormPostEncryptAsync<T>(RequestToken requestToken, string Url, JObject Data, string AppIdRenameTo = "AppId")
        {
            if (requestToken.AppId != null) Data.Add(AppIdRenameTo, requestToken.AppId);
            if (requestToken.Source != null) Data.Add("Source", requestToken.Source);
            if (requestToken.CallerName != null) Data.Add("CallerName", requestToken.CallerName);
            Data.Add("TimeStamp", DateTime.Now.GetUnixTimestamp().ToString());
            Data.Add("NonceStr", HashHelper.RandomChars(16));
            //将JObject的对象转换成字符串
            var payload = Data.ToJsonString();

            //整体传输对象
            JObject obj = new JObject();
            //将正文和签名密钥拼接计算MD5签名
            obj["Signature"] = HashHelper.GetMD5Hash(payload + requestToken.SignKey);
            //将正文使用加密密钥加密
            obj["Data"] = EncryptHelper.AesEncryptStringToBase64String(payload, requestToken.EncryptKey);
            //将正文再次和加密密钥拼接使用MD5签名
            obj["Check"] = HashHelper.GetMD5Hash(obj["Data"].Value<string>() + requestToken.EncryptKey);
            //将AppId添加到传输对象
            obj[AppIdRenameTo] = requestToken.AppId;
            //整体传输对象转换为json字符串
            var str = obj.ToJsonString();

            using (var client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(Url);
                    httpRequestMessage.Headers.Add("ApiEncrypt", "true");
                    httpRequestMessage.Content = new StringContent(str);
                    httpRequestMessage.Content.Headers.Remove("Content-Type");
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    var response = await client.SendAsync(httpRequestMessage);
                    var stream = await response.Content.ReadAsStreamAsync();
                    str = stream.MakeToString();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<ApiResult<T>>(str);
                        return result;
                    }
                    else
                        return ApiResult<T>.Create(ResultState.NotHttpSuccess, default, "Server return not is HTTP 2XX status code", -1);
                }
                catch (Exception ex)
                {
                    return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
                }
            }
        }
        public static async Task<ApiResult<T>> RequestFormPostEncryptAsync<T>(RequestToken requestToken, string Url, NameValueCollection Data, string AppIdRenameTo = "AppId")
        {
            if (requestToken.AppId != null) Data.Add(AppIdRenameTo, requestToken.AppId);
            if (requestToken.Source != null) Data.Add("Source", requestToken.Source);
            if (requestToken.CallerName != null) Data.Add("CallerName", requestToken.CallerName);
            Data.Add("TimeStamp", DateTime.Now.GetUnixTimestamp().ToString());
            Data.Add("NonceStr", HashHelper.RandomChars(16));
            Data.Add("Signature", Data.GetSign("Key", requestToken.SignKey));
            //将NameValueCollection的对象转换成字符串
            var payload = Data.ToQueryString();

            //整体传输对象
            JObject obj = new JObject();
            //将正文和签名密钥拼接计算MD5签名
            obj["Signature"] = HashHelper.GetMD5Hash(payload + requestToken.SignKey);
            //将正文使用加密密钥加密
            obj["Data"] = EncryptHelper.AesEncryptStringToBase64String(payload, requestToken.EncryptKey);
            //将正文再次和加密密钥拼接使用MD5签名
            obj["Check"] = HashHelper.GetMD5Hash(obj["Data"].Value<string>() + requestToken.EncryptKey);
            //将AppId添加到传输对象
            obj[AppIdRenameTo] = requestToken.AppId;

            var str = obj.ToJsonString();

            using (var client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(Url);
                    httpRequestMessage.Headers.Add("ApiEncrypt", "true");
                    httpRequestMessage.Content = new StringContent(str);
                    httpRequestMessage.Content.Headers.Remove("Content-Type");
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                    var response = await client.SendAsync(httpRequestMessage);
                    var stream = await response.Content.ReadAsStreamAsync();
                    str = stream.MakeToString();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<ApiResult<T>>(str);
                        return result;
                    }
                    else
                        return ApiResult<T>.Create(ResultState.NotHttpSuccess, default, "Server return not is HTTP 2XX status code", -1);
                }
                catch (Exception ex)
                {
                    return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
                }
            }
        }
        public static async Task<ApiResult<T>> RequestFormPostEncryptAsync<T>(RequestToken requestToken, string Url, UrlDataCollection Data, string AppIdRenameTo = "AppId")
        {
            if (requestToken.AppId != null) Data.Add(AppIdRenameTo, requestToken.AppId);
            if (requestToken.Source != null) Data.Add("Source", requestToken.Source);
            if (requestToken.CallerName != null) Data.Add("CallerName", requestToken.CallerName);
            Data.Add("TimeStamp", DateTime.Now.GetUnixTimestamp().ToString());
            Data.Add("NonceStr", HashHelper.RandomChars(16));
            var signature = Data.GetSign("Key", requestToken.SignKey);
            Data.Add("Signature", signature);
            //将NameValueCollection的对象转换成字符串
            var payload = Data.ToQueryString();

            //整体传输对象
            JObject obj = new JObject();
            //将正文和签名密钥拼接计算MD5签名
            obj["Signature"] = HashHelper.GetMD5Hash(payload + requestToken.SignKey);
            //将正文使用加密密钥加密
            obj["Data"] = EncryptHelper.AesEncryptStringToBase64String(payload, requestToken.EncryptKey);
            //将正文再次和加密密钥拼接使用MD5签名
            obj["Check"] = HashHelper.GetMD5Hash(obj["Data"].Value<string>() + requestToken.EncryptKey);
            //将AppId添加到传输对象
            obj[AppIdRenameTo] = requestToken.AppId;

            var str = obj.ToJsonString();

            using (var client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(Url);
                    httpRequestMessage.Headers.Add("ApiEncrypt", "true");
                    httpRequestMessage.Content = new StringContent(str);
                    httpRequestMessage.Content.Headers.Remove("Content-Type");
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                    var response = await client.SendAsync(httpRequestMessage);
                    var stream = await response.Content.ReadAsStreamAsync();
                    str = stream.MakeToString();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<ApiResult<T>>(str);
                        return result;
                    }
                    else
                        return ApiResult<T>.Create(ResultState.NotHttpSuccess, default, "Server return not is HTTP 2XX status code", -1);
                }
                catch (Exception ex)
                {
                    return ApiResult<T>.Create(ResultState.Exception, default, ex.Message, -1, ex);
                }
            }
        }
    }
}
