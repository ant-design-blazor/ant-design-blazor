using System;
using System.Globalization;

namespace AntBlazor
{
    public class DateHelper
    {
        private static Calendar calendar = CultureInfo.InvariantCulture.Calendar;

        public static bool IsSameDate(DateTime date, DateTime compareDate)
        {
            return date == compareDate;
        }

        public static bool IsSameYear(DateTime date, DateTime compareDate)
        {
            return date.Year == compareDate.Year;
        }

        public static bool IsSameMonth(DateTime date, DateTime compareDate)
        {
            return IsSameYear(date, compareDate)
                && date.Month == compareDate.Month;
        }

        public static bool IsSameDay(DateTime date, DateTime compareDate)
        {
            return calendar.GetDayOfYear(date) == calendar.GetDayOfYear(compareDate);
        }

        public static bool IsSameWeak(DateTime date, DateTime compareDate)
        {
            return GetWeekOfYear(date) == GetWeekOfYear(compareDate);
        }

        public static bool IsSameQuarter(DateTime date, DateTime compareDate)
        {
            return GetDayOfQuarter(date) == GetDayOfQuarter(compareDate);
        }

        public static string GetDayOfQuarter(DateTime date)
        {
            int offset = date.Month % 3 > 0 ? 1 : 0;
            int quarter = date.Month / 3 + offset;

            return $"Q{quarter}";
        }

        public static int GetWeekOfYear(DateTime date)
        {
            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}
