using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Silmoon.Extension.Converters
{
    public class BigIntegerJsonConverter : JsonConverter<BigInteger>
    {
        public override void WriteJson(JsonWriter writer, BigInteger value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override BigInteger ReadJson(JsonReader reader, Type objectType, BigInteger existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing BigInteger. Expected String, got {reader.TokenType}");
            }

            var value = (string)reader.Value;
            return string.IsNullOrEmpty(value) ? 0 : BigInteger.Parse(value);
        }
    }
}
