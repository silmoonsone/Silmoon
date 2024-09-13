using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Silmoon.Data.Models
{
    public interface ISqlObject
    {
        int id { get; set; }
        [DisplayName("创建日期")]
        DateTime created_at { get; set; }
    }
}
