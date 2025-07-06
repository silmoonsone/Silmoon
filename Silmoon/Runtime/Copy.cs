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

        public static List<destT> EnumerableNew<destT>(IEnumerable obj) where destT : new()
        {
            List<destT> list = new List<destT>();
            foreach (var item in obj)
            {
                list.Add(MemberCopy(item, new destT()));
            }
            return list;
        }
        public static List<destT> EnumerableNew<destT>(IEnumerable obj, Func<object, destT> createFunc)
        {
            List<destT> list = new List<destT>();

            foreach (var item in obj)
            {
                list.Add(MemberCopy(item, createFunc(item)));
            }
            return list;
        }

        public static destT[] EnumerableNew<sourceT, destT>(IEnumerable<sourceT> obj) where destT : new()
        {
            destT[] list = new destT[obj.Count()];
            for (int i = 0; i < obj.Count(); i++)
            {
                list[i] = MemberCopy(obj.ElementAt(i), new destT());
            }
            return list;
        }
        public static destT[] EnumerableNew<sourceT, destT>(IEnumerable<sourceT> obj, Func<sourceT, destT> createFunc) where destT : new()
        {
            destT[] list = new destT[obj.Count()];
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
        public static destT[] ArrayNew<destT>(Array array, Func<object, destT> createFunc) where destT : new()
        {
            destT[] list = new destT[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                list[i] = MemberCopy(array.GetValue(i), createFunc(array.GetValue(i)));
            }
            return list;
        }
        public static destT[] ArrayNew<sourceT, destT>(sourceT[] array) where destT : new()
        {
            destT[] list = new destT[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                list[i] = MemberCopy(array[i], new destT());
            }
            return list;
        }
        public static destT[] ArrayNew<sourceT, destT>(sourceT[] array, Func<sourceT, destT> createFunc) where destT : new()
        {
            destT[] list = new destT[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                list[i] = MemberCopy(array[i], createFunc(array[i]));
            }
            return list;
        }
    }
}
