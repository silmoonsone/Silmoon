using Silmoon.Data.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data
{
    public class QueryOptions
    {
        public Sort[] Sorts { get; set; } = null;
        public int? Offset { get; set; } = null;
        public int? Count { get; set; } = null;
        public string[] ExcludedField { get; set; } = null;

        public QueryOptions()
        {

        }
        public QueryOptions(int? offset, int? count)
        {
            Offset = offset;
            Count = count;
        }
        public QueryOptions(int? offset, int? count, Sort sort, params string[] excludedField)
        {
            Offset = offset;
            Count = count;
            Sorts = new Sort[] { sort };
            ExcludedField = excludedField;
        }
        public QueryOptions(int? offset, int? count, Sort[] sorts, params string[] excludedField)
        {
            Offset = offset;
            Count = count;
            Sorts = sorts;
            ExcludedField = excludedField;
        }
        public static QueryOptions Create(int? offset, int? count) => new QueryOptions(offset, count);
        public static QueryOptions Create(int? offset, int? count, params string[] excludedField) => new QueryOptions(offset, count, (Sort[])null, excludedField);
        public static QueryOptions Create(int? offset, int? count, Sort sort, params string[] excludedField) => new QueryOptions(offset, count, sort, excludedField);
        public static QueryOptions Create(int? offset, int? count, Sort[] sorts, params string[] excludedField) => new QueryOptions(offset, count, sorts, excludedField);
    }
}
