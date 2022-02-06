using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class IntExtension
    {
        public static T ToEnum<T>(this int value) where T : Enum
        {
            return (T)(object)value;
        }
    }
}
