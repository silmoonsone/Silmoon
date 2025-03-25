using Newtonsoft.Json;
using Silmoon.Collections;
using Silmoon.Extension.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.SdkApi.Warpper.Cloudflare
{
    public class TurnstileApi
    {
        public static async Task<JsonRequestResult<TurnstileResult>> Verify(string secret, string response)
        {
            var url = "https://challenges.cloudflare.com/turnstile/v0/siteverify";
            var data = new UrlDataCollection
            {
                { "secret", secret },
                { "response", response }
            };

            var result = await JsonRequest.PostFormDataAsync<TurnstileResult>(url, data, null);
            return result;
        }
    }
    public class TurnstileResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTs { get; set; }
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
    }
}
