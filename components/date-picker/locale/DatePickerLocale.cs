using System;

namespace AntDesign
{
    public class DatePickerLocale
    {
        public DayOfWeek FirstDayOfWeek { get; set; }

        public DateLocale Lang { get; set; } = new DateLocale();

        public TimePickerLocale TimePickerLocale { get; set; } = new TimePickerLocale();
    }

    public class DateLocale
    {
        public string Placeholder { get; set; } = "Select date";
        public string YearPlaceholder { get; set; } = "Select year";
        public string QuarterPlaceholder { get; set; } = "Select quarter";
        public string MonthPlaceholder { get; set; } = "Select month";
        public string WeekPlaceholder { get; set; } = "Select week";
        public string[] RangePlaceholder { get; set; } = new[] { "Start date", "End date" };
        public string[] RangeYearPlaceholder { get; set; } = new[] { "Start year", "End year" };
        public string[] RangeMonthPlaceholder { get; set; } = new[] { "Start month", "End month" };
        public string[] RangeWeekPlaceholder { get; set; } = new[] { "Start week", "End week" };
        public string Locale { get; set; } = "en_US";
        public string Today { get; set; } = "Today";
        public string Now { get; set; } = "Now";
        public string BackToToday { get; set; } = "Back to today";
        public string Ok { get; set; } = "Ok";
        public string Clear { get; set; } = "Clear";
        public string Month { get; set; } = "Month";
        public string Year { get; set; } = "Year";
        public string TimeSelect { get; set; } = "select time";
        public string DateSelect { get; set; } = "select date";
        public string WeekSelect { get; set; } = "Choose a week";
        public string MonthSelect { get; set; } = "Choose a month";
        public string YearSelect { get; set; } = "Choose a year";
        public string DecadeSelect { get; set; } = "Choose a decade";
        public string MonthFormat { get; set; } = "MMM";
        public string DateFormat { get; set; } = "yyyy-MM-dd";
        public string DayFormat { get; set; } = "D";
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
        public bool MonthBeforeYear { get; set; } = true;
        public string PreviousMonth { get; set; } = "Previous month (PageUp)";
        public string NextMonth { get; set; } = "Next month (PageDown)";
        public string PreviousYear { get; set; } = "Last year (Control + left)";
        public string NextYear { get; set; } = "Next year (Control + right)";
        public string PreviousDecade { get; set; } = "Last decade";
        public string NextDecade { get; set; } = "Next decade";
        public string PreviousCentury { get; set; } = "Last century";
        public string NextCentury { get; set; } = "Next century";
        public string YearFormat { get; set; } = "yyyy";
        public string StartDate { get; set; } = "Start date";
        public string StartWeek { get; set; } = "Start week";
        public string StartMonth { get; set; } = "Start month";
        public string StartYear { get; set; } = "Start year";
        public string StartQuarter { get; set; } = "Start quarter";
        public string EndDate { get; set; } = "End date";
        public string EndWeek { get; set; } = "End week";
        public string EndMonth { get; set; } = "End month";
        public string EndYear { get; set; } = "End year";
        public string EndQuarter { get; set; } = "End Quarter";
        public string QuarterSelect { get; set; } = "Select quarter";
        public string Week { get; set; } = "Week";
    }
}
