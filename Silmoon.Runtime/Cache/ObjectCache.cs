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
    public class ObjectCache
    {
        public static Dictionary<string, CacheItem> Items { get; set; } = new Dictionary<string, CacheItem>();

        public static void Set(string Key, object Value, DateTime? ExpireAt = null)
        {
            lock (Items)
            {
                if (ExpireAt == null) ExpireAt = DateTime.Now.AddHours(1);

                if (Items.ContainsKey(Key))
                {
                    var item = Items[Key];
                    if (item == null)
                    {
                        item = new CacheItem() { Key = Key, Value = Value, ExipreAt = ExpireAt.Value };
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
                    var item = new CacheItem() { Key = Key, Value = Value, ExipreAt = ExpireAt.Value };
                    Items.Add(Key, item);
                }
            }
        }
        public static dynamic Get(string Key, TimeSpan? DelayExpire = null)
        {
            Clearup();
            lock (Items)
            {
                if (Items.ContainsKey(Key))
                {
                    var item = Items[Key];
                    if (DelayExpire.HasValue) item.ExipreAt.Add(DelayExpire.Value);
                    return item.Value;
                }
                else
                {
                    return null;
                }
            }
        }
        public object this[string Key]
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
            List<string> readyToClears = new List<string>();

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
