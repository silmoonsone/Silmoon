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
            return Name.GetHashCode() + Value.GetHashCode();
        }
    }
}
