using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.SqlTypes;

namespace Silmoon.Data.SqlServer
{
    public class SqlHelper
    {
        public static T MakeObject<T>(SqlDataReader reader) where T : new()
        {
            T obj = new T();
            return MakeObject(reader, obj);
        }
        public static T MakeObject<T>(SqlDataReader reader, T obj)
        {
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
            reader.Close();
            return obj;
        }
        public static T[] MakeObjects<T>(SqlDataReader reader) where T : new()
        {
            List<T> result = new List<T>();

            while (reader.Read())
            {
                result.Add(MakeObject<T>(reader));
            }
            reader.Close();
            return result.ToArray();
        }

        public static T MakeObject<T>(DataRow row) where T : new()
        {
            T obj = new T();
            return MakeObject(row, obj);
        }
        public static T MakeObject<T>(DataRow row, T obj)
        {
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

            return obj;
        }
        public static T[] MakeObjects<T>(DataTable dt) where T : new()
        {
            T[] result = new T[dt.Rows.Count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = MakeObject<T>(dt.Rows[i]);
            }

            return result;
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

                        if (value != null)
                        {
                            if (type.IsEnum)
                                sqlCommand.Parameters.AddWithValue(name, value.ToString());
                            else if (type.Name == "DateTime" && ((DateTime)value) == DateTime.MinValue)
                                sqlCommand.Parameters.AddWithValue(name, SqlDateTime.MinValue);
                            else
                                sqlCommand.Parameters.AddWithValue(name, value);
                        }
                        else
                            sqlCommand.Parameters.AddWithValue(name, DBNull.Value);
                    }
                }
            }
        }
    }
}
