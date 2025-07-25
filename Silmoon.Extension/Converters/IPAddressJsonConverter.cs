using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Extension.Converters
{
    public class IPAddressJsonConverter : JsonConverter<IPAddress>
    {
        public override void WriteJson(JsonWriter writer, IPAddress value, JsonSerializer serializer)
        {
            // 只序列化为字符串形式（如：192.168.0.1）
            writer.WriteValue(value?.ToString());
        }

        public override IPAddress ReadJson(JsonReader reader, Type objectType, IPAddress existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // 从字符串反序列化为 IPAddress 对象
            if (reader.Value is null) return null;
            if (reader.TokenType != JsonToken.String) throw new Exception($"Unexpected token parsing IPAddress. Expected String, got {reader.TokenType}");

            string text = (string)reader.Value;
            return text.IsNullOrEmpty() ? null : IPAddress.Parse(text);
        }
    }
}
