using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Silmoon.Data.SqlClient
{
    public class DataConnector: IDisposable
    {
        public SqlConnection connect = null;
        protected static string sqlConnectionString = null;
        public DataConnector()
        {

        }
        public DataConnector(string sqlConnectionString = null)
        {
            DataConnector.sqlConnectionString = sqlConnectionString;
        }

        public void Dispose()
        {
            if (connect != null)
                connect.Dispose();
            connect = null;
        }

        public void Open()
        {
            connect = new SqlConnection(sqlConnectionString);
            connect.Open();
        }
    }
}
