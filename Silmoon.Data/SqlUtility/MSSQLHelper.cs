using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Data.SqlClient;
using System.Data;
using System.Collections;

namespace Silmoon.Data.SqlUtility
{
    /// <summary>
    /// MySQL实用类
    /// </summary>
    public class MSSQLHelper
    {
        SmMSSQLClient _mssql;
        /// <summary>
        /// 使用指定的ODBC数据连接创建MYSQL实用工具
        /// </summary>
        /// <param name="odbc">指定一个已经可以使用的ODBC连接</param>
        public MSSQLHelper(SmMSSQLClient odbc)
        {
            _mssql = odbc;
        }
        /// <summary>
        /// 刷新数据库所有对象
        /// </summary>
        public void Refresh()
        {
            _mssql.ExecNonQuery("RECONFIGURE WITH OVERRIDE");
        }

        /// <summary>
        /// 创建一个数据库
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        public int CreateDatabase(string database)
        {
            if (IsExistDatabase(database)) throw new MSSQLException(null, "数据库已存在");
            return _mssql.ExecNonQuery("CREATE DATABASE " + database);
        }
        /// <summary>
        /// 删除一个数据库
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        public int DropDatabase(string database)
        {
            if (database.ToLower() == "master") throw new MSSQLException(null, "系统数据库无法删除");
            if (!IsExistDatabase(database)) throw new MSSQLException(null, "指定了一个不存在的数据库");
            return _mssql.ExecNonQuery("DROP DATABASE " + database);
        }
        /// <summary>
        /// 添加一个用户并且制定所允许的数据库
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="database">指定的数据库</param>
        public int AddUserToDatabase(string username, string database)
        {
            if (!IsExistDatabase(database)) throw new MSSQLException(null, "指定了一个不存在的数据库");
            if (!IsExistUser(username)) throw new MSSQLException(null, "指定了一个不存在的数据库");

            int result = _mssql.ExecNonQuery("USE [" + database + "];CREATE USER [" + username + "] FOR LOGIN [" + username + "]");
            _mssql.ExecNonQuery("USE [" + database + "];EXEC sp_addrolemember N'db_owner', N'" + username + "'");
            _mssql.ExecNonQuery("USE [Master]");
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
            if (username == "") throw new MSSQLException(null, "不允许空的用户名!");
            if (IsExistUser(username)) throw new MSSQLException(null, "用户名已经存在!");
            int result = _mssql.ExecNonQuery("CREATE LOGIN [" + username + "] WITH PASSWORD=N'" + password + "', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF");
            Refresh();
            return result;
        }
        /// <summary>
        /// 使用命令强行删除一个用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public int RemoveUser(string username)
        {
            if (username.ToLower() == "sa") throw new MSSQLException(null, "禁止删除sa用户！");
            int result = _mssql.ExecNonQuery("DROP LOGIN [" + username + "]");
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
            if (!IsExistDatabase(database)) throw new MSSQLException(null, "指定了一个不存在的数据库");
            string brforeDatabase = _mssql.GetFieldObjectForSingleQuery("select db_name()").ToString();
            int result = _mssql.ExecNonQuery("USE " + database + ";drop user [" + username + "]");
            _mssql.ExecNonQuery("USE " + brforeDatabase);
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
            DataTable dt = _mssql.GetDataTable("Select Name From Master..SysDatabases");
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
            DataTable dt = _mssql.GetDataTable("select Name from Master..syslogins where isntname=0");
            ArrayList _array = new ArrayList();
            foreach (DataRow row in dt.Rows) _array.Add(row[0]);
            string[] nameArr = (string[])_array.ToArray(typeof(string));
            dt.Clear();
            dt.Dispose();
            return SmString.FindFormStringArray(nameArr, username);
        }
        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="username">目标用户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public int SetPassword(string username, string password)
        {
            int i = _mssql.ExecNonQuery("USE [master];ALTER LOGIN ["+username+"] WITH PASSWORD=N'"+password+"'");
            return i;
        }
        /// <summary>
        /// 创建一个连接MySQL ODBC 3.51驱动所连接数据源的连接字符串
        /// </summary>
        /// <param name="hostname">主机名</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        public static string MakeConnectionString(string hostname, string username, string password, string database)
        {
            return "DRIVER={MySQL ODBC 3.51 Driver};SERVER=" + hostname + ";DATABASE=" + database + ";UID=" + username + ";PASSWORD=" + password + ";";
        }
        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public string[] GetDatabases()
        {
            List<string> list = new List<string>();
            using (DataTable dt = _mssql.GetDataTable("Select Name FROM Master.dbo.SysDatabases ORDER BY Name"))
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
            using (DataTable dt = _mssql.GetDataTable("select Name from syslogins where isntname=0"))
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item[0].ToString());
                }
            }
            return list.ToArray();
        }

        public string[] GetUserDatabases(string username)
        {
            List<string> list = new List<string>();

            using (DataTable namedt = _mssql.GetDataTable("Select Name FROM Master.dbo.SysDatabases ORDER BY Name"))
            {
                foreach (DataRow row in namedt.Rows)
                {
                    using (DataTable dt = _mssql.GetDataTable("use " + row["Name"].ToString() + ";select Name from sysusers where islogin=1;use master"))
                    {
                        foreach (DataRow namesrow in dt.Rows)
                            if (namesrow["Name"].ToString() == username)
                                list.Add(row["Name"].ToString());
                    }
                }
            }
            return list.ToArray();
        }

        public string[] GetDatabaseUsers(string database)
        {
            List<string> list = new List<string>();

            using (DataTable dt = _mssql.GetDataTable("use " + database + ";select Name from sysusers where islogin=1;use master"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(row["Name"].ToString());
                }
            }
            return list.ToArray();
        }
    }
    /// <summary>
    /// 表示MySQL异常
    /// </summary>
    public class MSSQLException : Exception
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
        /// <param name="_innerException">引发当前异常的内部异常</param>
        /// <param name="message">消息</param>
        public MSSQLException(Exception innerException, string message)
        {
            _innerException = innerException;
            _message = message;
        }
    }
}
