using Silmoon.Data.QueryModel;
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
        SqlUtil sqlUtil = null;
        SqlAccess sqlAccess = null;


        public SqlExecuter(SqlConnection sqlConnection)
        {
            sqlUtil = new SqlUtil(sqlConnection);
            sqlAccess = new SqlAccess(sqlConnection);
        }
        public SqlExecuteResult AddObject<T>(string tableName, T obj)
        {
            var names = GetPropertyNames(typeof(T));

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
            var cmd = sqlAccess.GetCommand(sql);
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

        public SqlExecuteResult<T> GetObject<T>(string tableName, object whereObject, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(typeof(T), tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(typeof(T), tableName, ref options)} FROM [{tableName}]";

            if (options.OnOption != null) makeOnString(ref sql, ref tableName, options.OnOption);

            var props = getProperties(whereObject, true);
            var names = getPropertyNames(props, true);

            makeWhereString(ref sql, ref tableName, whereObject, ref props, ref names);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);


            var cmd = sqlAccess.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, whereObject, names);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                var obj = SqlHelper.MakeObject(reader, new T(), options.ExcludedField);
                return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResult<T> GetObjectWithWhere<T>(string tableName, string whereString, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(typeof(T), tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(typeof(T), tableName, ref options)} FROM [{tableName}]";

            if (options.OnOption != null) makeOnString(ref sql, ref tableName, options.OnOption);

            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            var cmd = sqlAccess.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, whereObject, GetPropertyNames(whereObject.GetType(), true));

            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                var obj = SqlHelper.MakeObject(reader, new T(), options.ExcludedField);
                return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResults<T[]> GetObjects<T>(string tableName, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(typeof(T), tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(typeof(T), tableName, ref options)} FROM [{tableName}]";

            if (options.OnOption != null) makeOnString(ref sql, ref tableName, options.OnOption);

            var props = getProperties(whereObject, true);
            var names = getPropertyNames(props, true);

            makeWhereString(ref sql, ref tableName, whereObject, ref props, ref names);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            var cmd = sqlAccess.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, whereObject, names);


            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = SqlHelper.MakeObjects<T>(reader, options.ExcludedField);
                return new SqlExecuteResults<T[]>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResults<T[]> GetObjectsWithWhere<T>(string tableName, string whereString = null, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = new SqlQueryOptions();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(typeof(T), tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(typeof(T), tableName, ref options)} FROM [{tableName}]";

            if (options.OnOption != null) makeOnString(ref sql, ref tableName, options.OnOption);

            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            var cmd = sqlAccess.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, whereObject, GetPropertyNames(whereObject.GetType(), true));

            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = SqlHelper.MakeObjects<T>(reader, options.ExcludedField);
                return new SqlExecuteResults<T[]>(reader.RecordsAffected, sql, obj);
            }
        }

        public SqlExecuteResult SetObject<T>(string tableName, T obj, object whereObject, params string[] updateObjectFieldNames)
        {
            return SetObject(tableName, (object)obj, whereObject, updateObjectFieldNames);
        }
        public SqlExecuteResult SetObject<T>(string tableName, T obj, string whereString, object whereObject = null, params string[] updateObjectFieldNames)
        {
            return SetObject(tableName, (object)obj, whereString, whereObject, updateObjectFieldNames);
        }
        public SqlExecuteResult SetObject(string tableName, object obj, object whereObject, params string[] updateObjectFieldNames)
        {
            string sql = $"UPDATE [{tableName}] SET ";
            string[] setNames = updateObjectFieldNames;
            Dictionary<string, PropertyInfo> setProps = getProperties(obj, false);

            if (updateObjectFieldNames == null || updateObjectFieldNames.Length == 0)
            {
                setNames = getPropertyNames(setProps, false);
            }

            foreach (var item in setNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);


            var props = getProperties(whereObject, true);
            var names = getPropertyNames(props, true);

            makeWhereString(ref sql, ref tableName, whereObject, ref props, ref names);

            var cmd = sqlAccess.GetCommand(sql);

            SqlHelper.AddSqlCommandParameters(cmd, obj, setNames);
            SqlHelper.AddSqlCommandParameters(cmd, whereObject, names);

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult SetObject(string tableName, object obj, string whereString, object whereObject = null, params string[] updateObjectFieldNames)
        {
            string sql = $"UPDATE [{tableName}] SET ";
            string[] setNames = updateObjectFieldNames;
            Dictionary<string, PropertyInfo> setProps = getProperties(obj, false);

            if (updateObjectFieldNames == null || updateObjectFieldNames.Length == 0)
            {
                setNames = getPropertyNames(setProps, false);
            }

            foreach (var item in setNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);

            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            var cmd = sqlAccess.GetCommand(sql);

            SqlHelper.AddSqlCommandParameters(cmd, obj, setNames);
            SqlHelper.AddSqlCommandParameters(cmd, whereObject, GetPropertyNames(whereObject.GetType(), true));

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult DeleteObject(string tableName, object whereObject)
        {
            string sql = $"DELETE [{tableName}]";

            var props = getProperties(whereObject, true);
            var names = getPropertyNames(props, true);

            makeWhereString(ref sql, ref tableName, whereObject, ref props, ref names);

            var cmd = sqlAccess.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, whereObject, names);
            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult DeleteObject(string tableName, string whereString, object whereObject = null)
        {
            string sql = $"DELETE [{tableName}]";

            var props = getProperties(whereString, true);
            var names = getPropertyNames(props, true);

            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            var cmd = sqlAccess.GetCommand(sql);
            SqlHelper.AddSqlCommandParameters(cmd, whereString, names);
            SqlHelper.AddSqlCommandParameters(cmd, whereObject, GetPropertyNames(whereObject.GetType(), true));

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }


        public SqlExecuteResult<bool> CreateTable<T>(string tableName)
        {
            var isExistResult = TableIsExist(tableName);
            if (isExistResult.Result) return new SqlExecuteResult<bool>() { Result = false, ResponseRows = isExistResult.ResponseRows, ExecuteSqlString = isExistResult.ExecuteSqlString };
            var props = getProperties(typeof(T), false);

            string sql = $"CREATE TABLE [{onlyWords(tableName)}]\r\n";
            sql += $"(\r\n";
            sql += $"[id] int NOT NULL IDENTITY (1, 1),\r\n";
            foreach (var item in props)
            {
                var type = item.PropertyType;
                if (type.IsEnum)
                {
                    sql += $"\t[{item.Name}] int,\r\n";
                }
                else if (type.Name == "DateTime")
                {
                    sql += $"\t{item.Name} datetime NULL,\r\n";
                }
                else if (type.Name == "String")
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";
                }
                else if (type.Name == "Boolean")
                {
                    sql += $"\t{item.Name} bit,\r\n";
                }
                else if (type.Name == "Int16" || type.Name == "UInt16")
                {
                    sql += $"\t[{item.Name}] smallint,\r\n";
                }
                else if (type.Name == "Int32" || type.Name == "UInt32")
                {
                    sql += $"\t[{item.Name}] int,\r\n";
                }
                else if (type.Name == "Int64" || type.Name == "UInt64")
                {
                    sql += $"\t[{item.Name}] bigint,\r\n";
                }
                else if (type.Name == "Decimal")
                {
                    sql += $"\t[{item.Name}] decimal(18, 4) NULL,\r\n";
                }
                else if (type.Name == "Guid")
                {
                    sql += $"\t[{item.Name}] uniqueidentifier NULL,\r\n";
                }
                else if (type.Name == "ObjectId")
                {
                    sql += $"\t[{item.Name}] nvarchar(24),\r\n";
                }
                else if (type.Name == "Byte[]")
                {
                    sql += $"\t[{item.Name}] VARBINARY(5120),\r\n";
                }
                else
                {
                    throw new Exception("未知的类型处理（" + type.Name + "）");
                }
            }
            sql += ")  ON [PRIMARY]\r\n";
            //sql += "TEXTIMAGE_ON [PRIMARY]\r\n";
            var i = sqlUtil.ExecNonQuery(sql);
            return new SqlExecuteResult<bool>() { Result = true, ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult<bool> TableIsExist(string tableName)
        {
            string sql = $"SELECT TOP 1 * FROM [SYSOBJECTS] WHERE id = OBJECT_ID(N'{tableName}') ";
            int i = sqlUtil.GetRecordCount(sql);
            return new SqlExecuteResult<bool>() { Result = i != 0, ExecuteSqlString = sql, ResponseRows = i };
        }


        public SqlAccessTransaction BeginTransaction(bool setCurrentTransaction = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return sqlAccess.BeginTransaction(setCurrentTransaction, isolationLevel);
        }
        public void CommitTransaction(SqlAccessTransaction transaction)
        {
            sqlAccess.CommitTransaction(transaction);
        }
        public void RollbackTransaction(SqlAccessTransaction transaction)
        {
            sqlAccess.RollbackTransaction(transaction);
        }



        private string makeSelectFieldString(Type type, string tableName, ref SqlQueryOptions options)
        {
            if (options.FieldOption == SelectFieldOption.All)
            {
                return "*";
            }
            else if (options.FieldOption == SelectFieldOption.SpecifiedObject)
            {
                string[] names = GetPropertyNames(type);
                string s = "";
                foreach (var item in names)
                {
                    if (options.ExcludedField?.Contains(item) ?? false) continue;
                    s += "[" + tableName + "].[" + item + "], ";
                }
                return s.Substring(0, s.Length - 2);
            }
            else return "*";
        }
        private void makeOnString(ref string sql, ref string tableName, OnOption onOption)
        {
            sql += $" {onOption.On.ToString().ToUpper()} JOIN [{onOption.TableName}] ON (";
            foreach (var item in onOption.FieldNames)
            {
                sql += $"[{tableName}].[{item}] = [{onOption.TableName}].[{item}] AND ";
            }
            sql = sql.Substring(0, sql.Length - 5);
            sql += ")";
        }
        private void makeWhereString(ref string sql, ref string tableName, object queryObj, ref Dictionary<string, PropertyInfo> props, ref string[] names, bool addWhereStr = true)
        {
            if (names.Length != 0)
            {
                if (addWhereStr) sql += " WHERE ";
                foreach (var item in names)
                {
                    if (props[item].GetValue(queryObj) == null)
                        sql += $"[{tableName}].[{item}] IS NULL AND ";
                    else
                        sql += $"[{tableName}].[{item}] = @{item} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }
        }
        private void makeOrderBy(ref string sql, ref string tableName, ref SqlQueryOptions options)
        {
            if (options.Sorts != null && options.Sorts.Count() != 0)
            {
                sql += " ORDER BY";
                foreach (var item in options.Sorts)
                {
                    sql += $" [{tableName}].[{item.Name}]";
                    if (item.Method == QueryModel.SortMethod.Asc)
                        sql += " ASC,";
                    else sql += " DESC,";
                }
                sql = sql.Substring(0, sql.Length - 1);
            }
        }
        private void makeOffset(ref string sql, ref SqlQueryOptions options)
        {
            if (options.Offset.HasValue)
            {
                sql += $" OFFSET {options.Offset} ROWS";
                if (options.Count.HasValue)
                    sql += $" FETCH NEXT {options.Count} ROWS ONLY";
            }
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
        public string[] GetPropertyNames(Type type, bool includeId = false)
        {
            List<string> propertyNames = new List<string>();
            var propertyInfos = type.GetProperties();
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
        private PropertyInfo[] getProperties(Type type, bool includeId = false)
        {
            List<PropertyInfo> propertyNames = new List<PropertyInfo>();
            var propertyInfos = type.GetProperties();
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
