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

        /// <summary>
        /// 用该函数来执行AddYears逻辑, 不会触发System.ArgumentOutOfRangeException异常
        /// AddYears by the function would never throw System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime AddYearsSafely(DateTime currentDate, int value)
        {
            int newYear = currentDate.Year + value;

            if (newYear < DateTime.MinValue.Year)
            {
                value = DateTime.MinValue.Year - currentDate.Year;
            }
            else if (newYear > DateTime.MaxValue.Year)
            {
                value = DateTime.MaxValue.Year - currentDate.Year;
            }

            return currentDate.AddYears(value);
        }

        /// <summary>
        /// 用该函数来执行AddMonths逻辑, 不会触发System.ArgumentOutOfRangeException异常
        /// AddMonths by the function would never throw System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime AddMonthsSafely(DateTime currentDate, int value)
        {
            int newMonth = currentDate.Month + value;

            /* at min date value */
            if (currentDate.Year == DateTime.MinValue.Year && newMonth < DateTime.MinValue.Month)
            {
                value = DateTime.MinValue.Month - currentDate.Month;
            }

            /* at max date value */
            if (currentDate.Year == DateTime.MaxValue.Year && newMonth > DateTime.MaxValue.Month)
            {
                value = DateTime.MaxValue.Month - currentDate.Month;
            }

            return currentDate.AddMonths(value);
        }

        /// <summary>
        /// 用该函数来执行AddDays逻辑, 不会触发System.ArgumentOutOfRangeException异常
        /// AddDays by the function would never throw System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime AddDaysSafely(DateTime currentDate, int value)
        {
            int newDay = currentDate.Day + value;

            /* at min date value */
            if (currentDate.Year == DateTime.MinValue.Year
                && currentDate.Month == DateTime.MinValue.Month
                && newDay < DateTime.MinValue.Day)
            {
                value = DateTime.MinValue.Day - currentDate.Day;
            }

            /* at max date value */
            if (currentDate.Year == DateTime.MaxValue.Year
                && currentDate.Month == DateTime.MaxValue.Month
                && newDay > DateTime.MaxValue.Day)
            {
                value = DateTime.MaxValue.Day - currentDate.Day;
            }

            return currentDate.AddDays(value);
        }
    }
}
