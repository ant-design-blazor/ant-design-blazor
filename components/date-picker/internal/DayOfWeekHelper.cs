using System;

namespace AntDesign
{
    internal static class DayOfWeekHelper
    {
        internal static int GetDiffForDayOfWeek()
        {
            switch (LocaleProvider.CurrentLocale.DatePicker.Lang.FirstDayOfWeek)
            {
                case DayOfWeek.Saturday: return 1;
                case DayOfWeek.Friday: return 2;
                case DayOfWeek.Thursday: return 3;
                case DayOfWeek.Wednesday: return 4;
                case DayOfWeek.Tuesday: return 5;
                case DayOfWeek.Monday: return 6;
                case DayOfWeek.Sunday: return 7;
                default: return 0;
            }
        }

        internal static string[] GetShortWeekDays()
        {
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            DateTime referenceDay = DateTime.Today;

            if(LocaleProvider.CurrentLocale.DatePicker.Lang.FirstDayOfWeek != currentDay)
            {
                int diff = LocaleProvider.CurrentLocale.DatePicker.Lang.FirstDayOfWeek - currentDay;
                referenceDay = referenceDay.AddDays(diff);
            }

            return new[] { referenceDay.GetTwoLetterCode(),
                           referenceDay.AddDays(1).GetTwoLetterCode(),
                           referenceDay.AddDays(2).GetTwoLetterCode(),
                           referenceDay.AddDays(3).GetTwoLetterCode(),
                           referenceDay.AddDays(4).GetTwoLetterCode(),
                           referenceDay.AddDays(5).GetTwoLetterCode(),
                           referenceDay.AddDays(6).GetTwoLetterCode(),
            };
        }

        private static string GetTwoLetterCode(this DateTime today)
            => today.ToString("ddd", LocaleProvider.CurrentLocale.CurrentCulture).Substring(0, 2);
    }
}
