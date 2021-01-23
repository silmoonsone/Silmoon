using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class DateTimeExtension
    {
        public static long ToUnixStyleTimeStamp(this DateTime datetime)
        {
            return SpecialConverter.GET_UNIX_TIMESTAMP(datetime);
        }
        public static string ToChineseFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
