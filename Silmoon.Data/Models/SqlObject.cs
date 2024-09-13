using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Silmoon.Data.Models
{
    [Serializable]
    public class SqlObject : ISqlObject
    {
        public int id { get; set; }
        [DisplayName("创建日期")]
        public virtual DateTime created_at { get; set; } = DateTime.Now;
    }
}
