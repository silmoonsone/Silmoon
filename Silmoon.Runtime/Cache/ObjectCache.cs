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
        public static Dictionary<TKey, CacheItem<TKey, TValue>> Items { get; set; } = new Dictionary<TKey, CacheItem<TKey, TValue>>();

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
        public static TValue Get(TKey Key, TimeSpan? AddExpireTime = null)
        {
            Clearup();
            lock (Items)
            {
                if (Items.ContainsKey(Key))
                {
                    var item = Items[Key];
                    if (AddExpireTime.HasValue) item.ExipreAt.Add(AddExpireTime.Value);
                    return item.Value;
                }
                else
                {
                    return default;
                }
            }
        }
        public static object GetInfo(TKey Key)
        {
            lock (Items)
            {
                if (Items.ContainsKey(Key))
                    return Items[Key];
                else return null;
            }
        }
        public TValue this[TKey Key]
        {
            get
            {
                return Get(Key);
            }
            set
            {
                Set(Key, value);
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
    }
}
