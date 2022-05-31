using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class ObjectExtension
    {
        public static string ToStringOrNull(this object obj)
        {
            return obj?.ToString();
        }
    }
}
