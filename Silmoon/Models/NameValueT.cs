using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class NameValue<T> : INameValue<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public NameValue()
        {

        }
        public NameValue(string name, T value)
        {
            Name = name;
            Value = value;
        }
        public override bool Equals(object obj)
        {
            if (obj is NameValue<T> nameValue)
                return nameValue.Name == Name && nameValue.Value.Equals(Value);
            else
                return false;
        }
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + (Name != null ? Name.GetHashCode() : 0);
            hash = hash * 31 + Value.GetHashCode();
            return hash;
        }
        public override string ToString() => $"{Name}: {Value}";
    }
}
