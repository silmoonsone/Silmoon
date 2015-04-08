using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silmoon.Data.SqlClient
{
    public class SmMySqlClientSource : IDisposable
    {
        MySqlConnection conn = new MySqlConnection();
        SmMySqlClient source = new SmMySqlClient();

        public SmMySqlClient DataSource
        {
            get { return source; }
            set { source = value; }
        }
        public MySqlConnection Connection
        {
            get { return conn; }
            set { conn = value; }
        }

        public SmMySqlClientSource()
        {
            source.Connection = conn;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Close();
            conn = null;
        }

        #endregion

        /// <summary>
        /// 实例化数据类型和数据源
        /// </summary>
        /// <param name="open">是否在实例的时候打开数据库</param>
        /// <param name="conStr">指定连接数据库的数据库连接字符串</param>
        public void InitData(bool open, string conStr)
        {
            conn.ConnectionString = conStr;
            if (open) Open();
        }
        public bool Open()
        {
            if (conn.State == System.Data.ConnectionState.Open) return false;
            conn.Open();
            return true;
        }
        public bool Close()
        {
            if (conn.State == System.Data.ConnectionState.Closed) return false;
            conn.Close();
            return true;
        }
    }
}