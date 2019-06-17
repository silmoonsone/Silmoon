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
            if (from == null || to == null) return null;
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
        public static object MemberCopy_T<sT, dT>(sT source, dT dest)
        {
            if (source == null || dest == null) return null;
            Type tto = typeof(dT);

            var p = typeof(sT).GetProperties();
            foreach (var item in p)
            {
                var property = tto.GetProperty(item.Name);
                if (property.GetSetMethod() != null && property.GetSetMethod().IsPublic)
                    property.SetValue(dest, item.GetValue(source, null), null);
            }
            return dest;
        }
        public static T New<T>(object obj) where T : new()
        {
            T t = new T();
            MemberCopy(obj, t);

            return t;
        }
        public static dT New<sT, dT>(sT obj) where dT : new()
        {
            dT t = new dT();
            MemberCopy_T<sT, dT>(obj, t);

            return t;
        }

    }
}
