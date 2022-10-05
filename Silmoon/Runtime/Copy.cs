using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Runtime
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
                if (property != null && property.GetSetMethod() != null && property.GetSetMethod().IsPublic)
                    property.SetValue(to, item.GetValue(from, null), null);
            }
            return to;
        }
        public static object MemberCopy<sT, dT>(sT source, dT dest)
        {
            if (source == null || dest == null) return null;
            Type tto = typeof(dT);

            var p = source.GetType().GetProperties();
            foreach (var item in p)
            {
                var property = tto.GetProperty(item.Name);
                if (property != null && property.GetSetMethod() != null && property.GetSetMethod().IsPublic)
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
        public static T[] ArrayNew<T>(IEnumerable obj) where T : new()
        {
            List<T> list = new List<T>();

            foreach (var item in obj)
            {
                T t = new T();
                MemberCopy(item, t);
                list.Add(t);
            }

            return list.ToArray();
        }
        public static dT New<sT, dT>(sT obj) where dT : new()
        {
            dT t = new dT();
            MemberCopy(obj, t);

            return t;
        }
        public static dT[] ArrayNew<sT, dT>(IEnumerable<sT> obj) where dT : new()
        {
            List<dT> list = new List<dT>();

            foreach (var item in obj)
            {
                dT t = new dT();
                MemberCopy(obj, t);
                list.Add(t);
            }

            return list.ToArray();
        }

    }
}
