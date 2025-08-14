using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Extension.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Extension.Models
{
    public class JsonRpcRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 1; // Default ID, can be set to any unique value
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; } = "2.0";
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("params")]
        public JToken Params { get; set; } = new JArray(); // Default to an empty array
        public override string ToString() => $"(id: {Id}, jsonrpc: {JsonRpc}) {Method}";
        [JsonIgnore]
        public JArray JArrayParams => Params as JArray;
        public JObject JObjectParams => Params as JObject;

        public async Task<JsonRequestResult<JsonRpcResponse>> SendRequest(string url) => await SendRequest(this, url);

        public static async Task<JsonRequestResult<JsonRpcResponse>> SendRequest(JsonRpcRequest jsonRpcRequest, string url) => await JsonRequest.PostAsync<JsonRpcResponse, JsonRpcRequest>(url, jsonRpcRequest, null);
        public static async Task<JsonRequestResult<JsonRpcResponse[]>> SendRequests(JsonRpcRequest[] jsonRpcRequests, string url) => await JsonRequest.PostAsync<JsonRpcResponse[], JsonRpcRequest[]>(url, jsonRpcRequests, null);

        public static JsonRpcRequest[] operator +(JsonRpcRequest left, JsonRpcRequest right) => new JsonRpcRequest[] { left, right };
        public static JsonRpcRequest[] operator +(JsonRpcRequest left, JsonRpcRequest[] right) => new JsonRpcRequest[] { left }.Concat(right).ToArray();
        public static JsonRpcRequest[] operator +(JsonRpcRequest[] left, JsonRpcRequest right) => left.Concat(new JsonRpcRequest[] { right }).ToArray();
    }
}
