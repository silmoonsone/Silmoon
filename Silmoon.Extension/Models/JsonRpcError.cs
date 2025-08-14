using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension.Models
{
    public class JsonRpcError
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public JToken Data { get; set; }
        public override string ToString() => $"({Code}) {Message}";
    }
}
