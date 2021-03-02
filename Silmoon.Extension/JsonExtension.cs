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
        public static void Remove(this JToken token, string name)
        {
            ((JObject)token).Remove(name);
        }

        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static string ToFormattedJsonString(this object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            if (obj != null)
            {
                using (StringWriter textWriter = new StringWriter())
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(textWriter) { Formatting = Formatting.Indented, Indentation = 4, IndentChar = ' ' })
                    {
                        serializer.Serialize(jsonWriter, obj);
                        return textWriter.ToString();
                    }
                }
            }
            else
            {
                return "null";
            }
        }
    }
}
