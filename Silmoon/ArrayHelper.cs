using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon
{
    public class ArrayHelper
    {
        public static T[] Merge<T>(T[] array1, T[] array2)
        {
            List<T> list = new List<T>();
            foreach (var item in array1)
            {
                list.Add(item);
            }

            foreach (var item in array2)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            return list.ToArray();
        }
    }
}
