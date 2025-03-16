using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Silmoon.Extension.Http
{
    public class JsonRequestSetting
    {
        public static JsonRequestSetting Default { get; } = new JsonRequestSetting();
        public Dictionary<string, string[]> RequestHeaders { get; set; } = new Dictionary<string, string[]>();
        public JsonSerializerSettings JsonSerializerSettings { get; set; } = null;
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);
        public bool IgnoreCertificateValidation { get; set; } = false;

        public void SetBearerToken(string token)
        {
            RequestHeaders["Authorization"] = new string[] { $"Bearer {token}" };
        }
        public void SetBasicAuth(string username, string password)
        {
            RequestHeaders["Authorization"] = new string[] { $"Basic {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"))}" };
        }
    }
}
