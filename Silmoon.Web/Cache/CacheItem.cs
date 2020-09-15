using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Web.Cache
{
    public class CacheItem
    {
        public DateTime ExipreAt { get; set; }
        public object Value { get; set; }
        public string Key { get; set; }
    }
}
