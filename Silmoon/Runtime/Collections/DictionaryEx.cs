using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Runtime.Collections
{
    public class DictionaryEx<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public TValue Get(TKey key, TValue defaultValue)
        {
            var has = ContainsKey(key);
            if (!has) return defaultValue;
            else
            {
                return this[key];
            }
        }
        public TValue Get(TKey key)
        {
            var has = ContainsKey(key);
            if (!has) return default(TValue);
            else
            {
                return this[key];
            }
        }
    }
}
