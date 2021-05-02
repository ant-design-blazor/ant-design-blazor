using System;
using System.Globalization;

namespace AntDesign
{
    public static class DateHelper
    {
        private static readonly System.Globalization.Calendar _calendar = CultureInfo.InvariantCulture.Calendar;
        private const int DECADE_YEAR_COUNT = 10;
        private const int QUARTER_MONTH_COUNT = 3;

        public static bool IsSameDate(DateTime? date, DateTime? compareDate)
        {
            if (date is null || compareDate is null)
                return false;
            return date == compareDate;
        }

        public static bool IsSameYear(DateTime? date, DateTime? compareDate)
        {
            if (date is null || compareDate is null)
                return false;
            return date.Value.Year == compareDate.Value.Year;
        }

        public static bool IsSameMonth(DateTime? date, DateTime? compareDate)
        {
            if (date is null || compareDate is null)
                return false;
            return IsSameYear(date, compareDate)
                && date.Value.Month == compareDate.Value.Month;
        }

        public static bool IsSameDay(DateTime? date, DateTime? compareDate)
        {
            if (date is null || compareDate is null)
                return false;
            return IsSameYear(date, compareDate)
                && _calendar.GetDayOfYear(date.Value) == _calendar.GetDayOfYear(compareDate.Value);
        }

        public static bool IsSameWeak(DateTime? date, DateTime? compareDate)
        {
            if (date is null || compareDate is null)
                return false;
            return IsSameYear(date, compareDate)
                && GetWeekOfYear(date.Value) == GetWeekOfYear(compareDate.Value);
        }

        public static bool IsSameQuarter(DateTime? date, DateTime? compareDate)
        {
            if (date is null || compareDate is null)
                return false;
            return IsSameYear(date, compareDate)
                && GetDayOfQuarter(date.Value) == GetDayOfQuarter(compareDate.Value);
        }

        public static string GetDayOfQuarter(DateTime date)
        {
            return $"Q{GetQuarter(date)}";
        }

        public static int GetQuarter(DateTime date)
        {
            int offset = date.Month % QUARTER_MONTH_COUNT > 0 ? 1 : 0;
            return date.Month / QUARTER_MONTH_COUNT + offset;
        }

        public static DateTime GetStartDateOfQuarter(DateTime date)
        {
            int quarter = GetQuarter(date);
            return new DateTime(date.Year, 1 + ((quarter - 1) * QUARTER_MONTH_COUNT), 1);
        }

        public static int GetWeekOfYear(DateTime date)
        {
            return _calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// for example,
        /// when currentDateTime is 2020-01-04 05:34:55 then: 
        ///     the next date shouble be 2030-01-01 00:00:00, it's the start date of next 10 years
        ///     
        /// when currentDateTime is 2023-01-04 05:34:55 then: 
        ///     the next date shouble be 2030-01-01 00:00:00, it's the start date of next 10 years
        ///     
        /// when currentDateTime is 2018-01-04 05:34:55 then: 
        ///     the next date shouble be 2020-01-01 00:00:00, it's the start date of next 10 years
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetNextStartDateOfDecade(DateTime date)
        {
            int year = date.Year / DECADE_YEAR_COUNT * DECADE_YEAR_COUNT;

            if (year < DateTime.MinValue.Year)
            {
                year = DateTime.MinValue.Year;
            }

            return AddYearsSafely(new DateTime(year, 1, 1), DECADE_YEAR_COUNT);
        }

        /// <summary>
        /// for example, when currentDateTime is 2020-01-04 05:34:55 then: 
        ///     the next date shouble be 2021-01-01 00:00:00, it's the start date of next year
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetNextStartDateOfYear(DateTime date)
        {
            return AddYearsSafely(new DateTime(date.Year, 1, 1), 1);
        }

        /// <summary>
        /// for example, when currentDateTime is 2020-01-04 05:34:55 then: 
        ///     the next date shouble be 2020-04-01 00:00:00, it's the start date of the next quarter in 2020
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetNextStartDateOfQuarter(DateTime date)
        {
            var nextQuarterDate = AddMonthsSafely(new DateTime(date.Year, date.Month, 1), QUARTER_MONTH_COUNT);

            return GetStartDateOfQuarter(nextQuarterDate);
        }

        /// <summary>
        /// for example, when currentDateTime is 2020-01-04 05:34:55 then: 
        ///     the next date shouble be 2020-02-01 00:00:00 , it's the start date of next month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetNextStartDateOfMonth(DateTime date)
        {
            return AddMonthsSafely(new DateTime(date.Year, date.Month, 1), 1);
        }

        /// <summary>
        /// for example, when currentDateTime is 2020-01-04 05:34:55 then: 
        ///     the next date shouble be 2021-01-05 00:00:00, it's the start date of next day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetNextStartDateOfDay(DateTime date)
        {
            return AddDaysSafely(new DateTime(date.Year, date.Month, date.Day), 1);
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
