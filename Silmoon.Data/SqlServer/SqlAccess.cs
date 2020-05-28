using Silmoon.Data.SqlServer.SqlInternal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Silmoon.Data.SqlServer
{
    public class SqlAccess
    {
        public SqlTransaction Transaction { get; private set; }
        public SqlConnection Connection { get; private set; }

        public SqlAccess(SqlConnection connection)
        {
            Connection = connection;
        }

        public SqlDataAdapter GetAdapter(string commandText)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(GetCommand(commandText));
            return adapter;
        }
        public SqlDataAdapter GetAdapter(SqlCommand command)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            return adapter;
        }
        public SqlCommand GetCommand(string commandText)
        {
            SqlCommand cmd = new SqlCommand(commandText, Connection);
            cmd.Transaction = Transaction;
            return cmd;
        }
        public DataTable GetTable(SqlCommand command)
        {
            DataTable dt = new DataTable();
            var adapter = GetAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public SqlAccessTransaction BeginTransaction(bool setCurrentTransaction = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new SqlAccessTransaction(this, setCurrentTransaction, isolationLevel);
        }
        public void CommitTransaction(SqlAccessTransaction transaction)
        {
            transaction.Transaction.Commit();
        }
        public void RollbackTransaction(SqlAccessTransaction transaction)
        {
            transaction.Transaction.Rollback();
        }
        public void SetCurrentTranscation(SqlTransaction transaction)
        {
            Transaction = transaction;
        }
    }
}
