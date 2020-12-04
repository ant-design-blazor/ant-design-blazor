using System;

namespace AntDesign
{
    internal static class DayOfWeekHelper
    {
        private const int DIFF_DEFAULT = 1;
        private const int DIFF_SATURDAY = 1;
        private const int DIFF_FRIDAY = 2;
        private const int DIFF_THURSDAY = 3;
        private const int DIFF_WEDNESDAY = 4;
        private const int DIFF_TUESDAY = 5;
        private const int DIFF_MONDAY = 6;
        private const int DIFF_SUNDAY = 7;

        /// <summary>
        ///     Returns the amount of days that have to be added to the start date to get the correct first day of the week.
        /// </summary>
        /// <param name="firstDayOfWeek">First day of the week as defined in the current locale.</param>
        /// <returns>Diff of days.</returns>
        internal static int GetDiffForDayOfWeek(DayOfWeek firstDayOfWeek)
        {
            switch (firstDayOfWeek)
            {
                case DayOfWeek.Saturday: return DIFF_SATURDAY + (int)firstDayOfWeek;
                case DayOfWeek.Friday: return DIFF_FRIDAY + (int)firstDayOfWeek;
                case DayOfWeek.Thursday: return DIFF_THURSDAY + (int)firstDayOfWeek;
                case DayOfWeek.Wednesday: return DIFF_WEDNESDAY + (int)firstDayOfWeek;
                case DayOfWeek.Tuesday: return DIFF_TUESDAY + (int)firstDayOfWeek;
                case DayOfWeek.Monday: return DIFF_MONDAY + (int)firstDayOfWeek;
                case DayOfWeek.Sunday: return DIFF_SUNDAY + (int)firstDayOfWeek;
                default: return DIFF_DEFAULT;
            }
        }
    }
}
