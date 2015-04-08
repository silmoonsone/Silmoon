using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data
{
    public class ConnectionStringMaker
    {
        public string Make_SQL_ConnectionString(string server, string username, string password,string database, bool useSSPI)
        {
            if (useSSPI)
            {
                return "Server=" + server + ";Database=" + database + ";Integrated Security=SSPI";
            }
            else
            {
                return "Server=" + server + ";UID=" + username + ";PWD=" + password + ";Database=" + database;
            }
        }
    }
}
