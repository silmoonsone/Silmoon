using Silmoon.Data.SqlServer.SqlInternal;
using System;
using System.Collections.Generic;
using System.Data;
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
        SqlAccess access = null;


        public SqlExecuter(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
            sqlUtil = new SqlUtil(sqlConnection);
            access = new SqlAccess(sqlConnection);
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
        public void AddObjects<T>(string tableName, T[] obj)
        {
            foreach (var item in obj)
            {
                AddObject(tableName, item);
            }
        }
        public SqlExecuteResult<T> GetObject<T>(string tableName, object queryObj, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} * FROM [{tableName}]";
            else sql += $" * FROM [{tableName}]";

            var props = getProperties(queryObj, true);
            var names = getPropertyNames(props, true);

            if (names.Length != 0)
            {
                sql += " WHERE ";
                foreach (var item in names)
                {
                    if (props[item].GetValue(queryObj) == null)
                        sql += $"[{item}] IS NULL AND ";
                    else
                        sql += $"[{item}] = @{item} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }

            if (options.Sorts != null && options.Sorts.Count() != 0)
            {
                sql += " ORDER BY";
                foreach (var item in options.Sorts)
                {
                    sql += $" [{item.Name}]";
                    if (item.Method == QueryModel.SortMethod.Asc)
                        sql += " ASC";
                    else sql += " DESC,";
                }
                sql = sql.Substring(0, sql.Length - 1);
            }

            if (options.Offset.HasValue)
            {
                sql += $" OFFSET {options.Offset} ROWS";
                if (options.Count.HasValue)
                    sql += $" FETCH NEXT {options.Count} ROWS ONLY";
            }


            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, queryObj, names);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                var obj = SqlHelper.MakeObject(reader, new T());
                return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResult<T> GetObjectWithWhere<T>(string tableName, string queryStr, object queryObj = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} * FROM [{tableName}]";
            else sql += $" * FROM [{tableName}]";


            if (!string.IsNullOrEmpty(queryStr))
            {
                sql += " WHERE " + queryStr;
            }

            if (options.Sorts != null && options.Sorts.Count() != 0)
            {
                sql += " ORDER BY";
                foreach (var item in options.Sorts)
                {
                    sql += $" [{item.Name}]";
                    if (item.Method == QueryModel.SortMethod.Asc)
                        sql += " ASC";
                    else sql += " DESC,";
                }
                sql = sql.Substring(0, sql.Length - 1);
            }

            if (options.Offset.HasValue)
            {
                sql += $" OFFSET {options.Offset} ROWS";
                if (options.Count.HasValue)
                    sql += $" FETCH NEXT {options.Count} ROWS ONLY";
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, queryObj, GetPropertyNames(queryObj, true));

            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                var obj = SqlHelper.MakeObject(reader, new T());
                return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResult<T[]> GetObjects<T>(string tableName, object queryObj = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} * FROM [{tableName}]";
            else sql += $" * FROM [{tableName}]";

            var props = getProperties(queryObj, true);
            var names = getPropertyNames(props, true);

            if (names.Length != 0)
            {
                sql += " WHERE ";
                foreach (var item in names)
                {
                    if (props[item].GetValue(queryObj) == null)
                        sql += $"[{item}] IS NULL AND ";
                    else
                        sql += $"[{item}] = @{item} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }

            if (options.Sorts != null && options.Sorts.Count() != 0)
            {
                sql += " ORDER BY";
                foreach (var item in options.Sorts)
                {
                    sql += $" [{item.Name}]";
                    if (item.Method == QueryModel.SortMethod.Asc)
                        sql += " ASC";
                    else sql += " DESC,";
                }
                sql = sql.Substring(0, sql.Length - 1);
            }

            if (options.Offset.HasValue)
            {
                sql += $" OFFSET {options.Offset} ROWS";
                if (options.Count.HasValue)
                    sql += $" FETCH NEXT {options.Count} ROWS ONLY";
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, queryObj, names);
            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = SqlHelper.MakeObjects<T>(reader);
                return new SqlExecuteResult<T[]>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResult<T[]> GetObjectsWithWhere<T>(string tableName, string queryStr = null, object queryObj = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} * FROM [{tableName}]";
            else sql += $" * FROM [{tableName}]";

            if (!string.IsNullOrEmpty(queryStr))
            {
                sql += " WHERE " + queryStr;
            }

            if (options.Sorts != null && options.Sorts.Count() != 0)
            {
                sql += " ORDER BY";
                foreach (var item in options.Sorts)
                {
                    sql += $" [{item.Name}]";
                    if (item.Method == QueryModel.SortMethod.Asc)
                        sql += " ASC";
                    else sql += " DESC,";
                }
                sql = sql.Substring(0, sql.Length - 1);
            }

            if (options.Offset.HasValue)
            {
                sql += $" OFFSET {options.Offset} ROWS";
                if (options.Count.HasValue)
                    sql += $" FETCH NEXT {options.Count} ROWS ONLY";
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, queryObj, GetPropertyNames(queryObj, true));

            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = SqlHelper.MakeObjects<T>(reader);
                return new SqlExecuteResult<T[]>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResult SetObject<T>(string tableName, T obj, object queryObj, params string[] fieldNames)
        {
            string sql = $"UPDATE [{tableName}] SET ";
            string[] setNames = fieldNames;
            Dictionary<string, PropertyInfo> setProps = getProperties(obj, false);

            if (fieldNames == null || fieldNames.Length == 0)
            {
                setNames = getPropertyNames(setProps, false);
            }

            foreach (var item in setNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);


            var props = getProperties(queryObj, true);
            var names = getPropertyNames(props, true);
            if (names.Length != 0)
            {
                sql += " WHERE ";
                foreach (var item in names)
                {
                    if (props[item].GetValue(queryObj) == null)
                        sql += $"[{item}] IS NULL AND ";
                    else
                        sql += $"[{item}] = @{item} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }
            var cmd = access.GetCommand(sql);

            SqlHelper.AddSqlCommandParameters(cmd, obj, setNames);
            SqlHelper.AddSqlCommandParameters(cmd, queryObj, names);

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult SetObject<T>(string tableName, T obj, string queryStr, object queryObj = null, params string[] fieldNames)
        {
            string sql = $"UPDATE [{tableName}] SET ";
            string[] setNames = fieldNames;
            Dictionary<string, PropertyInfo> setProps = getProperties(obj, false);

            if (fieldNames == null || fieldNames.Length == 0)
            {
                setNames = getPropertyNames(setProps, false);
            }

            foreach (var item in setNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);


            if (!string.IsNullOrEmpty(queryStr))
            {
                sql += " WHERE " + queryStr;
            }
            var cmd = access.GetCommand(sql);

            SqlHelper.AddSqlCommandParameters(cmd, obj, setNames);
            SqlHelper.AddSqlCommandParameters(cmd, queryObj, GetPropertyNames(queryObj, true));

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult DeleteObject<T>(string tableName, object queryObj)
        {
            string sql = $"DELETE [{tableName}]";

            var props = getProperties(queryObj, true);
            var names = getPropertyNames(props, true);

            if (names.Length != 0)
            {
                sql += " WHERE ";
                foreach (var item in names)
                {
                    if (props[item].GetValue(queryObj) == null)
                        sql += $"[{item}] IS NULL AND ";
                    else
                        sql += $"[{item}] = @{item} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, queryObj, names);
            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult DeleteObject<T>(string tableName, string queryStr, object queryObj = null)
        {
            string sql = $"DELETE [{tableName}]";

            var props = getProperties(queryStr, true);
            var names = getPropertyNames(props, true);

            if (!string.IsNullOrEmpty(queryStr))
            {
                sql += " WHERE " + queryStr;
            }

            var cmd = access.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, queryStr, names);
            SqlHelper.AddSqlCommandParameters(cmd, queryObj, GetPropertyNames(queryObj, true));

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }


        public SqlExecuteResult<bool> CreateTable<T>(string tableName)
        {
            var isExistResult = TableIsExist(tableName);
            if (isExistResult.Data) return new SqlExecuteResult<bool>() { Data = false, ResponseRows = isExistResult.ResponseRows, ExecuteSqlString = isExistResult.ExecuteSqlString };
            var props = getProperties<T>(false);

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
                else if (type.Name == "Byte[]")
                {
                    sql += $"\t{item.Name} VARBINARY(5120),\r\n";
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


        public SqlAccessTransaction BeginTransaction(bool setCurrentTransaction = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return access.BeginTransaction(setCurrentTransaction, isolationLevel);
        }
        public void CommitTransaction(SqlAccessTransaction transaction)
        {
            access.CommitTransaction(transaction);
        }
        public void RollbackTransaction(SqlAccessTransaction transaction)
        {
            access.RollbackTransaction(transaction);
        }


        public string[] GetPropertyNames(object obj, bool includeId = false)
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

        private string[] getPropertyNames(Dictionary<string, PropertyInfo> props, bool includeId = false)
        {
            List<string> propertyNames = new List<string>();
            foreach (var item in props)
            {
                if (item.Key.ToLower() == "id" && !includeId) continue;
                propertyNames.Add(item.Key);
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
        private Dictionary<string, PropertyInfo> getProperties(object obj, bool includeId = false)
        {
            Dictionary<string, PropertyInfo> propertyNames = new Dictionary<string, PropertyInfo>();
            if (obj != null)
            {
                var propertyInfos = obj.GetType().GetProperties();
                foreach (var item in propertyInfos)
                {
                    if (item.Name.ToLower() == "id" && !includeId) continue;
                    propertyNames.Add(item.Name, item);
                }
            }
            return propertyNames;
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
