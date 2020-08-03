using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class EnumExtension
    {
        public static T Parse<T>(string value) where T : Enum
        {
            var type = typeof(T);
            var result = (T)Enum.Parse(type, value);
            return result;
        }
    }
}
