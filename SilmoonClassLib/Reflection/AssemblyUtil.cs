using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Reflection
{
    public class AssemblyUtil
    {
        public static object GetProperty(object obj, string name)
        {
            Type t = obj.GetType();
            return t.GetProperty(name).GetValue(obj, null);
        }
        public static object GetField(object obj, string name)
        {
            Type t = obj.GetType();
            return t.GetField(name).GetValue(obj);
        }
    }
}
