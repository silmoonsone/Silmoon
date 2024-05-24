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
        public static Td MemberCopy<Ts, Td>(Ts source, Td dest)
        {
            if (source == null || dest == null) return default;
            Type tto = typeof(Td);

            var p = source.GetType().GetProperties();
            foreach (var item in p)
            {
                var property = tto.GetProperty(item.Name);
                if (property != null && property.GetSetMethod() != null && property.GetSetMethod().IsPublic)
                    property.SetValue(dest, item.GetValue(source, null), null);
            }
            return dest;
        }

        public static T New<T>(object obj) where T : new() => MemberCopy(obj, new T());
        public static T New<T>(object obj, Func<object, T> createFunc) where T : new() => MemberCopy(obj, createFunc(obj));
        public static Td New<Ts, Td>(Ts obj) where Td : new() => MemberCopy(obj, new Td());
        public static Td New<Ts, Td>(Ts obj, Func<Ts, Td> createFunc) where Td : new() => MemberCopy(obj, createFunc(obj));

        public static List<T> EnumerableNew<T>(IEnumerable obj) where T : new()
        {
            List<T> list = new List<T>();
            foreach (var item in obj)
            {
                list.Add(MemberCopy(item, new T()));
            }
            return list;
        }
        public static List<T> EnumerableNew<T>(IEnumerable obj, Func<object, T> createFunc)
        {
            List<T> list = new List<T>();

            foreach (var item in obj)
            {
                list.Add(MemberCopy(item, createFunc(item)));
            }
            return list;
        }

        public static Td[] EnumerableNew<Ts, Td>(IEnumerable<Ts> obj) where Td : new()
        {
            Td[] list = new Td[obj.Count()];
            for (int i = 0; i < obj.Count(); i++)
            {
                list[i] = MemberCopy(obj.ElementAt(i), new Td());
            }
            return list;
        }
        public static Td[] EnumerableNew<Ts, Td>(IEnumerable<Ts> obj, Func<Ts, Td> createFunc) where Td : new()
        {
            Td[] list = new Td[obj.Count()];
            for (int i = 0; i < obj.Count(); i++)
            {
                list[i] = MemberCopy(obj.ElementAt(i), createFunc(obj.ElementAt(i)));
            }
            return list;
        }


        public static Td[] ArrayNew<Td>(Array array) where Td : new()
        {
            Td[] list = new Td[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                list[i] = MemberCopy(array.GetValue(i), new Td());
            }
            return list;
        }
        public static Td[] ArrayNew<Td>(Array array, Func<object, Td> createFunc) where Td : new()
        {
            Td[] list = new Td[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                list[i] = MemberCopy(array.GetValue(i), createFunc(array.GetValue(i)));
            }
            return list;
        }
        public static Td[] ArrayNew<Ts, Td>(Ts[] array) where Td : new()
        {
            Td[] list = new Td[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                list[i] = MemberCopy(array[i], new Td());
            }
            return list;
        }
        public static Td[] ArrayNew<Ts, Td>(Ts[] array, Func<Ts, Td> createFunc) where Td : new()
        {
            Td[] list = new Td[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                list[i] = MemberCopy(array[i], createFunc(array[i]));
            }
            return list;
        }
    }
}
