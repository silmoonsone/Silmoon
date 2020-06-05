using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.SqlServer
{
    public class SqlExecuteResult
    {
        public SqlExecuteResult()
        {

        }
        public SqlExecuteResult(int ResponseRows, string ExecuteSqlString)
        {
            this.ResponseRows = ResponseRows;
            this.ExecuteSqlString = ExecuteSqlString;
        }
        public int ResponseRows { get; set; }
        public string ExecuteSqlString { get; set; }
    }
    public class SqlExecuteResult<T> : SqlExecuteResult
    {
        public SqlExecuteResult()
        {

        }
        public SqlExecuteResult(int ResponseRows, string ExecuteSqlString, T Data) : base(ResponseRows, ExecuteSqlString)
        {
            this.Data = Data;
        }
        public T Data { get; set; }
    }
}
