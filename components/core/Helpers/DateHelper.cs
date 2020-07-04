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
            return date
                .AddYears(year != null ? (int)year - date.Year : 0)
                .AddMonths(month != null ? (int)month - date.Month : 0)
                .AddDays(day != null ? (int)day - date.Day : 0)
                .AddHours(hour != null ? (int)hour - date.Hour : 0)
                .AddMinutes(minute != null ? (int)minute - date.Minute : 0)
                .AddSeconds(second != null ? (int)second - date.Second : 0);
        }

        public static DateTime? FormatDateByPicker(DateTime? dateTime, string picker)
        {
            if (dateTime == null)
            {
                return null;
            }

            return FormatDateByPicker((DateTime)dateTime, picker);
        }

        public static DateTime FormatDateByPicker(DateTime dateTime, string picker)
        {
            switch (picker)
            {
                case DatePickerType.Date: return dateTime.Date;
                case DatePickerType.Year: return new DateTime(dateTime.Year, 1, 1);
                case DatePickerType.Month: return new DateTime(dateTime.Year, dateTime.Month, 1);
                case DatePickerType.Quarter: return new DateTime(dateTime.Year, dateTime.Month, 1);
            }

            return dateTime;
        }
    }
}
