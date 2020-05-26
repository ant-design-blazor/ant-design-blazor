using System;
using System.Globalization;

namespace AntDesign
{
    public static class DateHelper
    {
        private static readonly System.Globalization.Calendar _calendar = CultureInfo.InvariantCulture.Calendar;

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
            return IsSameYear(date, compareDate)
                && _calendar.GetDayOfYear(date) == _calendar.GetDayOfYear(compareDate);
        }

        public static bool IsSameWeak(DateTime date, DateTime compareDate)
        {
            return IsSameYear(date, compareDate)
                && GetWeekOfYear(date) == GetWeekOfYear(compareDate);
        }

        public static bool IsSameQuarter(DateTime date, DateTime compareDate)
        {
            return IsSameYear(date, compareDate)
                && GetDayOfQuarter(date) == GetDayOfQuarter(compareDate);
        }

        public static string GetDayOfQuarter(DateTime date)
        {
            int offset = date.Month % 3 > 0 ? 1 : 0;
            int quarter = date.Month / 3 + offset;

            return $"Q{quarter}";
        }

        public static int GetWeekOfYear(DateTime date)
        {
            return _calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static DateTime CombineNewDate(
            DateTime date,
            int? year = null,
            int? month = null,
            int? day = null,
            int? hour = null,
            int? minute = null,
            int? second = null)
        {
            return new DateTime(
                year ?? date.Year,
                month ?? date.Month,
                day ?? date.Day,
                hour ?? date.Hour,
                minute ?? date.Minute,
                second ?? date.Second
            );
        }

    }
}
