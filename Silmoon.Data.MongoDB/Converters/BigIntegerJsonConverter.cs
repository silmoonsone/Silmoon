using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Silmoon.Data.MongoDB.Converters
{
    public class BigIntegerJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing ObjectId. Expected String.get{reader.TokenType}");
            }
            var value = (string)reader.Value;
            return string.IsNullOrEmpty(value) ? 0 : BigInteger.Parse(value);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectId).IsAssignableFrom(objectType);
        }
    }
}
