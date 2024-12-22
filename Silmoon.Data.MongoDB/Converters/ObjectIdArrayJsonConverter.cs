using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Extension;

namespace Silmoon.Data.MongoDB.Converters
{
    public class ObjectIdArrayJsonConverter : JsonConverter<List<ObjectId>>
    {
        public override void WriteJson(JsonWriter writer, List<ObjectId> value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartArray();
            foreach (var objectId in value)
            {
                writer.WriteValue(objectId.ToString());
            }
            writer.WriteEndArray();
        }

        public override List<ObjectId> ReadJson(JsonReader reader, Type objectType, List<ObjectId> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var objectIdList = new List<ObjectId>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                {
                    if (reader.TokenType != JsonToken.String)
                    {
                        throw new Exception($"Unexpected token parsing ObjectId. Expected String, got {reader.TokenType}");
                    }

                    var objectIdString = (string)reader.Value;
                    objectIdList.Add(objectIdString.IsNullOrEmpty() ? ObjectId.Empty : ObjectId.Parse(objectIdString));
                }
                return objectIdList;
            }
            else
            {
                throw new Exception($"Unexpected token parsing List<ObjectId>. Expected StartArray, got {reader.TokenType}");
            }
        }
    }
}
