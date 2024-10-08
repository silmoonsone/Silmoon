using Microsoft.Data.SqlClient;
using Silmoon.Data.QueryModel;
using Silmoon.Data.SqlServer.SqlInternal;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Silmoon.Runtime;
using Silmoon.Data.SqlServer.Extensions;

namespace Silmoon.Data.SqlServer
{
    public class SqlExecuter : IDisposable
    {
        SqlUtil SqlUtil { get; set; }
        SqlAccess SqlAccess { get; set; }
        public SqlConnection SqlConnection { get; }

        public SqlExecuter(string connectionString)
        {
            SqlConnection = new SqlConnection(connectionString);
            SqlConnection.Open();
            SqlUtil = new SqlUtil(SqlConnection);
            SqlAccess = new SqlAccess(SqlConnection);
        }
        public SqlExecuteResult AddObject<T>(string tableName, T obj)
        {
            var fieldInfos = getFieldInfos(obj, false);

            string sql = $"INSERT INTO [{onlyWords(tableName)}] (";
            foreach (var item in fieldInfos)
            {
                sql += "[" + item.Value.Name + "], ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += ") VALUES (";
            foreach (var item in fieldInfos)
            {
                sql += "@" + item.Value.Name + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += ")";
            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);
            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public void AddObjects<T>(string tableName, T[] obj) => obj.ForEachEx(x => AddObject(tableName, x));

        public SqlExecuteResult<T> GetObject<T>(string tableName, object whereObject, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = getFieldInfos(whereObject, true);

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            makeWhereString(ref sql, ref tableName, ref fieldInfos);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);


            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                var obj = reader.DeserializeObject(new T(), options.ExcludedField);
                return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResult<T> GetObject<T>(string tableName, ExpandoObject whereObject, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = getFieldInfos(whereObject, true);

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            makeWhereString(ref sql, ref tableName, ref fieldInfos);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);


            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                var obj = reader.DeserializeObject(new T(), options.ExcludedField);
                return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResult<T> GetObjectWithWhere<T>(string tableName, string whereString, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = getFieldInfos(whereObject, true);

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);

            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);

            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                var obj = reader.DeserializeObject(new T(), options.ExcludedField);
                return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
            }
        }

        public SqlExecuteResults<T[]> GetObjects<T>(string tableName, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = getFieldInfos(whereObject, true);

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            makeWhereString(ref sql, ref tableName, ref fieldInfos);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);


            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = reader.DeserializeObjects<T>(options.ExcludedField);
                return new SqlExecuteResults<T[]>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResults<T[]> GetObjects<T>(string tableName, ExpandoObject whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = getFieldInfos(whereObject, true);

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            makeWhereString(ref sql, ref tableName, ref fieldInfos);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);


            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = reader.DeserializeObjects<T>(options.ExcludedField);
                return new SqlExecuteResults<T[]>(reader.RecordsAffected, sql, obj);
            }
        }
        public SqlExecuteResults<T[]> GetObjectsWithWhere<T>(string tableName, string whereString = null, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = getFieldInfos(whereObject, true);

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);

            using (var reader = cmd.ExecuteReader())
            {
                //if (!reader.Read()) return default;
                var obj = reader.DeserializeObjects<T>(options.ExcludedField);
                return new SqlExecuteResults<T[]>(reader.RecordsAffected, sql, obj);
            }
        }


        public SqlExecuteResult SetObject<T>(string tableName, T obj, object whereObject, params string[] updateObjectFieldNames) => SetObjectInternal(tableName, obj, null, whereObject, updateObjectFieldNames);
        public SqlExecuteResult SetObject<T>(string tableName, T obj, ExpandoObject whereObject, params string[] updateObjectFieldNames) => SetObjectInternal(tableName, obj, null, whereObject, updateObjectFieldNames);
        public SqlExecuteResult SetObject<T>(string tableName, T obj, string whereString, object whereObject = null, params string[] updateObjectFieldNames) => SetObjectInternal(tableName, obj, whereString, whereObject, updateObjectFieldNames);

        public SqlExecuteResult SetObject<T>(string tableName, T obj, object whereObject, params Expression<Func<T, object>>[] updateExpressions) => SetObjectInternal(tableName, obj, null, whereObject, updateExpressions);
        public SqlExecuteResult SetObject<T>(string tableName, T obj, ExpandoObject whereObject, params Expression<Func<T, object>>[] updateExpressions) => SetObjectInternal(tableName, obj, null, whereObject, updateExpressions);
        public SqlExecuteResult SetObject<T>(string tableName, T obj, string whereString, object whereObject = null, params Expression<Func<T, object>>[] updateExpressions) => SetObjectInternal(tableName, obj, whereString, whereObject, updateExpressions);


        SqlExecuteResult SetObjectInternal(string tableName, object obj, string whereString, object whereObject, params string[] updateObjectFieldNames)
        {
            string sql = $"UPDATE [{tableName}] SET ";
            string[] setNames = updateObjectFieldNames;
            Dictionary<string, SimplePropertyInfo> setFieldInfos = getFieldInfos(obj, false);

            if (setNames.IsNullOrEmpty()) setNames = getPropertyNames(setFieldInfos, false);

            foreach (var item in setNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);


            Dictionary<string, SimplePropertyInfo> fieldInfos;
            if (whereObject is ExpandoObject expandoObject) fieldInfos = getFieldInfos(expandoObject, true);
            else fieldInfos = getFieldInfos(whereObject, true);

            if (whereString.IsNullOrEmpty())
                makeWhereString(ref sql, ref tableName, ref fieldInfos);
            else sql += " WHERE " + whereString;

            var cmd = SqlAccess.GetCommand(sql);

            cmd.AddParameters(getFieldInfos(obj, false), setNames);
            cmd.AddParameters(fieldInfos);

            int returnLine = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = returnLine };
        }
        SqlExecuteResult SetObjectInternal<T>(string tableName, T obj, string whereString, object whereObject, params Expression<Func<T, object>>[] updateExpressions)
        {
            string sql = $"UPDATE [{tableName}] SET ";

            var updateFieldNames = updateExpressions.Select(expr =>
            {
                var memberExpression = expr.Body as MemberExpression;
                if (memberExpression == null && expr.Body is UnaryExpression unaryExpression) memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression == null) throw new ArgumentException("无效的表达式。只支持属性表达式。");
                return memberExpression.Member.Name;
            }).ToArray();

            foreach (var item in updateFieldNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);

            Dictionary<string, SimplePropertyInfo> fieldInfos;
            if (whereObject is ExpandoObject) fieldInfos = getFieldInfos(whereObject as ExpandoObject, true);
            else fieldInfos = getFieldInfos(whereObject, true);

            if (whereString.IsNullOrEmpty())
                makeWhereString(ref sql, ref tableName, ref fieldInfos);
            else sql += " WHERE " + whereString;

            var cmd = SqlAccess.GetCommand(sql);

            cmd.AddParameters(getFieldInfos(obj, false), updateFieldNames);
            cmd.AddParameters(fieldInfos);

            int returnLine = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = returnLine };
        }

        public SqlExecuteResult DeleteObject(string tableName, object whereObject)
        {
            string sql = $"DELETE [{tableName}]";

            var fieldInfos = getFieldInfos(whereObject, true);

            makeWhereString(ref sql, ref tableName, ref fieldInfos);

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);
            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult DeleteObject(string tableName, ExpandoObject whereObject)
        {
            string sql = $"DELETE [{tableName}]";

            var fieldInfos = getFieldInfos(whereObject, true);

            makeWhereString(ref sql, ref tableName, ref fieldInfos);

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);
            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public SqlExecuteResult DeleteObject(string tableName, string whereString, object whereObject = null)
        {
            string sql = $"DELETE [{tableName}]";

            var fieldInfos = getFieldInfos(whereObject, true);

            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);

            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }

        public int Count(string tableName, object whereObject)
        {
            string sql = $"SELECT COUNT(id) FROM [{tableName}]";

            var fieldInfos = getFieldInfos(whereObject, true);

            makeWhereString(ref sql, ref tableName, ref fieldInfos);

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);

            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        public int Count(string tableName, ExpandoObject whereObject)
        {
            string sql = $"SELECT COUNT(id) FROM [{tableName}]";

            var fieldInfos = getFieldInfos(whereObject, true);

            makeWhereString(ref sql, ref tableName, ref fieldInfos);

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);

            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        public int Count(string tableName, string whereString, object whereObject = null)
        {
            string sql = $"SELECT COUNT(id) FROM [{tableName}]";

            var fieldInfos = getFieldInfos(whereObject, true);

            if (!string.IsNullOrEmpty(whereString)) sql += " WHERE " + whereString;

            var cmd = SqlAccess.GetCommand(sql);
            cmd.AddParameters(fieldInfos);

            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public SqlExecuteResult<bool> CreateTable<T>(string tableName)
        {
            var isExistResult = TableIsExist(tableName);
            if (isExistResult.Result) return new SqlExecuteResult<bool>() { Result = false, ResponseRows = isExistResult.ResponseRows, ExecuteSqlString = isExistResult.ExecuteSqlString };
            var props = typeof(T).GetProperties();

            string sql = $"CREATE TABLE [{onlyWords(tableName)}]\r\n";
            sql += $"(\r\n";
            sql += $"[id] int NOT NULL IDENTITY (1, 1),\r\n";
            foreach (var item in props)
            {
                if (item.Name.ToLower() == "id") continue;
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
                else if (type.Name == "Int32[]")
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";
                }
                else if (type.Name == "String[]")
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";
                }
                else if (type.Name == "JObject")
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";
                }
                else
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";

                    //throw new Exception("未知的类型处理（" + type.Name + "）");
                }
            }
            sql += ")  ON [PRIMARY]\r\n";
            //sql += "TEXTIMAGE_ON [PRIMARY]\r\n";
            var result = SqlUtil.ExecuteNonQuery(sql);
            return new SqlExecuteResult<bool>() { Result = true, ExecuteSqlString = sql, ResponseRows = result };
        }
        public SqlExecuteResult<bool> TableIsExist(string tableName)
        {
            var sql = $"SELECT TOP 1 * FROM [SYSOBJECTS] WHERE id = OBJECT_ID(N'{tableName}')";
            var result = SqlUtil.GetRecordCount(sql);

            return new SqlExecuteResult<bool>() { Result = result != 0, ExecuteSqlString = sql, ResponseRows = result };
        }


        public SqlAccessTransaction BeginTransaction(bool setCurrentTransaction = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return SqlAccess.BeginTransaction(setCurrentTransaction, isolationLevel);
        }
        public void CommitTransaction(SqlAccessTransaction transaction)
        {
            SqlAccess.CommitTransaction(transaction);
        }
        public void RollbackTransaction(SqlAccessTransaction transaction)
        {
            SqlAccess.RollbackTransaction(transaction);
        }



        private string makeSelectFieldString(Dictionary<string, SimplePropertyInfo> fieldInfos, string tableName, ref SqlQueryOptions options)
        {
            if (options.FieldOption == SelectFieldOption.All)
            {
                return "*";
            }
            else if (options.FieldOption == SelectFieldOption.SpecifiedObject)
            {
                string s = "";
                foreach (var item in fieldInfos)
                {
                    if (options.ExcludedField?.Contains(item.Value.Name) ?? false) continue;
                    s += "[" + tableName + "].[" + item + "], ";
                }
                return s.Substring(0, s.Length - 2);
            }
            else return "*";
        }
        private void makeOnString(ref string sql, ref string tableName, JoinOption onOption)
        {
            sql += $" {onOption.On.ToString().ToUpper()} JOIN [{onOption.TableName}] ON (";
            foreach (var item in onOption.FieldNames)
            {
                sql += $"[{tableName}].[{item}] = [{onOption.TableName}].[{item}] AND ";
            }
            sql = sql.Substring(0, sql.Length - 5);
            sql += ")";
        }
        private void makeWhereString(ref string sql, ref string tableName, ref Dictionary<string, SimplePropertyInfo> fieldInfos, bool addWhereStr = true)
        {
            if (fieldInfos.Count != 0)
            {
                if (addWhereStr) sql += " WHERE ";
                foreach (var item in fieldInfos)
                {
                    if (item.Value.Value == null)
                        sql += $"[{tableName}].[{item.Value.Name}] IS NULL AND ";
                    else
                        sql += $"[{tableName}].[{item.Value.Name}] = @{item.Value.Name} AND ";
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


        private string[] getPropertyNames(Dictionary<string, SimplePropertyInfo> props, bool includeId = false)
        {
            List<string> propertyNames = new List<string>();
            foreach (var item in props)
            {
                if (item.Key.ToLower() == "id" && !includeId) continue;
                propertyNames.Add(item.Key);
            }
            return propertyNames.ToArray();
        }

        private Dictionary<string, SimplePropertyInfo> getFieldInfos(object obj, bool includeId = false)
        {
            Dictionary<string, SimplePropertyInfo> propertyNames = new Dictionary<string, SimplePropertyInfo>();
            if (obj != null)
            {
                var propertyInfos = obj.GetType().GetProperties();
                foreach (var item in propertyInfos)
                {
                    if (item.Name.ToLower() == "id" && !includeId) continue;
                    propertyNames.Add(item.Name, item.GetFieldInfo(obj));
                }
            }
            return propertyNames;
        }
        private Dictionary<string, SimplePropertyInfo> getFieldInfos(ExpandoObject obj, bool includeId = false)
        {
            Dictionary<string, SimplePropertyInfo> propertyNames = new Dictionary<string, SimplePropertyInfo>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    if (item.Key == "id" && !includeId) continue;
                    propertyNames.Add(item.Key, new SimplePropertyInfo() { Name = item.Key, Value = item.Value, Type = item.Value.GetType() });
                }
            }
            return propertyNames;
        }


        private string onlyWords(string s)
        {
            return s;
        }

        public void Dispose()
        {
            SqlConnection.Dispose();
        }
        ~SqlExecuter()
        {
            SqlConnection.Dispose();
        }
    }
}
