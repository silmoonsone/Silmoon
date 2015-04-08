using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Silmoon.Data;
using Silmoon.Data.Odbc;
using Silmoon.Data.SqlClient;

namespace Silmoon.Data.SqlUtility
{
    /// <summary>
    /// MySQL实用类
    /// </summary>
    public class MySQLHelper
    {
        SmMySqlClient _odbc;
        /// <summary>
        /// 使用指定的ODBC数据连接创建MYSQL实用工具
        /// </summary>
        /// <param name="odbc">指定一个已经可以使用的ODBC连接</param>
        public MySQLHelper(SmMySqlClient odbc)
        {
            _odbc = odbc;
        }
        /// <summary>
        /// 刷新数据库所有对象
        /// </summary>
        public void Refresh()
        {
            _odbc.ExecNonQuery("FLUSH PRIVILEGES");
        }

        /// <summary>
        /// 创建一个数据库
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        public int CreateDatabase(string database)
        {
            if (IsExistDatabase(database)) throw new MySQLException(null, "数据库已存在");
            return _odbc.ExecNonQuery("CREATE DATABASE " + database);
        }
        /// <summary>
        /// 删除一个数据库
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        public int DropDatabase(string database)
        {
            _odbc.ExecNonQuery("DELETE FROM mysql.db WHERE db='" + database + "'");

            if (database.ToLower() == "mysql") throw new MySQLException(null, "系统数据库无法删除");
            if (!IsExistDatabase(database)) throw new MySQLException(null, "指定了一个不存在的数据库");
            return _odbc.ExecNonQuery("DROP DATABASE " + database);
        }
        /// <summary>
        /// 添加一个用户并且制定所允许的数据库
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="database">指定的数据库</param>
        public int AddUserToDatabase(string username, string database)
        {
            if (!IsExistDatabase(database)) throw new MySQLException(null, "指定了一个不存在的数据库");
            if (!IsExistUser(username)) throw new MySQLException(null, "指定了一个不存在的数据库");


            int result = _odbc.ExecNonQuery("GRANT ALL PRIVILEGES ON `" + database + "`.* TO '" + username + "'@'%'");
            Refresh();
            return result;
        }
        /// <summary>
        /// 创建一个用户并且制定所允许的数据库
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <param name="database">指定的数据库</param>
        /// <returns></returns>
        public int CreateUserToDatabase(string username, string password, string database)
        {
            if (!IsExistDatabase(database)) throw new MySQLException(null, "指定了一个不存在的数据库");

            _odbc.ExecNonQuery("GRANT USAGE ON *.* TO '" + username + "'@'%' IDENTIFIED BY '" + password + "'");
            int result = _odbc.ExecNonQuery("GRANT ALL PRIVILEGES ON `" + database + "`.* TO '" + username + "'@'%'");
            Refresh();
            return result;
        }
        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public int CreateUser(string username, string password)
        {

            if (IsExistUser(username)) throw new MySQLException(null, "用户名已经存在。");

            int result = _odbc.ExecNonQuery("GRANT USAGE ON *.* TO '" + username + "'@'%' IDENTIFIED BY '" + password + "'");
            Refresh();
            return result;
        }
        /// <summary>
        /// 使用命令强行删除一个用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public int ForceRemoveUser(string username)
        {
            if (username.ToLower() == "root") throw new MySQLException(null, "禁止删除Root用户！");
            _odbc.ExecNonQuery("DELETE FROM mysql.db WHERE user = '" + username + "'");
            int result = _odbc.ExecNonQuery("DELETE FROM mysql.user WHERE user = '" + username + "'");
            Refresh();
            return result;
        }
        /// <summary>
        /// 使用命令强行删除一个用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="host">允许连接的主机</param>
        /// <returns></returns>
        public int ForceRemoveUser(string username, string host)
        {
            if (username.ToLower() == "root") throw new MySQLException(null, "禁止删除Root用户！");
            _odbc.ExecNonQuery("DELETE FROM mysql.db WHERE user = '" + username + "'");
            int result = _odbc.ExecNonQuery("DELETE FROM mysql.user WHERE user = '" + username + "' and host = '" + host + "'");
            Refresh();
            return result;
        }
        /// <summary>
        /// 移出一个用户对数据库的权限
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        public int RemoveUserGrant(string username, string database)
        {
            if (!IsExistDatabase(database)) throw new MySQLException(null, "指定了一个不存在的数据库");

            int result = _odbc.ExecNonQuery("DELETE FROM mysql.db WHERE user = '" + username + "' and db='" + database + "'");
            Refresh();
            return result;
        }
        /// <summary>
        /// 移出一个用户对数据库的权限
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="database">数据库</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public int RemoveUserGrant(string username, string database, string host)
        {
            if (!IsExistDatabase(database)) throw new MySQLException(null, "指定了一个不存在的数据库");

            int result = _odbc.ExecNonQuery("DELETE FROM mysql.db WHERE user = '" + username + "' and db='" + database + "' and host = '" + host + "'");
            Refresh();
            return result;
        }
        /// <summary>
        /// 检查数据库是否存在
        /// </summary>
        /// <param name="database">要检查的数据库</param>
        /// <returns></returns>
        public bool IsExistDatabase(string database)
        {
            DataTable dt = _odbc.GetDataTable("SHOW DATABASES");
            ArrayList _array = new ArrayList();
            foreach (DataRow row in dt.Rows) _array.Add(row[0]);
            string[] nameArr = (string[])_array.ToArray(typeof(string));
            dt.Clear();
            dt.Dispose();
            return SmString.FindFormStringArray(nameArr, database);
        }
        /// <summary>
        /// 检查一个用户名是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsExistUser(string username)
        {
            DataTable dt = _odbc.GetDataTable("select user from mysql.user");
            ArrayList _array = new ArrayList();
            foreach (DataRow row in dt.Rows) _array.Add(row[0]);
            string[] nameArr = (string[])_array.ToArray(typeof(string));
            dt.Clear();
            dt.Dispose();
            return SmString.FindFormStringArray(nameArr, username);
        }

        /// <summary>
        /// 设置一个旧版算法的密码给用户
        /// </summary>
        /// <param name="username">目标用户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public int SetOldPassword(string username, string password)
        {
            int i = _odbc.ExecNonQuery("UPDATE mysql.user SET PASSWORD = OLD_PASSWORD('" + password + "') WHERE USER = '" + username + "'");
            Refresh();
            return i;
        }
        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="username">目标用户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public int SetPassword(string username, string password)
        {
            int i = _odbc.ExecNonQuery("UPDATE mysql.user SET PASSWORD = PASSWORD('" + password + "') WHERE USER = '" + username + "'");
            Refresh();
            return i;
        }
        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="username">目标用户</param>
        /// <param name="string">主机</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public int SetPassword(string username, string host, string password)
        {
            int i = _odbc.ExecNonQuery("UPDATE mysql.user SET PASSWORD = PASSWORD('" + _odbc.InjectFieldReplace(password) + "') WHERE USER = '" + _odbc.InjectFieldReplace(username) + "' AND HOST = '" + _odbc.InjectFieldReplace(host) + "'");
            Refresh();
            return i;
        }
        /// <summary>
        /// 创建一个连接MySQL ODBC 3.51驱动所连接数据源的连接字符串
        /// </summary>
        /// <param name="server">主机名</param>
        /// <param name="userID">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        public static string MakeConnectionString(string server, string userID, string password, string database)
        {
            MySql.Data.MySqlClient.MySqlConnectionStringBuilder builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
            builder.Server = server;
            builder.UserID = userID;
            builder.Password = password;
            builder.Database = database;
            return builder.GetConnectionString(true);
        }
        /// <summary>
        /// 创建一个连接MySQL ODBC 3.51驱动所连接数据源的连接字符串
        /// </summary>
        /// <param name="odbcDriverName">ODBC数据源驱动名称</param>
        /// <param name="hostname">主机名</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        public static string MakeConnectionString(string odbcDriverName, string hostname, string username, string password, string database)
        {
            return "DRIVER={" + odbcDriverName + "};SERVER=" + hostname + ";DATABASE=" + database + ";UID=" + username + ";PASSWORD=" + password + ";";
        }
        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public string[] GetDatabases()
        {
            List<string> list = new List<string>();
            using (DataTable dt = _odbc.GetDataTable("show databases"))
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item[0].ToString());
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public string[] GetUsers()
        {
            List<string> list = new List<string>();
            using (DataTable dt = _odbc.GetDataTable("select * from mysql.user"))
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item["user"] + "@" + item["host"]);
                }
            }
            return list.ToArray();
        }

        public string[] GetUserDatabases(string username, string host)
        {
            List<string> list = new List<string>();
            using (DataTable dt = _odbc.GetDataTable("select db from mysql.db where user = '" + _odbc.InjectFieldReplace(username) + "' and host  = '" + _odbc.InjectFieldReplace(host) + "'"))
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item["db"].ToString());
                }
            }
            return list.ToArray();
        }

        public string[] GetDatabaseUsers(string database)
        {
            List<string> list = new List<string>();
            DataTable dt = _odbc.GetDataTable("select user,host from mysql.db where db = '" + database + "'");
            foreach (DataRow row in dt.Rows)
            {
                list.Add(row["user"] + "@" + row["host"]);
            }

            return list.ToArray();
        }
    }
    /// <summary>
    /// 表示MySQL异常
    /// </summary>
    public class MySQLException : Exception
    {
        string _message;
        /// <summary>
        /// 获取错误消息
        /// </summary>
        override public string Message
        {
            get { return _message; }
        }
        Exception _innerException;
        /// <summary>
        /// 获取内部异常
        /// </summary>
        new public Exception InnerException
        {
            get { return _innerException; }
        }
        /// <summary>
        /// 实例化错误
        /// </summary>
        /// <param name="innerException">引发当前异常的内部异常</param>
        /// <param name="message">消息</param>
        public MySQLException(Exception innerException, string message)
        {
            _innerException = innerException;
            _message = message;
        }
    }
}
