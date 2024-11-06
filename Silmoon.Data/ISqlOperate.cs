using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Silmoon.Data
{
    public interface ISqlOperate
    {
        ConnectionState State { get; }
        SqlConnection Connection { get; }
        int SelectCommandTimeout { get; set; }
        string Connectionstring { get; set; }
        int ExecuteNonQuery(string sqlcommand);
        int GetRecordCount(string sqlcommand);
        SqlDataReader GetDataReader(string sqlcommand);
        SqlCommand GetCommand(string sqlcommand);
        SqlDataAdapter GetDataAdapter(string sqlcommand);
        DataTable GetDataTable(string sqlcommand);
        object GetFieldObjectForSingleQuery(string tablename, string resulefield, string fieldname, string fieldvalue);
        object GetFieldObjectForSingleQuery(string sqlcommand, bool isUseReader);
        object GetFieldObjectForSingleQuery(string sqlcommand);
        int UpdateFieldForSingleQuery(string tablename, string updatefield, string updatevalue, string fieldname, string fieldvalue);
        bool ExistRecord(string sqlcommand);
        string ExistRecord(string sqlcommand, string fieldname);
    }
}
