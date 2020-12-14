using System;
using System.Collections.Generic;

namespace DotNetCoreSqlDb.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static bool IsWeekend(this DateTime dt)
        {
            return dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsDayOff(this DateTime dt, List<DateTime> holidays)
        {
            return dt.IsWeekend() || holidays.Contains(dt.Date);
        }
    }
}
