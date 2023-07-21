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
            var timeSpan = DateTime.Now - dateTime;
            if (dateTime < DateTime.Now)
            {
                if (timeSpan.TotalDays > 1)
                {
                    return timeSpan.Days + "天前";
                }
                else if (timeSpan.TotalHours > 1)
                {
                    return timeSpan.Hours + "小时前";
                }
                else if (timeSpan.TotalMinutes > 1)
                {
                    return timeSpan.Minutes + "分钟前";
                }
                else
                {
                    return "刚刚";
                }
            }
            else
            {
                if (Math.Abs(timeSpan.TotalDays) > 1)
                {
                    return Math.Abs(timeSpan.Days) + "天后";
                }
                else if (Math.Abs(timeSpan.TotalHours) > 1)
                {
                    return Math.Abs(timeSpan.Hours) + "小时后";
                }
                else if (Math.Abs(timeSpan.TotalMinutes) > 1)
                {
                    return Math.Abs(timeSpan.Minutes) + "分钟后";
                }
                else
                {
                    return "稍后";
                }
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
    }
}
