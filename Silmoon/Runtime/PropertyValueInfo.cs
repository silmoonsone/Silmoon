using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Silmoon.Runtime
{
    public class PropertyValueInfo : PropertyValueInfo<object>
    {
        public PropertyValueInfo(PropertyInfo info, object value) : base(info, value) { }
    }
    public class PropertyValueInfo<T>
    {
        public PropertyValueInfo(PropertyInfo info, T value)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Value = value;
        }
        public string Name => Info.Name;
        public MemberTypes MemberTypes => Info.MemberType;
        public PropertyInfo Info { get; }
        public T Value { get; set; }
        public override string ToString()
        {
            string reflectedType = Info.ReflectedType?.ToString() ?? "Unknown";
            string propertyType = Info.PropertyType?.ToString() ?? "Unknown";
            return $"Name={Name}; Value={Value}; Type={reflectedType}.{propertyType};";
        }
    }
}
