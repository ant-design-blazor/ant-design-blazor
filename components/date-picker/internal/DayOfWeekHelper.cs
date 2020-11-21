using System;
using System.Globalization;

namespace AntDesign
{
    internal static class DayOfWeekHelper
    {
        internal static int GetDiffForDayOfWeek(DatePickerLocale locale)
        {
            switch (locale.FirstDayOfWeek)
            {
                case DayOfWeek.Saturday: return 1 + (int)locale.FirstDayOfWeek;
                case DayOfWeek.Friday: return 2 + (int)locale.FirstDayOfWeek;
                case DayOfWeek.Thursday: return 3 + (int)locale.FirstDayOfWeek;
                case DayOfWeek.Wednesday: return 4 + (int)locale.FirstDayOfWeek;
                case DayOfWeek.Tuesday: return 5 + (int)locale.FirstDayOfWeek;
                case DayOfWeek.Monday: return 6 + (int)locale.FirstDayOfWeek;
                case DayOfWeek.Sunday: return 7 + (int)locale.FirstDayOfWeek;
                default: return 0;
            }
        }

        internal static string[] GetShortWeekDays(DatePickerLocale locale, CultureInfo cultureInfo)
        {
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            DateTime referenceDay = DateTime.Today;

            if(locale.FirstDayOfWeek != currentDay)
            {
                int diff = locale.FirstDayOfWeek - currentDay;
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
