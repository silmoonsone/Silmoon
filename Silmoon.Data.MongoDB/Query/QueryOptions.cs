using Silmoon.Data.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.MongoDB.Query
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
        private QueryOptions(int? offset, int? count)
        {
            Offset = offset;
            Count = count;
        }
        private QueryOptions(int? offset, int? count, Sort sort, params string[] excludedField)
        {
            Sorts = new Sort[] { sort };
            Offset = offset;
            Count = count;
            ExcludedField = excludedField;
        }
        private QueryOptions(int? offset, int? count, Sort[] sorts, params string[] excludedField)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
            ExcludedField = excludedField;
        }
        public static QueryOptions Create(int? offset, int? count)
        {
            return new QueryOptions(offset, count);
        }
        public static QueryOptions Create(int? offset, int? count, params string[] excludedField)
        {
            return new QueryOptions(offset, count, (Sort[])null, excludedField);
        }
        public static QueryOptions Create(int? offset, int? count, Sort sort, params string[] excludedField)
        {
            return new QueryOptions(offset, count, sort, excludedField);
        }
        public static QueryOptions Create(int? offset, int? count, Sort[] sorts, params string[] excludedField)
        {
            return new QueryOptions(offset, count, sorts, excludedField);
        }
    }
}
