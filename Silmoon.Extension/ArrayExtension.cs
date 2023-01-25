using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class ArrayExtension
    {
        public static string[] ToStringArray(this Array array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array.GetValue(i).ToString();
            }
            return result;
        }
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
        public static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array?.Length == 0;
        }
        public static bool IsNullOrEmpty<T>(this List<T> array)
        {
            return array == null || array?.Count == 0;
        }
    }
}
