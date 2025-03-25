using Newtonsoft.Json;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.SdkApi.Warpper.Bark
{
    public class BarkResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }
        [JsonIgnore]
        public DateTime DateTime => SpecialConverter.FromUnixTimestamp(Timestamp);
        public BarkResponse(int code, string message, int timestamp)
        {
            Code = code;
            Message = message;
            Timestamp = timestamp;
        }
    }
}
