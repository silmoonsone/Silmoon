using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon
{
    public class Copy
    {
        public static object MemberCopy(object from, object to)
        {
            Type tto = to.GetType();

            var p = from.GetType().GetProperties();
            foreach (var item in p)
            {
                var property = tto.GetProperty(item.Name);
                if (property.GetSetMethod() != null && property.GetSetMethod().IsPublic)
                    property.SetValue(to, item.GetValue(from, null), null);
            }
            return to;
        }
        public static T New<T>(object obj) where T : new()
        {
            T t = new T();
            MemberCopy(obj, t);

            return t;
        }

    }
}
