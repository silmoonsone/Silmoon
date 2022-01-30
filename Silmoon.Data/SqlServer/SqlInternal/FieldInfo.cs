using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.SqlServer.SqlInternal
{
    public class FieldInfo
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
        public override string ToString()
        {
            return $"Name={Name};Value={Value};Type={Type.Name};";
        }
    }
}
