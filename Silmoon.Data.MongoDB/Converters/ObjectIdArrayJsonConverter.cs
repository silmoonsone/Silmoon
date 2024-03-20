using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Converters
{
    public class ObjectIdArrayJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objectIdList = value as IEnumerable<ObjectId>;
            if (objectIdList == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartArray();
            foreach (var objectId in objectIdList)
            {
                writer.WriteValue(objectId.ToString());
            }
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
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
                    objectIdList.Add(string.IsNullOrEmpty(objectIdString) ? ObjectId.Empty : ObjectId.Parse(objectIdString));
                }
                return objectIdList;
            }
            else
            {
                throw new Exception($"Unexpected token parsing List<ObjectId>. Expected StartArray, got {reader.TokenType}");
            }
        }

        public override bool CanConvert(Type objectType)
        {
            // 检查 objectType 是否是 List<> 的泛型类型
            if (!objectType.IsGenericType)
            {
                return false;
            }

            // 检查泛型类型是否为 List<>
            if (objectType.GetGenericTypeDefinition() != typeof(List<>))
            {
                return false;
            }

            // 检查 List<> 的泛型参数是否为 ObjectId
            return objectType.GetGenericArguments()[0] == typeof(ObjectId);
        }
    }
}
