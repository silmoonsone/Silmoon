using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Silmoon.Runtime;

namespace Silmoon.Extension
{
    public static class PropertyInfoExtension
    {
        [Obsolete]
        public static SimplePropertyInfo GetSimplePropertyInfo(this PropertyInfo propertyInfo, object obj)
        {
            return new SimplePropertyInfo() { Name = propertyInfo.Name, Type = propertyInfo.PropertyType, Value = propertyInfo.GetValue(obj) };
        }
        public static PropertyValueInfo GetPropertyValueInfo(this PropertyInfo propertyInfo, object obj) => new PropertyValueInfo(propertyInfo, propertyInfo.GetValue(obj));
    }
}
