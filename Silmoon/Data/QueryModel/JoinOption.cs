using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.QueryModel
{
    public class JoinOption
    {
        public On On { get; set; }
        public string TableName { get; set; }
        public string[] FieldNames { get; set; }
        private JoinOption(string tableName, On on, params string[] fieldNames)
        {
            TableName = tableName;
            On = on;
            FieldNames = fieldNames;
        }
        public static JoinOption Create(string tableName, On on, params string[] fieldNames)
        {
            return new JoinOption(tableName, on, fieldNames);
        }
    }
}
