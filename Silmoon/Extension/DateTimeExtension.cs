using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class DateTimeExtension
    {
        public static long GetUnixTimestamp(this DateTime datetime) => SpecialConverter.GetUnixTimestamp(datetime);
        public static string ToChineseCharacter(this DateTime dateTime, bool OnlyDate = false) => dateTime.ToString(OnlyDate ? "yyyy年MM月dd日" : "yyyy年MM月dd日 HH时mm分ss秒");
        public static string ToChineseFormat(this DateTime dateTime, bool OnlyDate = false) => dateTime.ToString(OnlyDate ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss");
        public static string GetDescription(this DateTime dateTime)
        {
            var now = DateTime.Now;
            var timeSpan = now - dateTime;

            // 判断是过去的时间还是未来的时间
            bool isPast = timeSpan.TotalMilliseconds > 0;

            timeSpan = isPast ? timeSpan : dateTime - now;

            if (timeSpan.TotalDays >= 365)
            {
                var years = (int)(timeSpan.TotalDays / 365);
                return isPast ? $"{years}年前" : $"{years}年后";
            }
            else if (timeSpan.TotalDays >= 30)
            {
                var months = (int)(timeSpan.TotalDays / 30);
                return isPast ? $"{months}个月前" : $"{months}个月后";
            }
            else if (timeSpan.TotalDays >= 7)
            {
                var weeks = (int)(timeSpan.TotalDays / 7);
                return isPast ? $"{weeks}周前" : $"{weeks}周后";
            }
            else if (timeSpan.TotalDays >= 1)
            {
                return isPast ? $"{timeSpan.Days}天前" : $"{timeSpan.Days}天后";
            }
            else if (timeSpan.TotalHours >= 1)
            {
                return isPast ? $"{timeSpan.Hours}小时前" : $"{timeSpan.Hours}小时后";
            }
            else if (timeSpan.TotalMinutes >= 1)
            {
                return isPast ? $"{timeSpan.Minutes}分钟前" : $"{timeSpan.Minutes}分钟后";
            }
            else if (timeSpan.TotalSeconds > 5)
            {
                return isPast ? $"{timeSpan.Seconds}秒前" : $"{timeSpan.Seconds}秒后";
            }
            else
            {
                return isPast ? "刚刚" : "马上";
            }
        }
        public static DateTime SixCharToDate(this string yyyyMMdd)
        {
            return yyyyMMdd.Length != 6
                ? throw new Exception("日期格式不正确")
                : DateTime.ParseExact(yyyyMMdd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
        }
        public static bool InRange(this DateTime dateTime, DateTime comparisonDateTime, TimeSpan timeSpan, bool before = true, bool after = true)
        {
            if (before && after)
            {
                var r1 = dateTime - comparisonDateTime;
                var r2 = comparisonDateTime - dateTime;
                return (r1 <= timeSpan) && (r2 <= timeSpan);
            }
            else if (before)
            {
                var r = dateTime - comparisonDateTime;
                return r <= timeSpan && comparisonDateTime <= dateTime;
            }
            else if (after)
            {
                var r = comparisonDateTime - dateTime;
                return r <= timeSpan && comparisonDateTime >= dateTime;
            }
            else
            {
                return false;
            }
        }
        public static byte[] GetBytes(this DateTime dateTime)
        {
            //convert datetime to byte array
            return BitConverter.GetBytes(dateTime.ToBinary());
        }
        public static DateTime ToDateTime(this byte[] bytes)
        {
            //convert byte array to datetime
            return DateTime.FromBinary(BitConverter.ToInt64(bytes, 0));
        }
        /// <summary>
        /// 将某个时区的本地时间转换为当前时区的本地时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="TimeZoneOffsetHours">原时区本地时间的时区小时数</param>
        /// <param name="RequireDateTimeKind">转换为UTC时间还是本地时区时间</param>
        /// <returns>当前时区的时间</returns>
        public static DateTime LocalToCurrentLocal(this DateTime dateTime, int TimeZoneOffsetHours, DateTimeKind RequireDateTimeKind = DateTimeKind.Local)
        {
            var result = DateTime.SpecifyKind(dateTime.AddHours(TimeZoneOffsetHours * -1), DateTimeKind.Utc);
            if (RequireDateTimeKind == DateTimeKind.Local)
                return result.ToLocalTime();
            else return result;
        }
    }
}
