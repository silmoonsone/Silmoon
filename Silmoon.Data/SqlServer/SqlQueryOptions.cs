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
        private SqlQueryOptions()
        {

        }
        private SqlQueryOptions(int? offset, int? count)
        {
            Offset = offset;
            Count = count;
        }
        private SqlQueryOptions(int? offset, int? count, Sort[] sorts)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
        }
        private SqlQueryOptions(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
            FieldOption = fieldOption;
        }
        private SqlQueryOptions(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, string[] excludedField)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
            FieldOption = fieldOption;
            ExcludedField = excludedField;
        }
        private SqlQueryOptions(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, string[] excludedField, JoinOption join)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
            FieldOption = fieldOption;
            ExcludedField = excludedField;
            Join = join;
        }
        public Sort[] Sorts { get; set; }
        public int? Offset { get; set; }
        public int? Count { get; set; }
        public JoinOption Join { get; set; }
        public SelectFieldOption FieldOption { get; set; } = SelectFieldOption.All;
        public string[] ExcludedField { get; set; }
        public static SqlQueryOptions Create()
        {
            return new SqlQueryOptions();
        }
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
        public static SqlQueryOptions Create(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, string[] excludedField, JoinOption join)
        {
            return new SqlQueryOptions(offset, count, sorts, fieldOption, excludedField, join);
        }
    }
}
