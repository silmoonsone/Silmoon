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
            writer.WriteValue(value.ToString());
        }

        public override IPAddress ReadJson(JsonReader reader, Type objectType, IPAddress existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // 从字符串反序列化为 IPAddress 对象
            return IPAddress.Parse((string)reader.Value);
        }
    }
}
