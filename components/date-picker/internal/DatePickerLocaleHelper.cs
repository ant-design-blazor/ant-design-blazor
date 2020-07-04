using System.Collections.Generic;
using System.Globalization;

#nullable enable

namespace AntDesign
{
    static class DatePickerLocaleHelper
    {
        static readonly Dictionary<string, IDateLocale> _locales = new Dictionary<string, IDateLocale>();

        public static IDateLocale GetDateLocale(this CultureInfo info)
        {
            if (_locales.ContainsKey(info.Name)) return _locales[info.Name];
            lock (_locales)
            {
                if (!_locales.ContainsKey(info.Name))
                {
                    _locales[info.Name] = GenerateDateLocale(info.Name);
                }
                return _locales[info.Name];
            }
        }

        private static IDateLocale GenerateDateLocale(string name)
        {
            switch (name)
            {
                case "en-US": return new EnUsDateLocale();
                case "zh-CN": return new ZhCnLocale();
                default: return new EnUsDateLocale();
            }
        }
    }

    interface IDateLocale
    {
        string DateFormat { get; }
        /// <summary>
        /// The start day of week should be either Monday or Sunday according to the CultureInfo
        /// </summary>
        string[] ShortWeekDays { get; }
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

    class EnUsDateLocale : IDateLocale
    {
        private string _locale = "en-US";
        public string DateFormat => "yyyy-MM-dd";
        public string[] ShortWeekDays => new string[] { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
        public string Year => "Year";
        public string Month => "Month";
        public string Week => "Week";
        public string Today => "Today";
        public bool MonthBeforeYear => true;
        public string YearFormat => "yyyy";
        public string MonthFormat => "MMM";
        public string Ok => "OK";
        public string Now => "Now";
        public string SelectDate => "Select date";
        public string SelectWeek => "Select week";
        public string SelectMonth => "Select month";
        public string SelectQuarter => "Select quarter";
        public string SelectYear => "Select year";
        public string SelectTime => "Select time";
        public string StartOfDate => "Start date";
        public string EndOfDate => "End date";
        public string StartOfWeek => "Start week";
        public string EndOfWeek => "End week";
        public string StartOfMonth => "Start month";
        public string EndOfMonth => "End month";
        public string StartOfYear => "Start year";
        public string EndOfYear => "End year";
        public string StartOfQuarter => "Start quarter";
        public string EndOfQuarter => "End quarter";

    }

    class ZhCnLocale : IDateLocale
    {
        private string _locale = "zh-CN";
        public string DateFormat => "yyyy年M月d日";
        public string[] ShortWeekDays => new string[] { "一", "二", "三", "四", "五", "六", "日" };
        public string Year => "年";
        public string Month => "月";
        public string Week => "周";
        public string Today => "今天";
        public bool MonthBeforeYear => false;
        public string YearFormat => "yyyy年";
        public string MonthFormat => "M月";
        public string Ok => "确 定";
        public string Now => "此刻";
        public string SelectDate => "请选择日期";
        public string SelectWeek => "请选择周";
        public string SelectMonth => "请选择月份";
        public string SelectQuarter => "请选择季度";
        public string SelectYear => "请选择年份";
        public string SelectTime => "请选择时间";
        public string StartOfDate => "开始日期";
        public string EndOfDate => "结束日期";
        public string StartOfWeek => "开始周";
        public string EndOfWeek => "结束周";
        public string StartOfMonth => "开始月份";
        public string EndOfMonth => "结束月份";
        public string StartOfYear => "开始年份";
        public string EndOfYear => "结束年份";
        public string StartOfQuarter => "开始季度";
        public string EndOfQuarter => "结束季度";
    }
}

