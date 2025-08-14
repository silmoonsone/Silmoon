using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension.Models
{
    public class JsonRpcResponse
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; }
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("result")]
        public JToken Result { get; set; }
        [JsonProperty("error")]
        public JsonRpcError Error { get; set; }
    }
}
