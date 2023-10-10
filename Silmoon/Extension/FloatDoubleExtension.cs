using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class FloatDoubleExtension
    {
        public static decimal ToDecimal(this double value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this float value) => Convert.ToDecimal(value);
    }
}
