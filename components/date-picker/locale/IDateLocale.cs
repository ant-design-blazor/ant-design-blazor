#nullable enable

namespace AntDesign
{
    public interface IDateLocale
    {
        string DateFormat { get; }

        /// <summary>
        /// The start day of week should be either Monday or Sunday according to the CultureInfo
        /// </summary>
        string[] ShortWeekDays { get; }

        /// <summary>
        /// The difference of days to get the either Monday (6) or Sunday (7)
        /// </summary>
        int FirstDayOfWeekDiff { get; }

        string Year { get; }
        string Month { get; }
        string Week => "Week";
        string Today { get; }
        bool MonthBeforeYear { get; }
        string YearFormat { get; }
        string MonthFormat { get; }
        string Ok { get; }
        string Now { get; }
        string SelectDate { get; }
        string SelectWeek { get; }
        string SelectMonth { get; }
        string SelectQuarter { get; }
        string SelectYear { get; }
        string SelectTime { get; }
        string StartOfDate { get; }
        string EndOfDate { get; }
        string StartOfWeek { get; }
        string EndOfWeek { get; }
        string StartOfMonth { get; }
        string EndOfMonth { get; }
        string StartOfYear { get; }
        string EndOfYear { get; }
        string StartOfQuarter { get; }
        string EndOfQuarter { get; }
    }
}
