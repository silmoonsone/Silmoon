using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

namespace Silmoon
{
    [Obsolete]
    public class SmInt
    {
        public static bool ChkIntLengthMin(int sint, int minlen)
        {
            int slen = sint.ToString().Length;
            return (sint >= minlen);
        }
        public static bool ChkIntLengthMax(int sint, int maxlen)
        {
            int slen = sint.ToString().Length;
            return (sint <= maxlen);
        }
        public static bool ChkIntLength(int sint, int minlen, int maxlen)
        {
            int sintlen = sint.ToString().Length;
            return (sintlen <= maxlen && sintlen >= minlen);
        }
        public static int ChkIntLengthMinThrowEx(int sint, int minlen)
        {
            int sintlen = sint.ToString().Length;
            if (sintlen < minlen)
            {
                throw new Exception("sint length(min) reject");
            }
            return sint;
        }
        public static int ChkIntLengthMaxThrowEx(int sint, int maxlen)
        {
            int sintlen = sint.ToString().Length;
            if (sintlen > maxlen)
            {
                throw new Exception("sint length(max) reject");
            }
            return sint;
        }
        public static int ChkIntLengthThrowEx(int sint, int minlen, int maxlen)
        {
            int sintlen = sint.ToString().Length;
            if (sintlen > maxlen || sintlen < minlen)
            {
                throw new Exception("sint length reject");
            }
            return sint;
        }

        public static bool ChkIntValue(int sint, int min, int max)
        {
            return (sint >= min && sint <= max);
        }
        public static int CheckIntValue(int sint, int min, int max, bool throwException)
        {
            if (sint < min)
            {
                if (throwException) throw new ArgumentException("参数 数字 应大于等于！" + min.ToString());
                sint = min;
            }
            else if (sint > max)
            {
                if (throwException) throw new ArgumentException("参数 数字 应小于等于！" + max.ToString());
                sint = max;
            }
            return sint;
        }
        public static int CheckIntValueMin(int sint, int min)
        {
            if (sint < min) { sint = min; }
            return sint;
        }
        public static int CheckIntValueMax(int sint, int max)
        {
            if (sint > max) { sint = max; }
            return sint;
        }
    }
}
