using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.SqlServer
{
    public class SqlExecuteResult
    {
        public int ResponseRows { get; set; }
        public string ExecuteSqlString { get; set; }
    }
    public class SqlExecuteResult<T> : SqlExecuteResult
    {
        public T Data { get; set; }
    }
}
