using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;

namespace Silmoon.Data.SqlClient
{
    public class SmMSSQLClient : SqlCommonTemplate,IDisposable,ISMSQL
    {
        SqlConnection con = null;

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
        public SqlConnection Connection
        {
            get { return con; }
            set { con = value; }
        }

        /// <summary>
        /// 获取SQL地连接状态
        /// </summary>
        public ConnectionState State
        {
            get { return con.State; }
        }

        /// <summary>
        /// 创建MS SQL数据源的实例
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public SmMSSQLClient()
        {
            con = new SqlConnection();
        }
        /// <summary>
        /// 创建MS SQL数据源的实例
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public SmMSSQLClient(string constr)
        {
            con = new SqlConnection();
            conStr = constr;
        }
        /// <summary>
        /// 创建MS SQL数据源的实例
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public SmMSSQLClient(SqlConnection conn)
        {
            con = conn;
        }


        /// <summary>
        /// 关闭数据库连接并且释放连接对象。
        /// </summary>
        public void Close()
        {
            if (State != ConnectionState.Closed)
            {
                con.Close();
            }
        }
        /// <summary>
        /// 使用默认连接并且打开一个数据库
        /// </summary>
        public void Open()
        {
            if (State == ConnectionState.Closed)
            {
                con.ConnectionString = conStr;
                con.Open();
            }
        }

        /// <summary>
        /// 执行一个没有返回或不需要返回的SQL，并且返回相应行数
        /// </summary>
        /// <returns></returns>
        public int ExecNonQuery(string sqlcommand)
        {
            int reint = 0;
            SqlCommand myCmd = new SqlCommand(__chkSqlstr(sqlcommand), con);
            reint = myCmd.ExecuteNonQuery();
            myCmd.Dispose();
            return reint;
        }
        /// <summary>
        /// 返回数据结果行数
        /// </summary>
        /// <param name="sqlcommand">查询语句</param>
        /// <returns></returns>
        public int GetRecordCount(string sqlcommand)
        {
            DataTable dt = GetDataTable(sqlcommand);
            int i = dt.Rows.Count;
            dt.Dispose();
            return i;
        }

        /// <summary>
        /// 返回一个SqlDataReader对象
        /// </summary>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        public object GetDataReader(string sqlcommand)
        {
            return new SqlCommand(__chkSqlstr(sqlcommand), con).ExecuteReader();
        }
        /// <summary>
        /// 返回一个SqlCommand对象
        /// </summary>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        public object GetCommand(string sqlcommand)
        {
            return new SqlCommand(__chkSqlstr(sqlcommand), con);
        }
        /// <summary>
        /// 获取一个数据适配器。
        /// </summary>
        /// <param name="sqlcommand">SQL语句</param>
        /// <returns></returns>
        public object GetDataAdapter(string sqlcommand)
        {
            return new SqlDataAdapter(__chkSqlstr(sqlcommand), con);
        }
        /// <summary>
        /// 获取一个内存数据表
        /// </summary>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sqlcommand)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = (SqlDataAdapter)GetDataAdapter(sqlcommand);
            da.SelectCommand.CommandTimeout = selectCommandTimeout;
            da.Fill(dt);
            da.Dispose();
            return dt;
        }

        /// <summary>
        /// 返回一个从数据库里面查询出来的字段值
        /// </summary>
        /// <param name="tablename">表</param>
        /// <param name="resulefield">字段</param>
        /// <param name="fieldname">条件字段</param>
        /// <param name="fieldvalue">条件值</param>
        /// <returns></returns>
        public object GetFieldObjectForSingleQuery(string tablename, string resulefield, string fieldname, string fieldvalue)
        {
            object reobj;
            SqlDataReader dr = (SqlDataReader)GetDataReader("select " + resulefield + " from [" + tablename + "] where " + fieldname + " = " + fieldvalue);
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
        /// <summary>
        /// 返回一个从数据库里面查询出来的字段值
        /// </summary>
        /// <param name="sqlcommand">SQL查询命令</param>
        /// <param name="isUseReader">是否使用DataReader进行工作</param>
        /// <returns></returns>
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
        /// <summary>
        /// 返回一个从数据库里面查询出来的字段值
        /// </summary>
        /// <param name="sqlcommand">SQL命令，仅需指定一个返回字段</param>
        /// <returns></returns>
        public object GetFieldObjectForSingleQuery(string sqlcommand)
        {
            object reobj;
            SqlDataReader dr = (SqlDataReader)GetDataReader(sqlcommand);
            if (dr.Read())
            { reobj = dr[0]; }
            else
            { reobj = null; }
            dr.Close();
            dr.Dispose();
            return reobj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="updatefield"></param>
        /// <param name="updatevalue"></param>
        /// <param name="fieldname"></param>
        /// <param name="fieldvalue"></param>
        public int UpdateFieldForSingleQuery(string tablename, string updatefield, string updatevalue, string fieldname, string fieldvalue)
        {
            return ExecNonQuery("Update [" + tablename + "] set " + updatefield + " = " + updatevalue + " where " + fieldname + " = " + fieldvalue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlcommand"></param>
        /// <returns></returns>
        public bool ExistRecord(string sqlcommand)
        {
            bool rebool = false;
            SqlDataReader dr = (SqlDataReader)GetDataReader(sqlcommand);
            if (dr.Read())
            { rebool = true; }
            else { rebool = false; }
            dr.Close();
            return rebool;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlcommand"></param>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public string ExistRecord(string sqlcommand, string fieldname)
        {
            string restring = "";
            SqlDataReader dr = (SqlDataReader)GetDataReader(sqlcommand);
            if (dr.Read())
            { restring = dr[fieldname].ToString(); }
            else { restring = null; }
            dr.Close();
            return restring;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object GetConnection()
        {
            return con;
        }


        public void Dispose()
        {
            Close();
            con.Close();
            con.Dispose();
            con = null;
        }

        public string __chkSqlstr(string sqlcommand)
        {
            //HttpContext.Current.Response.Write(sqlcommand);
            return sqlcommand;
        }
    }
    public sealed class SmOleDb : IDisposable
    {
        string sqlCommand;
        OleDbConnection con;
        string conStr;
        bool isConnect;

        /// <summary>
        /// 获取对象是否已经创建数据库对象。
        /// </summary>
        public bool Isconnect
        {
            get
            {
                return isConnect;
            }
        }

        /// <summary>
        /// 类的构造函数
        /// </summary>
        public SmOleDb(string constr)
        {
            isConnect = false;
            conStr = constr;
            InitClass();
        }

        private void InitClass()
        {

        }

        /// <summary>
        /// 关闭数据库连接并且释放连接对象。
        /// </summary>
        public void Close()
        {
            con.Close();
            con = null;
            isConnect = false;
        }
        /// <summary>
        /// 使用默认连接并且打开一个数据库
        /// </summary>
        public void Open()
        {
            con = new OleDbConnection(conStr);
            con.Open();
            isConnect = true;
        }


        /// <summary>
        /// 设置或者获取SQL执行命令。
        /// </summary>
        public string SqlCommand
        {
            get { return sqlCommand; }
            set { sqlCommand = value; }
        }
        /// <summary>
        /// 执行一个没有返回或不需要返回的SQL，并且返回相应行数
        /// </summary>
        /// <returns></returns>
        public int ExecNonQuery()
        {
            int reint = 0;
            if (isConnect)
            {
                OleDbCommand myCmd = new OleDbCommand(sqlCommand, con);
                reint = myCmd.ExecuteNonQuery();
                myCmd.Dispose();
            }
            else
            {
                throw new Exception("数据库没有连接");
            }
            return reint;
        }
        /// <summary>
        /// 执行一个没有返回或不需要返回的SQL，并且返回相应行数
        /// </summary>
        /// <returns></returns>
        public int ExecNonQuery(string sqlcommand)
        {
            int reint = 0;
            if (isConnect)
            {
                OleDbCommand myCmd = new OleDbCommand(sqlcommand, con);
                reint = myCmd.ExecuteNonQuery();
                myCmd.Dispose();
            }
            else
            {
                throw new Exception("数据库没有连接");
            }
            return reint;
        }

        /// <summary>
        /// 返回SQL查询SqlDataReader类型结果。
        /// </summary>
        /// <returns></returns>
        public OleDbDataReader GetOleDbDataReader()
        {
            return new OleDbCommand(sqlCommand, con).ExecuteReader();
        }
        public OleDbDataReader GetOleDbDataReader(string sqlcommand)
        {
            return new OleDbCommand(sqlcommand, con).ExecuteReader();
        }
        public OleDbCommand GetOleDbCommand(string sqlcommand)
        {
            return new OleDbCommand(sqlcommand, con);
        }
        public object GetFieldObjectForSingleQuery(string tablename, string resulefield, string fieldname, string fieldvalue)
        {
            object reobj;
            OleDbDataReader dr = GetOleDbDataReader("select " + resulefield + " from [" + tablename + "] where " + fieldname + " = " + fieldvalue);
            if (dr.Read())
            { reobj = dr[0]; }
            else
            {
                Close();
                throw new Exception("无返回数据");
            }
            dr.Close();
            dr.Dispose();
            return reobj;
        }
        public void UpdateFieldForSingleQuery(string tablename, string updatefield, string updatevalue, string fieldname, string fieldvalue)
        {
            ExecNonQuery("Update [" + tablename + "] set " + updatefield + " = " + updatevalue + " where " + fieldname + " = " + fieldvalue);
        }
        public bool ExistRecord(string sqlcommand)
        {
            bool rebool = false;
            OleDbDataReader dr = GetOleDbDataReader(sqlcommand);
            if (dr.Read())
            { rebool = true; }
            else { rebool = false; }
            dr.Close();
            return rebool;
        }
        public string ExistRecord(string sqlcommand, string fieldname)
        {
            string restring = "";
            OleDbDataReader dr = GetOleDbDataReader(sqlcommand);
            if (dr.Read())
            { restring = dr[fieldname].ToString(); }
            else { restring = null; }
            dr.Close();
            return restring;
        }
        public OleDbDataAdapter GetOleDbAdapter(string sqlcommand)
        {
            return new OleDbDataAdapter(sqlcommand, con);
        }
        /// <summary>
        /// 获取一个内存数据表
        /// </summary>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        public DataTable GetOleDbDataTable(string sqlcommand)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = GetOleDbAdapter(sqlcommand);
            da.Fill(dt);
            da.Dispose();
            return dt;
        }
        public OleDbConnection GetOleDbConnection()
        {
            return con;
        }

        public static string Constr(string mdbPath)
        {
            return @"Provider=Microsoft.Jet.OLEDB.4.0;Data source= " + mdbPath;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
    public sealed class SmOracleClient : IDisposable
    {
        string sqlCommand;
        OracleConnection con;
        string conStr;
        bool isConnect;


        /// <summary>
        /// 获取对象是否已经创建数据库对象。
        /// </summary>
        public bool Isconnect
        {
            get
            {
                return isConnect;
            }
        }

        /// <summary>
        /// 类的构造函数
        /// </summary>
        public SmOracleClient(string constr)
        {
            isConnect = false;
            conStr = constr;
            InitClass();
        }

        private void InitClass()
        {

        }

        /// <summary>
        /// 关闭数据库连接并且释放连接对象。
        /// </summary>
        public void Close()
        {
            con.Close();
            con = null;
            isConnect = false;
        }
        /// <summary>
        /// 使用默认连接并且打开一个数据库
        /// </summary>
        public void Open()
        {
            con = new OracleConnection(conStr);
            con.Open();
            isConnect = true;
        }


        /// <summary>
        /// 设置或者获取SQL执行命令。
        /// </summary>
        public string SqlCommand
        {
            get { return sqlCommand; }
            set { sqlCommand = value; }
        }
        /// <summary>
        /// 执行一个没有返回或不需要返回的SQL，并且返回相应行数
        /// </summary>
        /// <returns></returns>
        public int ExecNonQuery()
        {
            int reint = 0;
            if (isConnect)
            {
                OracleCommand myCmd = new OracleCommand(sqlCommand, con);
                reint = myCmd.ExecuteNonQuery();
                myCmd.Dispose();
            }
            else
            {
                throw new Exception("数据库没有连接");
            }
            return reint;
        }
        /// <summary>
        /// 执行一个没有返回或不需要返回的SQL，并且返回相应行数
        /// </summary>
        /// <returns></returns>
        public int ExecNonQuery(string sqlcommand)
        {
            int reint = 0;
            if (isConnect)
            {
                OracleCommand myCmd = new OracleCommand(sqlcommand, con);
                reint = myCmd.ExecuteNonQuery();
                myCmd.Dispose();
            }
            else
            {
                throw new Exception("数据库没有连接");
            }
            return reint;
        }
        /// <summary>
        /// 返回SQL查询SqlDataReader类型结果。
        /// </summary>
        /// <returns></returns>
        public OracleDataReader SqlDataRead()
        {
            return new OracleCommand(sqlCommand, con).ExecuteReader();
        }
        public OracleDataReader SqlDataRead(string sqlcommand)
        {
            return new OracleCommand(sqlcommand, con).ExecuteReader();
        }
        public OracleCommand SqlCommander(string sqlcommand)
        {
            return new OracleCommand(sqlcommand, con);
        }
        public object GetFieldObjectForSingleQuery(string tablename, string resulefield, string fieldname, string fieldvalue)
        {
            object reobj;
            OracleDataReader dr = SqlDataRead("select " + resulefield + " from [" + tablename + "] where " + fieldname + " = " + fieldvalue);
            if (dr.Read())
            { reobj = dr[0]; }
            else
            {
                Close();
                throw new Exception("无返回数据");
            }
            dr.Close();
            dr.Dispose();
            return reobj;
        }
        public void UpdateFieldForSingleQuery(string tablename, string updatefield, string updatevalue, string fieldname, string fieldvalue)
        {
            ExecNonQuery("Update [" + tablename + "] set " + updatefield + " = " + updatevalue + " where " + fieldname + " = " + fieldvalue);
        }
        public bool ExSqlRecordSet(string sqlcommand)
        {
            bool rebool = false;
            OracleDataReader dr = SqlDataRead(sqlcommand);
            if (dr.Read())
            { rebool = true; }
            else { rebool = false; }
            dr.Close();
            return rebool;
        }
        public OracleDataAdapter DataAdapter(string sqlString)
        {
            return new OracleDataAdapter(sqlString, con);
        }


        public static string Constr(string mdbPath)
        {
            return @"Provider=Microsoft.Jet.OLEDB.4.0;Data source= " + mdbPath;
        }


        public void Dispose()
        {
            GC.Collect();
        }
    }
}
