using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Runtime
{
    public class CSharpHelper
    {
        [Obsolete]
        public static string[] NamesOf(params object[] objects)
        {
            string[] result = new string[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                //result[i] = nameof(objects[i]);
            }
            return result;
        }
    }
}
