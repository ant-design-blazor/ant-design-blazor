using System.Collections.Generic;
using System.Globalization;

#nullable enable

namespace AntDesign
{
    internal static class DatePickerLocaleHelper
    {
        private static readonly Dictionary<string, IDateLocale> _locales = new Dictionary<string, IDateLocale>();

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
                case "en-GB": return new EnGbDateLocale();
                case "de-DE":
                case "de-AT":
                case "de-CH": return new DeChDateLocale();
                case "zh-CN": return new ZhCnLocale();
                default: return new EnUsDateLocale();
            }
        }
    }

    internal class EnUsDateLocale : IDateLocale
    {
        private string _locale = "en-US";
        public string DateFormat => "yyyy-MM-dd";
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
    internal class EnGbDateLocale : IDateLocale
    {
        private string _locale = "en-GB";
        public string DateFormat => "dd.MM.yyyy";
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

    internal class DeChDateLocale : IDateLocale
    {
        private string _locale = "de-CH";
        public string DateFormat => "dd.MM.yyyy";
        public string Year => "Jahr";
        public string Month => "Monat";
        public string Week => "Woche";
        public string Today => "Heute";
        public bool MonthBeforeYear => true;
        public string YearFormat => "yyyy";
        public string MonthFormat => "MMM";
        public string Ok => "OK";
        public string Now => "Jetzt";
        public string SelectDate => "Datum wählen";
        public string SelectWeek => "Woche wählen";
        public string SelectMonth => "Monat wählen";
        public string SelectQuarter => "Quartal wählen";
        public string SelectYear => "Jahr wählen";
        public string SelectTime => "Zeit wählen";
        public string StartOfDate => "Startdatum";
        public string EndOfDate => "Enddatum";
        public string StartOfWeek => "Startwoche";
        public string EndOfWeek => "Schlusswoche";
        public string StartOfMonth => "Startmonat";
        public string EndOfMonth => "Schlussmonat";
        public string StartOfYear => "Startjahr";
        public string EndOfYear => "Schlussjahr";
        public string StartOfQuarter => "Startquartal";
        public string EndOfQuarter => "Schlussquartal";
    }

    internal class ZhCnLocale : IDateLocale
    {
        private string _locale = "zh-CN";
        public string DateFormat => "yyyy年M月d日";
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
