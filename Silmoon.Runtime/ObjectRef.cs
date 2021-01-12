using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Silmoon.Runtime
{
    public class ObjectRef
    {
        public static string[] GetPropertyNames(Type type, params string[] exclude)
        {
            List<string> propertyNames = new List<string>();
            var propertyInfos = type.GetProperties();
            foreach (var item in propertyInfos)
            {
                if (exclude.Contains(item.Name)) continue;
                propertyNames.Add(item.Name);
            }
            return propertyNames.ToArray();
        }

        public static Dictionary<string, PropertyInfo> GetProperties(object obj, params string[] exclude)
        {
            Dictionary<string, PropertyInfo> propertyNames = new Dictionary<string, PropertyInfo>();
            if (obj != null)
            {
                var propertyInfos = obj.GetType().GetProperties();
                foreach (var item in propertyInfos)
                {
                    if (exclude.Contains(item.Name)) continue;
                    propertyNames.Add(item.Name, item);
                }
            }
            return propertyNames;
        }

    }
}
