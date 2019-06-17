using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class DateTimeExtension
    {
        public static string GetUnixStyleTimeStamp(this DateTime datetime)
        {
            return SpecialConverter.GET_UNIX_TIMESTAMP(datetime).ToString();
        }
    }
}
