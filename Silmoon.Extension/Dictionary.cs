using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class DictionaryExtension
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
}
