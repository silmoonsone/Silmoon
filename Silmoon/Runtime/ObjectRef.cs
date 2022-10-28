using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using FieldInfo = Silmoon.Runtime.FieldInfo;

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
        public static Dictionary<string, FieldInfo> GetProperties(this ExpandoObject obj, params string[] exclude)
        {
            Dictionary<string, FieldInfo> propertyNames = new Dictionary<string, FieldInfo>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    if (exclude.Contains(item.Key)) continue;
                    propertyNames.Add(item.Key, new FieldInfo() { Name = item.Key, Value = item.Value, Type = item.Value.GetType() });
                }
            }
            return propertyNames;
        }

    }
}
