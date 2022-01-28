using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using Microsoft.Data.SqlClient;

namespace Silmoon.Data.SqlServer
{
    public class SqlUtil : SqlCommonTemplate, IDisposable, ISqlOperate
    {
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
        public SqlConnection Connection { get; set; }


        /// <summary>
        /// 获取SQL地连接状态
        /// </summary>
        public ConnectionState State
        {
            get { return Connection.State; }
        }

        /// <summary>
        /// 创建MS SQL数据源的实例
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public SqlUtil()
        {
            Connection = new SqlConnection();
        }
        /// <summary>
        /// 创建MS SQL数据源的实例
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public SqlUtil(string constr)
        {
            Connection = new SqlConnection();
            conStr = constr;
        }
        /// <summary>
        /// 创建MS SQL数据源的实例
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public SqlUtil(SqlConnection conn)
        {
            Connection = conn;
        }


        /// <summary>
        /// 关闭数据库连接并且释放连接对象。
        /// </summary>
        public void Close()
        {
            if (State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }
        /// <summary>
        /// 使用默认连接并且打开一个数据库
        /// </summary>
        public void Open()
        {
            if (State == ConnectionState.Closed)
            {
                Connection.ConnectionString = conStr;
                Connection.Open();
            }
        }

        /// <summary>
        /// 执行一个没有返回或不需要返回的SQL，并且返回相应行数
        /// </summary>
        /// <returns></returns>
        public int ExecNonQuery(string sqlcommand)
        {
            int reint = 0;
            SqlCommand myCmd = new SqlCommand(__chkSqlstr(sqlcommand), Connection);
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
            using (DataTable dt = GetDataTable(sqlcommand))
            {
                int i = dt.Rows.Count;
                return i;
            }
        }

        /// <summary>
        /// 返回一个SqlDataReader对象
        /// </summary>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        public object GetDataReader(string sqlcommand)
        {
            return new SqlCommand(__chkSqlstr(sqlcommand), Connection).ExecuteReader();
        }
        /// <summary>
        /// 返回一个SqlCommand对象
        /// </summary>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        public object GetCommand(string sqlcommand)
        {
            return new SqlCommand(__chkSqlstr(sqlcommand), Connection);
        }
        /// <summary>
        /// 获取一个数据适配器。
        /// </summary>
        /// <param name="sqlcommand">SQL语句</param>
        /// <returns></returns>
        public object GetDataAdapter(string sqlcommand)
        {
            return new SqlDataAdapter(__chkSqlstr(sqlcommand), Connection);
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
            return Connection;
        }


        public void Dispose()
        {
            Close();
            Connection.Dispose();
            Connection = null;
        }

        public string __chkSqlstr(string sqlcommand)
        {
            //HttpContext.Current.Response.Write(sqlcommand);
            return sqlcommand;
        }
    }
}
