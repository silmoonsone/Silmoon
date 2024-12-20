using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using Microsoft.Data.SqlClient;
using System.Transactions;
using IsolationLevel = System.Data.IsolationLevel;
using System.Reflection;

namespace Silmoon.Data.SqlServer
{
    public class SqlServerOperate : IDisposable
    {
        private SqlTransaction SqlTransaction { get; set; }

        string ConnectionString { get; set; }
        int selectCommandTimeout { get; set; } = 30;

        /// <summary>
        /// 在使用数据适配器的时候，执行SELECT查询的超时时间。
        /// </summary>
        public int SelectCommandTimeout
        {
            get { return selectCommandTimeout; }
            set { selectCommandTimeout = value; }
        }

        public SqlConnection Connection { get; private set; }


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
        public SqlServerOperate()
        {
            Connection = new SqlConnection();
            Connection.Open();
        }
        /// <summary>
        /// 创建MS SQL数据源的实例
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerOperate(string connectionString)
        {
            Connection = new SqlConnection();
            ConnectionString = connectionString;
        }
        /// <summary>
        /// 创建MS SQL数据源的实例
        /// </summary>
        /// <param name="connection">SqlConnection连接实例</param>
        public SqlServerOperate(SqlConnection connection)
        {
            Connection = connection;
        }


        /// <summary>
        /// 返回一个SqlDataReader对象
        /// </summary>
        /// <param name="sqlCommandText">SQL命令</param>
        /// <returns></returns>
        public SqlDataReader GetDataReader(string sqlCommandText) => new SqlCommand(sqlCommandText, Connection, SqlTransaction).ExecuteReader();
        /// <summary>
        /// 返回一个SqlCommand对象
        /// </summary>
        /// <param name="sqlCommandText">SQL命令</param>
        /// <returns></returns>
        public SqlCommand GetDataCommand(string sqlCommandText) => new SqlCommand(sqlCommandText, Connection, SqlTransaction);
        /// <summary>
        /// 获取一个数据适配器。
        /// </summary>
        /// <param name="sqlCommandText">SQL语句</param>
        /// <returns></returns>
        public SqlDataAdapter GetDataAdapter(string sqlCommandText) => new SqlDataAdapter(sqlCommandText, Connection);
        /// <summary>
        /// 获取一个内存数据表
        /// </summary>
        /// <param name="sqlCommandText">SQL命令</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sqlCommandText)
        {
            using (SqlDataAdapter dataAdapter = GetDataAdapter(sqlCommandText))
            {
                DataTable result = new DataTable();
                dataAdapter.Fill(result);
                return result;
            }
        }


        /// <summary>
        /// 执行一个没有返回或不需要返回的SQL，并且返回响应行数
        /// </summary>
        /// <param name="sqlCommandText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlCommandText)
        {
            using (var sqlCommand = GetDataCommand(sqlCommandText))
            {
                return sqlCommand.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 返回数据结果行数
        /// </summary>
        /// <param name="sqlCommandText">查询语句</param>
        /// <returns></returns>
        public int GetRecordCount(string sqlCommandText)
        {
            using (var cmd = GetDataCommand(sqlCommandText))
            {
                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }


        /// <summary>
        /// 返回一个从数据库里面查询出来的字段值
        /// </summary>
        /// <param name="tableName">表</param>
        /// <param name="resultField">字段</param>
        /// <param name="fieldName">条件字段</param>
        /// <param name="fieldValue">条件值</param>
        /// <returns></returns>
        public object GetFieldObjectForSingleQuery(string tableName, string resultField, string fieldName, string fieldValue)
        {
            using (SqlDataReader sqlDataReader = GetDataReader("select " + resultField + " from [" + tableName + "] where " + fieldName + " = " + fieldValue))
            {
                if (sqlDataReader.Read())
                    return sqlDataReader[0];
                else return null;
            }
        }
        /// <summary>
        /// 返回一个从数据库里面查询出来的字段值
        /// </summary>
        /// <param name="sqlCommandText">SQL查询命令</param>
        /// <param name="isUseReader">是否使用DataReader进行工作</param>
        /// <returns></returns>
        public object GetFieldObjectForSingleQuery(string sqlCommandText, bool isUseReader)
        {
            if (isUseReader) return GetFieldObjectForSingleQuery(sqlCommandText);
            else
            {
                using (DataTable dataTable = GetDataTable(sqlCommandText))
                {
                    if (dataTable.Rows.Count != 0)
                        return dataTable.Rows[0][0];
                    else return null;
                }
            }
        }
        /// <summary>
        /// 返回一个从数据库里面查询出来的字段值
        /// </summary>
        /// <param name="sqlCommandText">SQL命令，仅需指定一个返回字段</param>
        /// <returns></returns>
        public object GetFieldObjectForSingleQuery(string sqlCommandText)
        {
            using (SqlDataReader sqlDataReader = GetDataReader(sqlCommandText))
            {
                if (sqlDataReader.Read()) return sqlDataReader[0];
                else return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateField"></param>
        /// <param name="updateValue"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public int UpdateFieldForSingleQuery(string tableName, string updateField, string updateValue, string fieldName, string fieldValue)
        {
            return ExecuteNonQuery("Update [" + tableName + "] set " + updateField + " = " + updateValue + " where " + fieldName + " = " + fieldValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCommandText"></param>
        /// <returns></returns>
        public bool ExistRecord(string sqlCommandText)
        {
            using (SqlDataReader sqlDataReader = GetDataReader(sqlCommandText))
            {
                if (sqlDataReader.Read()) return true;
                else return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCommandText"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string ExistRecord(string sqlCommandText, string fieldName)
        {
            using (SqlDataReader sqlDataReader = GetDataReader(sqlCommandText))
            {
                if (sqlDataReader.Read())
                    return sqlDataReader[fieldName].ToString();
                else return null;
            }
        }

        public SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            SqlTransaction = Connection.BeginTransaction(isolationLevel);
            return SqlTransaction;
        }
        public void CommitTransaction()
        {
            SqlTransaction.Commit();
            SqlTransaction.Dispose();
            SqlTransaction = null;
        }
        public void RollbackTransaction()
        {
            SqlTransaction.Rollback();
            SqlTransaction.Dispose();
            SqlTransaction = null;
        }


        public void Dispose()
        {
            SqlTransaction?.Dispose();
            SqlTransaction = null;
            Connection.Dispose();
            Connection = null;
        }
    }
}
