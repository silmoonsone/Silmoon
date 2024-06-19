using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class ObjectExtension
    {
        public static string ToStringOrNull(this object obj) => obj?.ToString();
        public static string ToStringOrEmpty(this object obj) => obj?.ToString() ?? string.Empty;
        public static bool IsNullOrDefault<T>(this T obj)
        {
            if (obj == null) return true;
            if (obj.Equals(default(T))) return true;
            return false;
        }
    }
}
