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
        public SqlExecuter(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
        }
        public void AddObject<T>(T obj)
        {
            string sql = "INSERT INTO ";
        }
        public void CreateTable<T>(string tableName)
        {
            var props = getProperties<T>();

            string sql = $"CREATE TABLE {onlyWords(tableName)}\r\n";
            sql += $"(\r\n";
            sql += $"id int NOT NULL IDENTITY (1, 1),\r\n";
            foreach (var item in props)
            {
                var type = item.PropertyType;
                if (type.IsEnum)
                {

                }
            }
        }



        private string[] getPropertyNames<T>()
        {
            List<string> propertyNames = new List<string>();
            var propertyInfos = typeof(T).GetType().GetProperties();
            foreach (var item in propertyInfos)
            {
                if (item.Name.ToLower() == "id") continue;
                propertyNames.Add(item.Name);
            }
            return propertyNames.ToArray();
        }
        private PropertyInfo[] getProperties<T>()
        {
            List<PropertyInfo> propertyNames = new List<PropertyInfo>();
            var propertyInfos = typeof(T).GetType().GetProperties();
            foreach (var item in propertyInfos)
            {
                if (item.Name.ToLower() == "id") continue;
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
