using System.Globalization;

namespace AntDesign
{
    internal class DatePickerPlaceholder
    {


        public static string GetPlaceholderByType(string pickerType, CultureInfo info)
        {
            var placeholder = pickerType switch
            {
                DatePickerType.Date => info.GetDateLocale().SelectDate,
                DatePickerType.Week => info.GetDateLocale().SelectWeek,
                DatePickerType.Month => info.GetDateLocale().SelectMonth,
                DatePickerType.Quarter => info.GetDateLocale().SelectQuarter,
                DatePickerType.Year => info.GetDateLocale().SelectYear,
                DatePickerType.Time => info.GetDateLocale().SelectTime,
                _ => info.GetDateLocale().SelectDate,
            };

            return placeholder;
        }

        public static (string, string) GetRangePlaceHolderByType(string pickerType, CultureInfo info)
        {
            var placeholder = pickerType switch
            {
                DatePickerType.Date => (info.GetDateLocale().StartOfDate, info.GetDateLocale().EndOfDate),
                DatePickerType.Week => (info.GetDateLocale().StartOfWeek, info.GetDateLocale().EndOfWeek),
                DatePickerType.Month => (info.GetDateLocale().StartOfMonth, info.GetDateLocale().EndOfMonth),
                DatePickerType.Year => (info.GetDateLocale().StartOfYear, info.GetDateLocale().EndOfYear),
                DatePickerType.Time => (info.GetDateLocale().StartOfDate, info.GetDateLocale().EndOfDate),
                DatePickerType.Quarter => (info.GetDateLocale().StartOfQuarter, info.GetDateLocale().EndOfQuarter),
                _ => (info.GetDateLocale().StartOfDate, info.GetDateLocale().EndOfDate),
            };
            return placeholder;
        }
    }
}
