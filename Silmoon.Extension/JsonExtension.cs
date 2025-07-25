using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Silmoon.Extension
{
    public static class JsonExtension
    {
        public static void Remove(this JToken token, string name) => ((JObject)token).Remove(name);

        public static string ToJsonString(this object obj) => JsonConvert.SerializeObject(obj);
        public static string ToJsonString(this object obj, JsonSerializerSettings settings) => JsonConvert.SerializeObject(obj, settings);
        public static string ToFormattedJsonString(this object obj) => ToFormattedJsonString(obj, JsonConvert.DefaultSettings?.Invoke() ?? new JsonSerializerSettings());

        public static string ToFormattedJsonString(this object obj, JsonSerializerSettings settings)
        {
            JsonSerializer serializer = JsonSerializer.Create(settings);
            if (obj != null)
            {
                using (StringWriter textWriter = new StringWriter())
                using (JsonTextWriter jsonWriter = new JsonTextWriter(textWriter) { Formatting = Formatting.Indented, Indentation = 4, IndentChar = ' ' })
                {
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }
            }
            else
            {
                return "null";
            }
        }

        public static T[] ToObjects<T>(this JArray array)
        {
            T[] result = new T[array.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array[i].ToObject<T>();
            }
            return result;
        }
    }
}
