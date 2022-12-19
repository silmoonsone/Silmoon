using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class ObjectExtension
    {
        public static string ToStringOrNull(this object obj)
        {
            return obj?.ToString();
        }
        //convert a object to JObject
        public static JObject ToJObject(this object obj)
        {
            return JObject.FromObject(obj);
        }
        //convert a object array to JArray
        public static JArray ToJArray(this object[] obj)
        {
            return JArray.FromObject(obj);
        }
        //if a object is array, convert this object to JArray
        public static JArray ToJArray(this object obj)
        {
            if (obj is Array)
            {
                return JArray.FromObject(obj);
            }
            else
            {
                return null;
            }
        }
    }
}
