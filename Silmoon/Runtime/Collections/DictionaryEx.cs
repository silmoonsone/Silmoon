using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Runtime.Collections
{
    public class DictionaryEx<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public TValue Get(TKey key, TValue defaultValue)
        {
            if (!ContainsKey(key)) return defaultValue;
            else return this[key];
        }
        public TValue Get(TKey key)
        {
            if (!ContainsKey(key)) return default(TValue);
            else return this[key];
        }
    }
}
