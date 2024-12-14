using Newtonsoft.Json;
using Silmoon.Extension;
using Silmoon.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Converters
{
    public class HostEndPointJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing ObjectId. Expected String, got {reader.TokenType}");
            }

            string text = (string)reader.Value;
            if (!text.IsNullOrEmpty())
            {
                if (!text.Contains(":")) text += ":0";
                return string.IsNullOrEmpty(text) ? null : HostEndPoint.Parse(text);
            }
            else return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(HostEndPoint).IsAssignableFrom(objectType);
        }
    }
}
