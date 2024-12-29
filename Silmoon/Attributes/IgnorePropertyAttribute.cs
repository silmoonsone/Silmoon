using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}
