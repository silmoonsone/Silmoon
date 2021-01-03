using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
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
        public static string GetEnumDisplayName(this Enum @enum)
        {
            var type = @enum.GetType();
            var field = type.GetField(@enum.ToString());
            var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return obj?.Name ?? @enum.ToString();
        }
    }
}
