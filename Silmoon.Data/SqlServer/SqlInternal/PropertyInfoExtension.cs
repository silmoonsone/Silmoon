using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FieldInfo = Silmoon.Runtime.FieldInfo;

namespace Silmoon.Data.SqlServer.SqlInternal
{
    public static class PropertyInfoExtension
    {
        public static FieldInfo GetFieldInfo(this PropertyInfo propertyInfo, object obj)
        {
            return new FieldInfo() { Name = propertyInfo.Name, Type = propertyInfo.PropertyType, Value = propertyInfo.GetValue(obj) };
        }
    }
}
