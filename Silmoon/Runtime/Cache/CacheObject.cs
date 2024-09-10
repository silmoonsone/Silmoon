using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Runtime.Cache
{
    [Serializable]
    public class CacheObject<TKey, TValue>
    {
        public DateTime ExipreAt { get; set; }
        public TValue Value { get; set; }
        public TKey Key { get; set; }
    }
}
