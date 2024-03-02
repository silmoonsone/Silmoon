using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class ObjectExtension
    {
        public static string ToStringOrNull(this object obj) => obj?.ToString();
        public static string ToStringOrEmpty(this object obj) => obj?.ToString() ?? string.Empty;
        public static JObject ToJObject(this object obj) => JObject.FromObject(obj);
        public static JArray ToJArray(this object[] obj) => JArray.FromObject(obj);
        public static JArray ToJArray(this object obj) => obj is Array ? JArray.FromObject(obj) : null;
    }
}
