using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Silmoon.Runtime
{
    public static class Memory
    {
        public static void RemoveString(ref string[] array, string sDest)
        {
            if (array == null || array.Length == 0) return;

            ArrayList tmpArr = new ArrayList();
            for (int i = 0; i < array.Length; i++)
                tmpArr.Add(array[i]);

            int arrayLength = tmpArr.Count;

            for (int i = 0; i < arrayLength; i++)
            {
                if (tmpArr[i].ToString() == sDest)
                {
                    tmpArr.RemoveAt(i);
                    arrayLength--;
                }
            }
            array = (string[])tmpArr.ToArray(typeof(string));

        }

        public static void MemCat(ref string[] dArray1, ref string[] sArray2)
        {
            if (dArray1 == null || dArray1.Length == 0) return;
            if (sArray2 == null || sArray2.Length == 0) return;

            ArrayList array = new ArrayList();
            for (int i = 0; i < dArray1.Length; i++)
                array.Add(dArray1.GetValue(i));

            for (int i = 0; i < sArray2.Length; i++)
                array.Add(sArray2.GetValue(i));

            dArray1 = (string[])array.ToArray(typeof(string));
        }
        public static void MemCat(ref object[] dArray1, ref object[] sArray2)
        {
            if (dArray1 == null || dArray1.Length == 0) return;
            if (sArray2 == null || sArray2.Length == 0) return;

            ArrayList array = new ArrayList();
            for (int i = 0; i < dArray1.Length; i++)
                array.Add(dArray1.GetValue(i));

            for (int i = 0; i < sArray2.Length; i++)
                array.Add(sArray2.GetValue(i));

            dArray1 = (string[])array.ToArray(typeof(string));
        }
        public static void MemCat(ref byte[] dArray1, ref byte[] sArray2)
        {
            if (dArray1 == null || dArray1.Length == 0) return;
            if (sArray2 == null || sArray2.Length == 0) return;

            List<byte> array = new List<byte>();
            for (int i = 0; i < dArray1.Length; i++)
                array.Add(dArray1[i]);

            for (int i = 0; i < sArray2.Length; i++)
                array.Add(sArray2[i]);

            dArray1 = array.ToArray();
        }

        public static void MemCpy(ref byte[] destByte, ref byte[] scrByte)
        {
            if (destByte == null || destByte.Length == 0) return;
            if (scrByte == null || scrByte.Length == 0) return;

            for (int i = 0; i < destByte.Length; i++)
                destByte[i] = scrByte[i];
        }
        public static bool MemCpy(ref byte[] destByte, ref byte[] scrByte, int destIndex, int scrIndex, int offset)
        {
            if (destByte == null || destByte.Length == 0) return false;
            if (scrByte == null || scrByte.Length == 0) return false;
            if (scrByte.Length < scrIndex + offset) return false;
            if (destByte.Length < destIndex + offset) return false;

            for (int i = 0; i < offset; i++)
            {
                destByte[i + destIndex] = scrByte[i + scrIndex];
            }
            return true;
        }
        public static void MemSet(byte[] data, byte byt)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = byt;
            }
        }
    }
}
