using Silmoon.Data.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.SqlServer
{
    public class SqlQueryOptions
    {
        public SqlQueryOptions()
        {

        }
        [Obsolete]
        public SqlQueryOptions(int? offset, int? count)
        {
            Offset = offset;
            Count = count;
        }
        [Obsolete]
        public SqlQueryOptions(int? offset, int? count, Sort[] sorts)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
        }
        [Obsolete]
        public SqlQueryOptions(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
            FieldOption = fieldOption;
        }
        [Obsolete]
        public SqlQueryOptions(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, string[] excludedField)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
            FieldOption = fieldOption;
            ExcludedField = excludedField;
        }
        public Sort[] Sorts { get; set; }
        public int? Offset { get; set; }
        public int? Count { get; set; }
        public OnOption OnOption { get; set; }
        public SelectFieldOption FieldOption { get; set; } = SelectFieldOption.All;
        public string[] ExcludedField { get; set; }
        public static SqlQueryOptions Create(int? offset, int? count)
        {
            return new SqlQueryOptions(offset, count);
        }
        public static SqlQueryOptions Create(int? offset, int? count, Sort[] sorts)
        {
            return new SqlQueryOptions(offset, count, sorts);
        }
        public static SqlQueryOptions Create(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption)
        {
            return new SqlQueryOptions(offset, count, sorts, fieldOption);
        }
        public static SqlQueryOptions Create(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, string[] excludedField)
        {
            return new SqlQueryOptions(offset, count, sorts, fieldOption, excludedField);
        }
    }
}
