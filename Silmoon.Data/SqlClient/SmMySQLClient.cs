using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;

namespace Silmoon.Data.SqlClient
{
    public class SmMySqlClient : SqlCommonTemplate, IDisposable, ISMSQL
    {
        MySqlConnection con = null;

        string conStr;
        int selectCommandTimeout = 30;

        /// <summary>
        /// 在使用数据适配器的时候，执行SELECT查询的超时时间。
        /// </summary>
        public int SelectCommandTimeout
        {
            get { return selectCommandTimeout; }
            set { selectCommandTimeout = value; }
        }
        public string Connectionstring
        {
            get { return conStr; }
            set { conStr = value; }
        }
        public MySqlConnection Connection
        {
            get { return con; }
            set { con = value; }
        }

        public SmMySqlClient()
        {

        }
        public SmMySqlClient(string conStr)
        {

            this.conStr = conStr;
        }
        public SmMySqlClient(MySqlConnection conn)
        {
            con = conn;
        }

        #region ISMSQL 成员
        public System.Data.ConnectionState State
        {
            get
            {
                return con.State;
            }
        }
        public void Open()
        {
            if (con == null) con = new MySqlConnection();
            if (State == ConnectionState.Closed)
            {
                con.ConnectionString = conStr;
                con.Open();
            }
        }
        public void Close()
        {
            if (State != ConnectionState.Closed)
            {
                con.Close();
            }
        }
        public int ExecNonQuery(string sqlcommand)
        {
            int reint = 0;
            MySqlCommand myCmd = new MySqlCommand(__chkSqlstr(sqlcommand), con);
            reint = myCmd.ExecuteNonQuery();
            myCmd.Dispose();
            return reint;
        }
        public int GetRecordCount(string sqlcommand)
        {
            DataTable dt = GetDataTable(sqlcommand);
            int i = dt.Rows.Count;
            dt.Dispose();
            return i;
        }
        public object GetDataReader(string sqlcommand)
        {
            return ((MySqlCommand)GetCommand(sqlcommand)).ExecuteReader();
        }
        public object GetCommand(string sqlcommand)
        {
            return new MySqlCommand(__chkSqlstr(sqlcommand), con);
        }
        public object GetDataAdapter(string sqlcommand)
        {
            return new MySqlDataAdapter(__chkSqlstr(sqlcommand), con);
        }
        public DataTable GetDataTable(string sqlcommand)
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter da = (MySqlDataAdapter)GetDataAdapter(sqlcommand);
            da.SelectCommand.CommandTimeout = selectCommandTimeout;
            da.Fill(dt);
            da.Dispose();
            return dt;
        }
        public object GetFieldObjectForSingleQuery(string tablename, string resulefield, string fieldname, string fieldvalue)
        {
            object reobj;
            MySqlDataReader dr = (MySqlDataReader)GetDataReader("select " + resulefield + " from [" + tablename + "] where " + fieldname + " = " + fieldvalue);
            if (dr.Read())
            { reobj = dr[0]; }
            else
            {
                Close();
                reobj = null;
            }
            dr.Close();
            dr.Dispose();
            return reobj;
        }
        public object GetFieldObjectForSingleQuery(string sqlcommand, bool isUseReader)
        {
            if (isUseReader) return GetFieldObjectForSingleQuery(sqlcommand);
            else
            {
                DataTable dt = GetDataTable(sqlcommand);
                object returnObj = null;
                if (dt.Rows.Count != 0)
                    returnObj = dt.Rows[0][0];
                dt.Clear();
                dt.Dispose();
                return returnObj;
            }
        }
        public object GetFieldObjectForSingleQuery(string sqlcommand)
        {
            object reobj;
            MySqlDataReader dr = (MySqlDataReader)GetDataReader(sqlcommand);
            if (dr.Read())
            { reobj = dr[0]; }
            else
            { reobj = null; }
            dr.Close();
            dr.Dispose();
            return reobj;
        }
        public int UpdateFieldForSingleQuery(string tablename, string updatefield, string updatevalue, string fieldname, string fieldvalue)
        {
            return ExecNonQuery("Update [" + tablename + "] set " + updatefield + " = " + updatevalue + " where " + fieldname + " = " + fieldvalue);
        }
        public bool ExistRecord(string sqlcommand)
        {
            bool rebool = false;
            MySqlDataReader dr = (MySqlDataReader)GetDataReader(sqlcommand);
            if (dr.Read())
            { rebool = true; }
            else { rebool = false; }
            dr.Close();
            return rebool;
        }
        public string ExistRecord(string sqlcommand, string fieldname)
        {
            string restring = "";
            MySqlDataReader dr = (MySqlDataReader)GetDataReader(sqlcommand);
            if (dr.Read())
            { restring = dr[fieldname].ToString(); }
            else { restring = null; }
            dr.Close();
            return restring;
        }
        public object GetConnection()
        {
            return con;
        }
        public string __chkSqlstr(string sqlcommand)
        {
            return sqlcommand;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Close();
            con.Close();
            con.Dispose();
            con = null;
        }

        #endregion
    }
}
