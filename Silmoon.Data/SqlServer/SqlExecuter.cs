using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.SqlServer
{
    public class SqlExecuter
    {
        SqlConnection SqlConnection = null;
        SqlUtil sqlUtil = null;
        DataBizAccess access = null;
        public SqlExecuter(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
            sqlUtil = new SqlUtil(sqlConnection);
            access = new DataBizAccess(sqlConnection);
        }
        public SqlExecuteResult AddObject<T>(string tableName, T obj)
        {
            var names = getPropertyNames<T>();

            string sql = $"INSERT INTO [{onlyWords(tableName)}] (";
            foreach (var item in names)
            {
                sql += "[" + item + "], ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += ") VALUES (";
            foreach (var item in names)
            {
                sql += "@" + item + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += ")";
            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, obj, names);
            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public T GetObject<T>(string tableName, object query = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            string sql = $"SELECT * FROM [{tableName}]";
            var props = getProperties(query, true);
            var names = getPropertyNames(props, true);
            if (names.Length != 0)
            {
                sql += " WHERE ";
                foreach (var item in names)
                {
                    sql += $"[{item}] = @{item} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, query, names);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return default;
                var obj = SqlHelper.MakeObject<T>(reader, new T());
                return (T)obj;
            }
        }
        public T GetObjectWithWhere<T>(string tableName, string query = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();

            string sql = $"SELECT * FROM [{tableName}]";
            var props = getProperties(query, true);
            var names = getPropertyNames(props, true);
            if (!string.IsNullOrEmpty(query))
            {
                sql += " WHERE " + query;
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, query, names);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return default;
                var obj = SqlHelper.MakeObject<T>(reader, new T());
                return (T)obj;
            }
        }
        public T[] GetObjects<T>(string tableName, object query = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();

            string sql = $"SELECT * FROM [{tableName}]";
            var props = getProperties(query, true);
            var names = getPropertyNames(props, true);
            if (names.Length != 0)
            {
                sql += " WHERE ";
                foreach (var item in names)
                {
                    sql += $"[{item}] = @{item} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, query, names);
            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = SqlHelper.MakeObjects<T>(reader);
                return obj;
            }
        }
        public T[] GetObjectsWithWhere<T>(string tableName, string query = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();

            string sql = $"SELECT * FROM [{tableName}]";
            var props = getProperties(query, true);
            var names = getPropertyNames(props, true);
            if (!string.IsNullOrEmpty(query))
            {
                sql += " WHERE " + query;
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, query, names);
            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = SqlHelper.MakeObjects<T>(reader);
                return obj;
            }
        }
        public SqlExecuteResult SetObject<T>(string tableName, T obj, object query, SqlQueryOptions options = null, params string[] fieldNames)
        {
            string sql = $"UPDATE [{tableName}] SET ";
            string[] setNames = fieldNames;
            PropertyInfo[] setProps = getProperties(obj);

            if (fieldNames == null || fieldNames.Length == 0)
            {
                setNames = getPropertyNames(setProps);
            }

            foreach (var item in setNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);


            var queryProps = getProperties(query, true);
            var queryNames = getPropertyNames(queryProps, true);
            if (queryNames.Length != 0)
            {
                sql += " WHERE ";
                foreach (var item in queryNames)
                {
                    sql += $"[{item}] = @{item} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }
            var cmd = access.GetCommand(sql);

            SqlHelper.AddSqlCommandParameters(cmd, obj, setNames);
            SqlHelper.AddSqlCommandParameters(cmd, query, queryNames);

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult SetObject<T>(string tableName, T obj, string query, SqlQueryOptions options = null, params string[] fieldNames)
        {
            string sql = $"UPDATE [{tableName}] SET ";
            string[] setNames = fieldNames;
            PropertyInfo[] setProps = getProperties(obj);

            if (fieldNames == null || fieldNames.Length == 0)
            {
                setNames = getPropertyNames(setProps);
            }

            foreach (var item in setNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);


            if (!string.IsNullOrEmpty(query))
            {
                sql += " WHERE " + query;
            }
            var cmd = access.GetCommand(sql);

            SqlHelper.AddSqlCommandParameters(cmd, obj, setNames);

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }


        public SqlExecuteResult<bool> CreateTable<T>(string tableName)
        {
            var isExistResult = TableIsExist(tableName);
            if (isExistResult.Data) return new SqlExecuteResult<bool>() { Data = false, ResponseRows = isExistResult.ResponseRows, ExecuteSqlString = isExistResult.ExecuteSqlString };
            var props = getProperties<T>();

            string sql = $"CREATE TABLE {onlyWords(tableName)}\r\n";
            sql += $"(\r\n";
            sql += $"id int NOT NULL IDENTITY (1, 1),\r\n";
            foreach (var item in props)
            {
                var type = item.PropertyType;
                if (type.IsEnum)
                {
                    sql += $"\t{item.Name} nvarchar(50),\r\n";
                }
                else if (type.Name == "DateTime")
                {
                    sql += $"\t{item.Name} datetime NULL,\r\n";
                }
                else if (type.Name == "String")
                {
                    sql += $"\t{item.Name} nvarchar(MAX) NULL,\r\n";
                }
                else if (type.Name == "Boolean")
                {
                    sql += $"\t{item.Name} bit,\r\n";
                }
                else if (type.Name == "Int32" || type.Name == "UInt32" || type.Name == "Int16" || type.Name == "UInt16" || type.Name == "Int64" || type.Name == "UInt64")
                {
                    sql += $"\t{item.Name} int,\r\n";
                }
                else if (type.Name == "Guid")
                {
                    sql += $"\t{item.Name} uniqueidentifier NULL,\r\n";
                }
                else if (type.Name == "ObjectId")
                {
                    sql += $"\t{item.Name} nvarchar(24),\r\n";
                }
                else
                {
                    throw new Exception("未知的类型处理（" + type.Name + "）");
                }
            }
            sql += ")  ON [PRIMARY]\r\n";
            sql += "TEXTIMAGE_ON [PRIMARY]\r\n";
            var i = sqlUtil.ExecNonQuery(sql);
            return new SqlExecuteResult<bool>() { Data = true, ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult<bool> TableIsExist(string tableName)
        {
            string sql = $"SELECT TOP 1 * FROM [SYSOBJECTS] WHERE id = OBJECT_ID(N'{tableName}') ";
            int i = sqlUtil.GetRecordCount(sql);
            return new SqlExecuteResult<bool>() { Data = i != 0, ExecuteSqlString = sql, ResponseRows = i };
        }




        private string[] getPropertyNames(object obj, bool includeId = false)
        {
            List<string> propertyNames = new List<string>();
            if (obj != null)
            {
                var propertyInfos = obj.GetType().GetProperties();
                foreach (var item in propertyInfos)
                {
                    if (item.Name.ToLower() == "id" && !includeId) continue;
                    propertyNames.Add(item.Name);
                }
            }
            return propertyNames.ToArray();
        }
        private string[] getPropertyNames(PropertyInfo[] props, bool includeId = false)
        {
            List<string> propertyNames = new List<string>();
            foreach (var item in props)
            {
                if (item.Name.ToLower() == "id" && !includeId) continue;
                propertyNames.Add(item.Name);
            }
            return propertyNames.ToArray();
        }
        private string[] getPropertyNames<T>(bool includeId = false)
        {
            List<string> propertyNames = new List<string>();
            var propertyInfos = typeof(T).GetProperties();
            foreach (var item in propertyInfos)
            {
                if (item.Name.ToLower() == "id" && !includeId) continue;
                propertyNames.Add(item.Name);
            }
            return propertyNames.ToArray();
        }
        private PropertyInfo[] getProperties(object obj, bool includeId = false)
        {
            List<PropertyInfo> propertyNames = new List<PropertyInfo>();
            if (obj != null)
            {
                var propertyInfos = obj.GetType().GetProperties();
                foreach (var item in propertyInfos)
                {
                    if (item.Name.ToLower() == "id" && !includeId) continue;
                    propertyNames.Add(item);
                }
            }
            return propertyNames.ToArray();
        }
        private PropertyInfo[] getProperties<T>(bool includeId = false)
        {
            List<PropertyInfo> propertyNames = new List<PropertyInfo>();
            var propertyInfos = typeof(T).GetProperties();
            foreach (var item in propertyInfos)
            {
                if (item.Name.ToLower() == "id" && !includeId) continue;
                propertyNames.Add(item);
            }
            return propertyNames.ToArray();
        }
        private string onlyWords(string s)
        {
            return s;
        }
    }
}
