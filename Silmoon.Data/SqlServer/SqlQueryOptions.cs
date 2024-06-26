using Silmoon.Data.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.SqlServer
{
    public class SqlQueryOptions : QueryOptions
    {
        public JoinOption Join { get; set; }
        public SelectFieldOption FieldOption { get; set; } = SelectFieldOption.All;

        private SqlQueryOptions()
        {

        }
        private SqlQueryOptions(int? offset, int? count) : base(offset, count) { }
        private SqlQueryOptions(int? offset, int? count, Sort[] sorts) : base(offset, count, sorts) { }
        private SqlQueryOptions(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption) : base(offset, count, sorts)
        {
            FieldOption = fieldOption;
        }
        private SqlQueryOptions(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, string[] excludedField) : base(offset, count, sorts, excludedField)
        {
            FieldOption = fieldOption;
        }
        private SqlQueryOptions(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, JoinOption join, string[] excludedField) : base(offset, count, sorts, excludedField)
        {
            FieldOption = fieldOption;
            Join = join;
        }
        private SqlQueryOptions(int? offset, int? count, Sort sort, SelectFieldOption fieldOption, JoinOption join, string[] excludedField) : base(offset, count, sort, excludedField)
        {
            FieldOption = fieldOption;
            Join = join;
        }

        public static SqlQueryOptions Create() => new SqlQueryOptions();
        public static SqlQueryOptions Create(int? offset, int? count, Sort[] sorts) => new SqlQueryOptions(offset, count, sorts);
        public static SqlQueryOptions Create(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption) => new SqlQueryOptions(offset, count, sorts, fieldOption);
        public static SqlQueryOptions Create(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, string[] excludedField) => new SqlQueryOptions(offset, count, sorts, fieldOption, excludedField);
        public static SqlQueryOptions Create(int? offset, int? count, Sort[] sorts, SelectFieldOption fieldOption, JoinOption join, string[] excludedField) => new SqlQueryOptions(offset, count, sorts, fieldOption, join, excludedField);
        public static SqlQueryOptions Create(int? offset, int? count, Sort sort) => new SqlQueryOptions(offset, count, sort, default, null, null);
        public static SqlQueryOptions Create(int? offset, int? count, Sort sort, SelectFieldOption fieldOption) => new SqlQueryOptions(offset, count, sort, fieldOption, null, null);
        public static SqlQueryOptions Create(int? offset, int? count, Sort sort, SelectFieldOption fieldOption, string[] excludedField) => new SqlQueryOptions(offset, count, sort, fieldOption, null, excludedField);
        public static SqlQueryOptions Create(int? offset, int? count, Sort sort, SelectFieldOption fieldOption, JoinOption join, string[] excludedField) => new SqlQueryOptions(offset, count, sort, fieldOption, join, excludedField);
    }
}
