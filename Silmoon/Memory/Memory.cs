using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Silmoon.Memory
{
    public class Memory
    {
        public static void RemoveString(ref ArrayList array, string sDest)
        {
            if (array == null || array.Count == 0) return;

            int arrayLength = array.Count;

            for (int i = 0; i < arrayLength; i++)
            {
                if (array[i].ToString() == sDest)
                {
                    array.RemoveAt(i);
                    arrayLength--;
                }
            }

        }
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

        public static void MemCat(ref ArrayList dArray1, ref ArrayList sArray2)
        {
            if (dArray1 == null || dArray1.Count == 0) return;
            if (sArray2 == null || sArray2.Count == 0) return;

            ArrayList array = new ArrayList();
            for (int i = 0; i < dArray1.Count; i++)
                array.Add(dArray1[i]);

            for (int i = 0; i < sArray2.Count; i++)
                array.Add(sArray2[i]);

            dArray1 = array;
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
        public static bool MemCat(ref ArrayList dArray1, ref byte[] scrData, int scrIndex, int offset)
        {
            if (scrData.Length < scrIndex + offset) return false;
            for (int i = 0; i < offset; i++)
            {
                dArray1.Add(scrData[scrIndex + i]);
            }
            return true;
        }
        public static bool MemCat(ref List<byte> dArray1, ref byte[] scrData, int scrIndex, int offset)
        {
            if (scrData.Length < scrIndex + offset) return false;
            for (int i = 0; i < offset; i++)
            {
                dArray1.Add(scrData[scrIndex + i]);
            }
            return true;
        }

        public static void MemCpy(ref byte[] destByte, ref byte[] scrByte)
        {
            if (destByte == null || destByte.Length == 0) return;
            if (scrByte == null || scrByte.Length == 0) return;

            for (int i = 0; i < destByte.Length; i++)
                destByte[i] = scrByte[i];
        }
        /// <summary>
        /// 复制byte数组到另一个数组中
        /// </summary>
        /// <param name="destByte">目标byte数组</param>
        /// <param name="scrByte">源byte数组</param>
        /// <param name="destIndex">目标数组开始位置</param>
        /// <param name="scrIndex">源数组开始位置</param>
        /// <param name="offset">复制长度，如果超出了源和目标数组，复制将会失败</param>
        public static bool MemCpy(ref byte[] destByte, ref byte[] scrByte,int destIndex, int scrIndex, int offset)
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
        public static void MemSet(ArrayList array, object obj)
        {
            for (int i = 0; i < array.Count; i++)
            {
                array[i] = obj;
            }
        }

        public static int Find(ref ArrayList dArray1, ref object fObject)
        {
            int result = 0;

            if (dArray1 == null || dArray1.Count == 0) return result;

            for (int i = 0; i < dArray1.Count; i++)
            {
                if (dArray1[1] == fObject) result++;
            }
            return result;
        }
        public static int Find(ref string[] dArray1, ref string fString)
        {
            int result = 0;

            if (dArray1 == null || dArray1.Length == 0) return result;

            for (int i = 0; i < dArray1.Length; i++)
            {
                if (dArray1[1] == fString) result++;
            }
            return result;
        }
        public static int Find(ref object[] dArray1, ref object[] fObject)
        {
            int result = 0;

            if (dArray1 == null || dArray1.Length == 0) return result;

            for (int i = 0; i < dArray1.Length; i++)
            {
                if (dArray1[1] == fObject) result++;
            }
            return result;
        }

        public static Array ToArray(ref object obj)
        {
            Array array = (Array)obj;
            return array;
        }
    }
}
