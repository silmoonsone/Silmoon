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
    public class GlobalCaching<TKey, TValue>
    {
        static Dictionary<TKey, CacheObject<TKey, TValue>> Items { get; set; } = new Dictionary<TKey, CacheObject<TKey, TValue>>();

        public static void Set(TKey Key, TValue Value, TimeSpan ExpireTime)
        {
            Set(Key, Value, DateTime.Now.Add(ExpireTime));
        }
        public static void Set(TKey Key, TValue Value, DateTime? ExpireAt = null)
        {
            lock (Items)
            {
                if (ExpireAt == null) ExpireAt = DateTime.Now.AddHours(1);

                if (Items.TryGetValue(Key, out CacheObject<TKey, TValue> existingItem))
                {
                    existingItem.Value = Value;
                    existingItem.ExipreAt = ExpireAt.Value;
                }
                else
                {
                    var item = new CacheObject<TKey, TValue>() { Key = Key, Value = Value, ExipreAt = ExpireAt.Value };
                    Items.Add(Key, item);
                }
            }
        }
        public static (bool Matched, TValue Value) Get(TKey Key, TimeSpan? AddExpireTime = null)
        {
            lock (Items)
            {
                Cleanup();
                if (Items.ContainsKey(Key))
                {
                    var item = Items[Key];
                    if (AddExpireTime.HasValue) item.ExipreAt = item.ExipreAt.Add(AddExpireTime.Value);
                    return (true, item.Value);
                }
                else
                {
                    return (false, default);
                }
            }
        }
        public static (bool Matched, CacheObject<TKey, TValue> Item) GetInfo(TKey Key)
        {
            lock (Items)
            {
                Cleanup();
                if (Items.ContainsKey(Key))
                    return (true, Items[Key]);
                else return (false, default);
            }
        }
        public static bool Remove(TKey Key)
        {
            lock (Items)
            {
                return Items.ContainsKey(Key) && Items.Remove(Key);
            }
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
        public static IEnumerable<CacheObject<TKey, TValue>> GetValues()
        {
            lock (Items)
            {
                Cleanup();
                foreach (var item in Items)
                {
                    yield return item.Value;
                }
            }
        }
        static void Cleanup()
        {
            lock (Items)
            {
                List<TKey> readyToClears = new List<TKey>();
                foreach (var item in Items)
                {
                    if (item.Value.ExipreAt < DateTime.Now)
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
            lock (Items)
            {
                Items.Clear();
            }
        }
    }
}
