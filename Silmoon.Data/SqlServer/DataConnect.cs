using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Silmoon.Data.SqlServer
{
    public class DataConnector : IDisposable
    {
        public SqlConnection Connection = null;
        protected static string SqlConnectionString = null;
        public DataConnector()
        {

        }
        public DataConnector(string sqlConnectionString = null)
        {
            DataConnector.SqlConnectionString = sqlConnectionString;
        }

        public void Dispose()
        {
            if (Connection != null)
                Connection.Dispose();
            Connection = null;
        }

        public void Open(string sqlConnectionString = null)
        {
            if (sqlConnectionString != null) SqlConnectionString = sqlConnectionString;
            if (SqlConnectionString == null)
                throw new ArgumentException("连接字符串ConnectionString未设置", "SqlConnectionString");
            Connection = new SqlConnection(SqlConnectionString);
            Connection.Open();
        }
    }
}
