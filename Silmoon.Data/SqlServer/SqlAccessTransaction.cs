using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.SqlServer
{
    public class SqlAccessTransaction : IDisposable
    {
        private SqlServerOperate SqlServerOperate = null;
        public SqlTransaction Transaction { get; set; }
        public SqlAccessTransaction(SqlServerOperate sqlServerOperate, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            SqlServerOperate = sqlServerOperate;
            Transaction = sqlServerOperate.Connection.BeginTransaction(isolationLevel);
        }
        public SqlAccessTransaction(SqlServerOperate sqlServerOperate, bool setCurrentTransaction = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            SqlServerOperate = sqlServerOperate;
            Transaction = sqlServerOperate.Connection.BeginTransaction(isolationLevel);
            if (setCurrentTransaction) SqlServerOperate.SetCurrentTranscation(Transaction);
        }

        public void Dispose()
        {
            Transaction.Dispose();
            SqlServerOperate.SetCurrentTranscation(null);
            SqlServerOperate = null;
            Transaction = null;
        }
    }
}
