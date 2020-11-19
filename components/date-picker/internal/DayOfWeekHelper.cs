using System;
using System.Globalization;

namespace AntDesign
{
    internal static class DayOfWeekHelper
    {
        private const string MONDAY = "Monday";
        private const string TUESDAY = "Tuesday";
        private const string WEDNESDAY = "Wednesday";
        private const string THURSDAY = "Thursday";
        private const string FRIDAY = "Friday";
        private const string SATURDAY = "Saturday";
        private const string SUNDAY = "Sunday";

        internal static int GetDiffForDayOfWeek(DatePickerLocale locale)
        {
            switch (locale.FirstDayOfWeek)
            {
                case SATURDAY: return 1;
                case FRIDAY: return 2;
                case THURSDAY: return 3;
                case WEDNESDAY: return 4;
                case TUESDAY: return 5;
                case MONDAY: return 6;
                case SUNDAY: return 7;
                default: return 0;
            }
        }

        internal static string[] GetShortWeekDays(DatePickerLocale locale, CultureInfo cultureInfo)
        {
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            DayOfWeek firstDayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), locale.FirstDayOfWeek);
            DateTime referenceDay = DateTime.Today;

            if(firstDayOfWeek != currentDay)
            {
                int diff = firstDayOfWeek - currentDay;
                referenceDay = referenceDay.AddDays(diff);
            }

            return new[] { referenceDay.GetTwoLetterCode(cultureInfo),
                           referenceDay.AddDays(1).GetTwoLetterCode(cultureInfo),
                           referenceDay.AddDays(2).GetTwoLetterCode(cultureInfo),
                           referenceDay.AddDays(3).GetTwoLetterCode(cultureInfo),
                           referenceDay.AddDays(4).GetTwoLetterCode(cultureInfo),
                           referenceDay.AddDays(5).GetTwoLetterCode(cultureInfo),
                           referenceDay.AddDays(6).GetTwoLetterCode(cultureInfo),
            };
        }

        private static string GetTwoLetterCode(this DateTime today, CultureInfo cultureInfo)
            => today.ToString("ddd", cultureInfo).Substring(0, 2);
    }
}
