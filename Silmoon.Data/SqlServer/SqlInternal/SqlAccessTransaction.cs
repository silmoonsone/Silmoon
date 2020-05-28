using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.SqlServer.SqlInternal
{
    public class SqlAccessTransaction : IDisposable
    {
        private SqlAccess access = null;
        public SqlTransaction Transaction { get; set; }
        public SqlAccessTransaction(SqlAccess access, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            this.access = access;
            Transaction = access.Connection.BeginTransaction(isolationLevel);
        }
        public SqlAccessTransaction(SqlAccess access, bool setCurrentTransaction = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            this.access = access;
            Transaction = access.Connection.BeginTransaction(isolationLevel);
            if (setCurrentTransaction) this.access.SetCurrentTranscation(Transaction);
        }

        public void Dispose()
        {
            Transaction.Dispose();
            this.access.SetCurrentTranscation(null);
            access = null;
            Transaction = null;
        }
    }
}
