using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Collections
{
    public class ConcurrentTwoWayDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, List<TValue>>>
    {
        private readonly ConcurrentDictionary<TKey, ConcurrentDictionary<TValue, byte>> _forward = new ConcurrentDictionary<TKey, ConcurrentDictionary<TValue, byte>>();
        private readonly ConcurrentDictionary<TValue, ConcurrentDictionary<TKey, byte>> _reverse = new ConcurrentDictionary<TValue, ConcurrentDictionary<TKey, byte>>();
        private readonly object _syncRoot = new ConcurrentDictionary<TKey, ConcurrentDictionary<TValue, byte>>();

        public void Add(TKey key, TValue value)
        {
            lock (_syncRoot)
            {
                var values = _forward.GetOrAdd(key, _ => new ConcurrentDictionary<TValue, byte>());
                values.TryAdd(value, 0);

                var keys = _reverse.GetOrAdd(value, _ => new ConcurrentDictionary<TKey, byte>());
                keys.TryAdd(key, 0);
            }
        }

        public bool Remove(TKey key, TValue value)
        {
            lock (_syncRoot)
            {
                var removed = false;

                if (_forward.TryGetValue(key, out var values))
                {
                    removed = values.TryRemove(value, out _);
                    if (values.IsEmpty)
                        _forward.TryRemove(key, out _);
                }

                if (_reverse.TryGetValue(value, out var keys))
                {
                    keys.TryRemove(key, out _);
                    if (keys.IsEmpty)
                        _reverse.TryRemove(value, out _);
                }

                return removed;
            }
        }

        public bool TryGetValues(TKey key, out List<TValue> values)
        {
            if (_forward.TryGetValue(key, out var valueSet))
            {
                values = new List<TValue>(valueSet.Keys);
                return true;
            }

            values = null;
            return false;
        }

        public List<TValue> GetValuesOrDefault(TKey key, List<TValue> defaultValue = null) => TryGetValues(key, out var values) ? values : defaultValue;

        public bool TryGetKeys(TValue value, out List<TKey> keys)
        {
            if (_reverse.TryGetValue(value, out var keySet))
            {
                keys = new List<TKey>(keySet.Keys);
                return true;
            }

            keys = null;
            return false;
        }

        public List<TKey> GetKeysOrDefault(TValue value, List<TKey> defaultValue = null) => TryGetKeys(value, out var keys) ? keys : defaultValue;

        public bool ContainsKey(TKey key) => _forward.ContainsKey(key);
        public bool ContainsValue(TValue value) => _reverse.ContainsKey(value);
        public IEnumerable<TKey> Keys => _forward.Keys;
        public int Count => _forward.Count;

        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
        {
            foreach (var kvp in _forward)
            {
                yield return new KeyValuePair<TKey, List<TValue>>(kvp.Key, new List<TValue>(kvp.Value.Keys));
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public List<TValue> this[TKey key] => GetValuesOrDefault(key, new List<TValue>());
    }
}
