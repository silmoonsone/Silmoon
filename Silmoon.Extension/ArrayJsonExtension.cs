using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class ArrayJsonExtension
    {
        public static JArray ToJArray(this Array array) => JArray.FromObject(array);
        public static JArray ToJArray<T>(this List<T> list) => JArray.FromObject(list);
    }
}
