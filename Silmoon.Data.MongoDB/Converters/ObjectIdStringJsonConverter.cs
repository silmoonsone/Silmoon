using MongoDB.Bson;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Silmoon.Data.MongoDB.Converters
{
    /// <summary>
    /// 用于System.Text.Json的序列化ObjectId输出string和将string转换为ObjectId的转换器
    /// </summary>
    public class ObjectIdStringJsonConverter : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (str.IsNullOrEmpty()) return default;
            else return ObjectId.Parse(str);
        }

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
    }
}
