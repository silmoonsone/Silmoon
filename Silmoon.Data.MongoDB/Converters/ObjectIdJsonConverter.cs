using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Extension;

namespace Silmoon.Data.MongoDB.Converters
{
    /// <summary>
    /// ObjectId转换器，原本不用此转换器，可以直接将Bson的ObjectId类型转换成字符串，但是在反序列化时，字符串没有反序列化称Bson的ObjectId类型的实现，所以需要此转换器。
    /// </summary>
    /// <remarks>
    /// <code>
    /// 可以直接在属性上添加[JsonConverter(typeof(ObjectIdConverter))]，也可以在Newtonsoft.Json.JsonConvert.DefaultSettings中添加此转换器：
    ///            Newtonsoft.Json.JsonConvert.DefaultSettings = new Func<Newtonsoft.Json.JsonSerializerSettings>(() =>
    ///            {
    ///                var settings = new Newtonsoft.Json.JsonSerializerSettings();
    ///                settings.Converters.Add(new ObjectIdConverter());
    ///                return settings;
    ///            });
    /// </code>
    /// </remarks>
    public class ObjectIdJsonConverter : JsonConverter<ObjectId>
    {
        public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is null) return ObjectId.Empty;

            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing ObjectId. Expected String, got {reader.TokenType}");
            }

            var value = (string)reader.Value;
            return value.IsNullOrEmpty() ? ObjectId.Empty : ObjectId.Parse(value);
        }
    }
}
