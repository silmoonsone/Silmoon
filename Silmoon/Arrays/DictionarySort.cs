using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Arrays
{
    [Obsolete("Please try the new implementation DictionarySortV2.")]
    public class DictionarySort : System.Collections.IComparer
    {
        public int Compare(object left, object right)
        {
            string sLeft = left as string;
            string sRight = right as string;
            int iLeftLength = sLeft.Length;
            int iRightLength = sRight.Length;
            int index = 0;
            while (index < iLeftLength && index < iRightLength)
            {
                if (sLeft[index] < sRight[index])
                    return -1;
                else if (sLeft[index] > sRight[index])
                    return 1;
                else
                    index++;
            }
            return iLeftLength - iRightLength;

        }
    }
}
