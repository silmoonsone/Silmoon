using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class DictionaryExtension
    {
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            var has = dictionary.ContainsKey(key);
            if (!has) return defaultValue;
            else return dictionary[key];
        }
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var has = dictionary.ContainsKey(key);
            if (!has) return default;
            else return dictionary[key];
        }
    }
}
