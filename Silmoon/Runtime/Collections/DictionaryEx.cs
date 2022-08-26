using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Silmoon.Runtime.Collections
{
    [Serializable]
    public class DictionaryEx<TKey, TValue> : Dictionary<TKey, TValue>, ISerializable
    {
        public DictionaryEx()
        {

        }
        public DictionaryEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
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
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
