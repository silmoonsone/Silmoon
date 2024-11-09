using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Silmoon.Runtime;

namespace Silmoon.Extension
{
    public static class PropertyInfoExtension
    {
        public static SimplePropertyInfo GetFieldInfo(this PropertyInfo propertyInfo, object obj)
        {
            return new SimplePropertyInfo() { Name = propertyInfo.Name, Type = propertyInfo.PropertyType, Value = propertyInfo.GetValue(obj) };
        }
    }
}
