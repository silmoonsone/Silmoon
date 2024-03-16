using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class NameValue : INameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public NameValue()
        {

        }
        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public override bool Equals(object obj)
        {
            if (obj is NameValue nameValue)
                return nameValue.Name == Name && nameValue.Value == Value;
            else
                return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + Value.GetHashCode();
        }
    }
}
