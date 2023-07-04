using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Silmoon.Extension
{
    public static class SpecialConverter
    {
        public static readonly DateTime SqlMinDateTime = DateTime.Parse("1/1/1753 00:00:00");

        public static long GetUnixTimestamp(this DateTime dateTime) => new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        public static DateTime FromUnixTimestamp(long timestamp) => DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;

        public static object IfNullToDBNull<T>(T obj) => obj == null ? DBNull.Value : (object)obj;
    }
}
