using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.QueryModel
{
    public class Sort
    {
        [Obsolete]
        public Sort()
        {

        }
        [Obsolete]
        public Sort(string name, SortMethod method)
        {
            Name = name;
            Method = method;
        }
        public string Name { get; set; }
        public SortMethod Method { get; set; }

        public static Sort Create(string name, SortMethod sortMethod)
        {
            return new Sort(name, sortMethod);
        }

    }
}
