using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class NameValue : NameValue<string>, INameValue
    {
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
            int hash = 17;
            hash = hash * 31 + (Name != null ? Name.GetHashCode() : 0);
            hash = hash * 31 + Value.GetHashCode();
            return hash;
        }
    }
}
