﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class ArrayExtension
    {
        public static string[] ToStringArray(this Array array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array.GetValue(i).ToString();
            }
            return result;
        }
    }
}