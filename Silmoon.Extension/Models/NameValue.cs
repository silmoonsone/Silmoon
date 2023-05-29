using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class NameValue : IValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
