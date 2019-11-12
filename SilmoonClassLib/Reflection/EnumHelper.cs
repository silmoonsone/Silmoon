using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Reflection
{
    public class EnumHelper
    {
        public static string GetEnumDisplayName(Enum @enum)
        {
            var type = @enum.GetType();
            var field = type.GetField(@enum.ToString());
            var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return obj?.Name ?? @enum.ToString();
        }
    }
}
