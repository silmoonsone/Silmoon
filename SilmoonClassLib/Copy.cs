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
                tto.GetProperty(item.Name).SetValue(to, item.GetValue(from, null), null);
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
