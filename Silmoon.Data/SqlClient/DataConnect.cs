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
        protected static string sqlconnectionstr = null;
        public DataConnector()
        {

        }
        public DataConnector(string sqlconnectionstr = null)
        {
            DataConnector.sqlconnectionstr = sqlconnectionstr;
        }

        public void Dispose()
        {
            if (connect != null)
                connect.Dispose();
            connect = null;
        }

        public void Open()
        {
            connect = new SqlConnection(sqlconnectionstr);
            connect.Open();
        }
    }
}
