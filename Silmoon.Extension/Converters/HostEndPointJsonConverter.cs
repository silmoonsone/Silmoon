using Newtonsoft.Json;
using Silmoon.Extension;
using Silmoon.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension.Converters
{
    public class HostEndPointJsonConverter : JsonConverter<HostEndPoint>
    {
        public override void WriteJson(JsonWriter writer, HostEndPoint value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override HostEndPoint ReadJson(JsonReader reader, Type objectType, HostEndPoint existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing HostEndPoint. Expected String, got {reader.TokenType}");
            }

            string text = (string)reader.Value;
            if (!text.IsNullOrEmpty())
            {
                // 如果缺少端口部分，补上默认的 ":0"
                if (!text.Contains(":")) text += ":0";

                return text.IsNullOrEmpty() ? null : HostEndPoint.Parse(text);
            }
            else
            {
                return null;
            }
        }
    }
}
