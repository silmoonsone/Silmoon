using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    public class SpecialConverter
    {
        public static readonly DateTime SqlDefaultDateTime = DateTime.Parse("1/1/1753 0:00:00");

        public static long ToTimeStamp(System.DateTime time)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (long)(time - startTime).TotalSeconds;
            return intResult;
        }

        public static long ToTimeStamp(System.DateTime time, DateTime baseUTCTime)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(baseUTCTime);
            intResult = (long)(time - startTime).TotalSeconds;
            return intResult;
        }

        public static System.DateTime ConvertIntDateTime(long d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        public static System.DateTime ConvertIntDateTime(long d, DateTime baseTime)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(baseTime);
            time = startTime.AddSeconds(d);
            return time;
        }

        public static string ToChnDateTimeString(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToChnDateTimeString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }


        /// <summary>
        /// 将制定的时间转换成时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long UNIX_TIMESTAMP(DateTime dateTime)
        {
            return (dateTime.Ticks - 621355968000000000) / 10000000;
        }
    }
}
