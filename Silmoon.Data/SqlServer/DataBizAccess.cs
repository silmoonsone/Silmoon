using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Silmoon.Data.SqlServer
{
    public class DataBizAccess : IDisposable
    {
        public SqlTransaction Transaction { get; private set; }
        public DataConnector DataConnector { get; set; }

        public DataBizAccess()
        {

        }
        public DataBizAccess(DataConnector dc)
        {
            this.DataConnector = dc;
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
        public SqlCommand GetCommand(string commandText, SqlConnection connect = null)
        {
            if (connect == null) connect = DataConnector.connect;
            SqlCommand cmd = new SqlCommand(commandText, connect);
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
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (Transaction == null)
                Transaction = DataConnector.connect.BeginTransaction(isolationLevel);
        }
        public void CommitTransaction()
        {
            if (Transaction != null)
                Transaction.Commit();
            Transaction = null;
        }
        public void RollbackTransaction()
        {
            if (Transaction != null)
                Transaction.Rollback();
            Transaction = null;
        }

        public void Dispose()
        {
            DataConnector.Dispose();
            DataConnector = null;
        }
    }
}
