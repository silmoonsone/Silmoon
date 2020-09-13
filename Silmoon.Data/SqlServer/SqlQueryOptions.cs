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
        public SqlQueryOptions(int? offset, int? count, Sort[] sorts)
        {
            Sorts = sorts;
            Offset = offset;
            Count = count;
        }
        public Sort[] Sorts { get; set; }
        public int? Offset { get; set; }
        public int? Count { get; set; }
        public OnOption OnOption { get; set; }
    }
}
