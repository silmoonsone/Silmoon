using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class DictionaryExtensionSS
    {
        public static string Get(this IDictionary<string, string> dictionary, string key, string defaultValue)
        {
            var has = dictionary.ContainsKey(key);
            if (!has) return defaultValue;
            else
            {
                return dictionary[key];
            }
        }
        public static string Get(this IDictionary<string, string> dictionary, string key)
        {
            var has = dictionary.ContainsKey(key);
            if (!has) return null;
            else
            {
                return dictionary[key];
            }
        }
    }
    public static class DictionaryExtensionSO
    {
        public static object Get(this IDictionary<string, object> dictionary, string key, object defaultValue)
        {
            var has = dictionary.ContainsKey(key);
            if (!has) return defaultValue;
            else
            {
                return dictionary[key];
            }
        }
        public static object Get(this IDictionary<string, object> dictionary, string key)
        {
            var has = dictionary.ContainsKey(key);
            if (!has) return null;
            else
            {
                return dictionary[key];
            }
        }
    }
}
