using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Silmoon.Runtime
{
    public class PropertyValueInfo : PropertyValueInfo<object>
    {
        public PropertyValueInfo(PropertyInfo info, object value) : base(info, value) { }
        public PropertyValueInfo(string name, Type type, object value) : base(name, type, value) { }
    }
    public class PropertyValueInfo<T>
    {
        public PropertyValueInfo(string name, Type type, T value)
        {
            Info = null;
            Name = name;
            Type = type;
            Value = value;
        }
        public PropertyValueInfo(PropertyInfo info, T value)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Name = info.Name;
            Type = info.PropertyType;
            Value = value;
        }

        public string Name { get; private set; }
        public Type Type { get; private set; }
        public PropertyInfo Info { get; private set; }
        public T Value { get; set; }
        public override string ToString()
        {
            string reflectedType = Info?.ReflectedType?.ToString() ?? "[Unknown]";
            string propertyType = Type?.ToString() ?? "[Unknown]";
            return $"Name={Name}; Value={Value}; Type={reflectedType}.{propertyType}";
        }
    }
}
