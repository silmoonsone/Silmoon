using Silmoon.Attributes;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Silmoon.Runtime
{
    public static class ObjectRef
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
        public static string[] GetPropertyNames<T>()
        {
            return GetPropertyNames(typeof(T));
        }
        public static string[] GetPropertyNames<T>(params string[] exclude)
        {
            return GetPropertyNames(typeof(T), exclude);
        }
        public static string[] GetPropertyNames(this object obj)
        {
            return GetPropertyNames(obj.GetType());
        }
        public static string[] GetPropertyNames(this object obj, params string[] exclude)
        {
            return GetPropertyNames(obj.GetType(), exclude);
        }
        public static string[] GetPropertyNames(this ExpandoObject obj, params string[] exclude)
        {
            List<string> propertyNames = new List<string>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    if (exclude.Contains(item.Key)) continue;
                    propertyNames.Add(item.Key);
                }
            }
            return propertyNames.ToArray();
        }

        [Obsolete]
        public static Dictionary<string, PropertyInfo> GetProperties(this object obj, params string[] exclude)
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
        [Obsolete]
        public static Dictionary<string, SimplePropertyInfo> GetProperties(this ExpandoObject obj, params string[] exclude)
        {
            Dictionary<string, SimplePropertyInfo> propertyNames = new Dictionary<string, SimplePropertyInfo>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    if (exclude.Contains(item.Key)) continue;
                    propertyNames.Add(item.Key, new SimplePropertyInfo() { Name = item.Key, Value = item.Value, Type = item.Value.GetType() });
                }
            }
            return propertyNames;
        }

        public static Dictionary<string, PropertyValueInfo> GetPropertyValues(this object obj, params string[] excludePropertyNames)
        {
            var excludedNameSet = new HashSet<string>(excludePropertyNames);
            Dictionary<string, PropertyValueInfo> propertyNames = new Dictionary<string, PropertyValueInfo>();
            if (obj != null)
            {
                var propertyInfos = obj.GetType().GetProperties();
                foreach (var item in propertyInfos)
                {
                    if (!excludedNameSet.Contains(item.Name) && item.GetCustomAttribute<IgnorePropertyAttribute>() == null)
                    {
                        propertyNames[item.Name] = item.GetPropertyValueInfo(obj);
                    }
                }
            }
            return propertyNames;
        }
    }
}
