using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTesting
{
    public interface ISqlObject
    {
        int id { get; set; }

        [DisplayName("创建日期")]
        DateTime created_at { get; set; }
    }
}
