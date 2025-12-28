using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Silmoon.Extension
{
    public static class JsonExtension
    {
        static JsonSerializerSettings DefaultSetting = new JsonSerializerSettings();
        public static void Remove(this JToken token, string name) => ((JObject)token).Remove(name);

        public static string ToJsonString(this object obj) => ToJsonString(obj, JsonConvert.DefaultSettings?.Invoke() ?? DefaultSetting);
        public static string ToJsonString(this object obj, JsonSerializerSettings settings) => JsonConvert.SerializeObject(obj, settings);
        public static string ToFormattedJsonString(this object obj) => ToFormattedJsonString(obj, JsonConvert.DefaultSettings?.Invoke() ?? DefaultSetting);
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

        public static string ToJsonString<T>(this object obj) => ToJsonString<T>(obj, JsonConvert.DefaultSettings?.Invoke() ?? DefaultSetting);
        public static string ToJsonString<T>(this object obj, JsonSerializerSettings settings)
        {
            var settings2 = new JsonSerializerSettings(settings);
            settings2.ContractResolver = new ForceBaseTypeResolver<T>();
            var result = JsonConvert.SerializeObject(obj, settings2);
            return result;
        }
        public static string ToFormattedJsonString<T>(this object obj) => ToFormattedJsonString<T>(obj, JsonConvert.DefaultSettings?.Invoke() ?? DefaultSetting);
        public static string ToFormattedJsonString<T>(this object obj, JsonSerializerSettings settings)
        {
            var settings2 = new JsonSerializerSettings(settings);
            settings2.ContractResolver = new ForceBaseTypeResolver<T>();
            JsonSerializer serializer = JsonSerializer.Create(settings2);
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

#if NET10_0_OR_GREATER
        extension(JsonConvert)
        {
            public static string SerializeObject<T>(object obj) => ToJsonString<T>(obj);
            public static string SerializeObject<T>(object obj, JsonSerializerSettings settings) => ToJsonString<T>(obj, settings);
        }
#endif


        sealed class ForceBaseTypeResolver<TBase> : DefaultContractResolver
        {
            protected override JsonContract CreateContract(Type objectType)
            {
                // 运行时是派生类时，也强制用 TBase 的契约
                if (typeof(TBase).IsAssignableFrom(objectType))
                    return base.CreateContract(typeof(TBase));

                return base.CreateContract(objectType);
            }
        }
    }
}
