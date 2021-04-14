using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silmoon.Runtime.Cache
{
    /// <summary>
    /// 提供对象在静态类型中的缓存服务。默认缓存超时时间为1小时。
    /// </summary>
    public class ObjectCache<TKey, TValue>
    {
        static Dictionary<TKey, CacheItem<TKey, TValue>> Items { get; set; } = new Dictionary<TKey, CacheItem<TKey, TValue>>();

        public static void Set(TKey Key, TValue Value, TimeSpan ExpireTime)
        {
            Set(Key, Value, DateTime.Now.Add(ExpireTime));
        }
        public static void Set(TKey Key, TValue Value, DateTime? ExpireAt = null)
        {
            lock (Items)
            {
                if (ExpireAt == null) ExpireAt = DateTime.Now.AddHours(1);

                if (Items.ContainsKey(Key))
                {
                    var item = Items[Key];
                    if (item.ExipreAt == default)
                    {
                        item = new CacheItem<TKey, TValue>() { Key = Key, Value = Value, ExipreAt = ExpireAt.Value };
                        Items[Key] = item;
                    }
                    else
                    {
                        item.Value = Value;
                        item.ExipreAt = ExpireAt.Value;
                    }
                }
                else
                {
                    var item = new CacheItem<TKey, TValue>() { Key = Key, Value = Value, ExipreAt = ExpireAt.Value };
                    Items.Add(Key, item);
                }
            }
        }
        public static (bool Matched, TValue Value) Get(TKey Key, TimeSpan? AddExpireTime = null)
        {
            Clearup();
            lock (Items)
            {
                if (Items.ContainsKey(Key))
                {
                    var item = Items[Key];
                    if (AddExpireTime.HasValue) item.ExipreAt.Add(AddExpireTime.Value);
                    return (true, item.Value);
                }
                else
                {
                    return (false, default);
                }
            }
        }
        public static (bool Matched, CacheItem<TKey, TValue> Item) GetInfo(TKey Key)
        {
            Clearup();
            lock (Items)
            {
                if (Items.ContainsKey(Key))
                    return (true, Items[Key]);
                else return (false, default);
            }
        }
        public static bool Remove(TKey Key)
        {
            if (Items.ContainsKey(Key))
                return Items.Remove(Key);
            else { return false; }
        }
        public TValue this[TKey Key]
        {
            get
            {
                return Get(Key).Value;
            }
            set
            {
                Set(Key, value);
            }
        }
        public static IEnumerable<CacheItem<TKey, TValue>> GetValues()
        {
            Clearup();
            lock (Items)
            {
                foreach (var item in Items)
                {
                    yield return item.Value;
                }
            }
        }
        static void Clearup()
        {
            List<TKey> readyToClears = new List<TKey>();

            lock (Items)
            {
                foreach (var item in Items)
                {
                    if (item.Value.Value == null)
                    {
                        readyToClears.Add(item.Key);
                    }
                    else if (item.Value.ExipreAt < DateTime.Now)
                    {
                        readyToClears.Add(item.Key);
                    }
                }

                foreach (var item in readyToClears)
                {
                    Items.Remove(item);
                }
            }
        }
        public static void Clear()
        {
            Items.Clear();
        }
    }
}
