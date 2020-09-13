using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.SqlTypes;
using Silmoon.Reflection;

namespace Silmoon.Data.SqlServer
{
    public class SqlHelper
    {
        public static (T, NameObjectCollection<object>) MakeObject<T>(SqlDataReader reader, bool closeReader = true) where T : new()
        {
            T obj = new T();
            return MakeObject(reader, obj, closeReader);
        }
        public static (T, NameObjectCollection<object>) MakeObject<T>(SqlDataReader reader, T obj, bool closeReader = true) where T : new()
        {
            NameObjectCollection<object> data = new NameObjectCollection<object>();
            for (int i = 0; i < reader.FieldCount; i++)
                data.Add(reader.GetName(i), reader[i]);


            var propertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo item in propertyInfos)
            {
                string name = item.Name;
                Type type = item.PropertyType;
                if (reader[name] != DBNull.Value)
                {
                    if (type.IsEnum)
                        item.SetValue(obj, Enum.Parse(type, reader[name].ToString()), null);
                    else
                        item.SetValue(obj, reader[name], null);
                }
            }
            if (closeReader) reader.Close();
            return (obj, data);
        }
        public static (T[], NameObjectCollection<object>[]) MakeObjects<T>(SqlDataReader reader) where T : new()
        {
            List<T> result = new List<T>();
            List<NameObjectCollection<object>> data = new List<NameObjectCollection<object>>();
            while (reader.Read())
            {
                var r = MakeObject<T>(reader, false);
                result.Add(r.Item1);
                data.Add(r.Item2);
            }
            reader.Close();
            return (result.ToArray(), data.ToArray());
        }

        public static (T, NameObjectCollection<object>) MakeObject<T>(DataRow row) where T : new()
        {
            T obj = new T();
            return MakeObject(row, obj);
        }
        public static (T, NameObjectCollection<object>) MakeObject<T>(DataRow row, T obj) where T : new()
        {
            NameObjectCollection<object> data = new NameObjectCollection<object>();
            for (int i = 0; i < row.Table.Columns.Count; i++)
                data.Add(row.Table.Columns[i].ColumnName, row[i]);

            var propertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo item in propertyInfos)
            {
                string name = item.Name;
                Type type = item.PropertyType;
                if (row[name] != DBNull.Value)
                {
                    if (type.IsEnum)
                        item.SetValue(obj, Enum.Parse(type, row[name].ToString()), null);
                    else
                        item.SetValue(obj, row[name], null);
                }
            }

            return (obj, data);
        }
        public static (T[], NameObjectCollection<object>[]) MakeObjects<T>(DataTable dt) where T : new()
        {
            T[] result = new T[dt.Rows.Count];
            NameObjectCollection<object>[] data = new NameObjectCollection<object>[dt.Rows.Count];

            for (int i = 0; i < result.Length; i++)
            {
                var r = MakeObject<T>(dt.Rows[i]);
                result[i] = r.Item1;
                data[i] = r.Item2;
            }

            return (result, data);
        }
        public static void AddSqlCommandParameters(SqlCommand sqlCommand, object obj, params string[] paraNames)
        {
            if (obj != null)
            {
                var propertyInfos = obj.GetType().GetProperties();
                foreach (PropertyInfo item in propertyInfos)
                {
                    string name = item.Name;
                    if (paraNames.Contains(name))
                    {
                        var value = item.GetValue(obj, null);
                        Type type = item.PropertyType;

                        if (!sqlCommand.Parameters.Contains(name))
                        {
                            if (value != null)
                            {
                                if (type.IsEnum)
                                    ///这里存储在数据库中的枚举使用字符串（.ToString()）
                                    sqlCommand.Parameters.AddWithValue(name, value.ToString());
                                else if (type.Name == "DateTime" && ((DateTime)value) == DateTime.MinValue)
                                    sqlCommand.Parameters.AddWithValue(name, SqlDateTime.MinValue);
                                else
                                    sqlCommand.Parameters.AddWithValue(name, value);
                            }
                            else
                            {
                                if (type.Name == "Byte[]")
                                    sqlCommand.Parameters.AddWithValue(name, SqlBinary.Null);
                                else
                                    sqlCommand.Parameters.AddWithValue(name, DBNull.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}
