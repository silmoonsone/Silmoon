using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.QueryModel
{
    public class Sort
    {
        public Sort()
        {

        }
        public Sort(string name, SortMethod method)
        {
            Name = name;
            Method = method;
        }
        public string Name { get; set; }
        public SortMethod Method { get; set; }

    }
}
