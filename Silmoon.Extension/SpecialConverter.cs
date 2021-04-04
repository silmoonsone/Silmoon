using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Silmoon.Extension
{
    public class SpecialConverter
    {
        public static readonly DateTime SqlMinDateTime = DateTime.Parse("1/1/1753 00:00:00");

        public static long ToTimeStamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long intResult = (long)(time - startTime).TotalSeconds;
            return intResult;
        }
        public static long ToTimeStamp(DateTime time, DateTime baseUTCTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(baseUTCTime);
            long intResult = (long)(time - startTime).TotalSeconds;
            return intResult;
        }

        public static DateTime ConvertIntDateTime(long d)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime time = startTime.AddSeconds(d);
            return time;
        }
        public static DateTime ConvertIntDateTime(long d, DateTime baseTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(baseTime);
            DateTime time = startTime.AddSeconds(d);
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
        public static long GET_UNIX_TIMESTAMP(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        public static DateTime GET_TIME_BY_UNIX_TEMPSTAMP(long timestamp)
        {
            return (new DateTime((timestamp * 10000000) + 621355968000000000)).ToLocalTime();
        }

        public string ConvertToChinese(decimal number)
        {
            var format = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A").Replace("0B0A", "@");
            var simplify = Regex.Replace(format, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var result = Regex.Replace(simplify, ".", match => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空整分角拾佰仟万亿兆京垓秭穰"[match.Value[0] - '-'].ToString());
            return result;
        }

        public static object NullToDBNull<T>(T obj)
        {
            if (obj == null) return DBNull.Value;
            else return obj;
        }
    }
}
