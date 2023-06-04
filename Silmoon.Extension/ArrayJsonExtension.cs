using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class ArrayJsonExtension
    {
        public static JArray ToJArray(this Array array)
        {
            var result = JArray.FromObject(array);
            return result;
        }
        public static JArray ToJArray<T>(this List<T> list)
        {
            var result = JArray.FromObject(list);
            return result;
        }
    }
}
