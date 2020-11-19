namespace AntDesign
{
    internal class DatePickerPlaceholder
    {


        public static string GetPlaceholderByType(string pickerType)
        {
            var placeholder = pickerType switch
            {
                DatePickerType.Date => LocaleProvider.CurrentLocale.DatePicker.Lang.DateSelect,
                DatePickerType.Week => LocaleProvider.CurrentLocale.DatePicker.Lang.WeekSelect,
                DatePickerType.Month => LocaleProvider.CurrentLocale.DatePicker.Lang.MonthSelect,
                DatePickerType.Quarter => LocaleProvider.CurrentLocale.DatePicker.Lang.QuarterSelect,
                DatePickerType.Year => LocaleProvider.CurrentLocale.DatePicker.Lang.YearSelect,
                DatePickerType.Time => LocaleProvider.CurrentLocale.DatePicker.Lang.TimeSelect,
                _ => LocaleProvider.CurrentLocale.DatePicker.Lang.DateSelect,
            };

            return placeholder;
        }

        public static (string, string) GetRangePlaceHolderByType(string pickerType)
        {
            var placeholder = pickerType switch
            {
                DatePickerType.Date => (LocaleProvider.CurrentLocale.DatePicker.Lang.StartDate, LocaleProvider.CurrentLocale.DatePicker.Lang.EndDate),
                DatePickerType.Week => (LocaleProvider.CurrentLocale.DatePicker.Lang.StartWeek, LocaleProvider.CurrentLocale.DatePicker.Lang.EndWeek),
                DatePickerType.Month => (LocaleProvider.CurrentLocale.DatePicker.Lang.StartMonth, LocaleProvider.CurrentLocale.DatePicker.Lang.EndMonth),
                DatePickerType.Year => (LocaleProvider.CurrentLocale.DatePicker.Lang.StartYear, LocaleProvider.CurrentLocale.DatePicker.Lang.EndYear),
                DatePickerType.Time => (LocaleProvider.CurrentLocale.DatePicker.Lang.StartDate, LocaleProvider.CurrentLocale.DatePicker.Lang.EndDate),
                DatePickerType.Quarter => (LocaleProvider.CurrentLocale.DatePicker.Lang.StartQuarter, LocaleProvider.CurrentLocale.DatePicker.Lang.EndQuarter),
                _ => (LocaleProvider.CurrentLocale.DatePicker.Lang.StartDate, LocaleProvider.CurrentLocale.DatePicker.Lang.EndDate),
            };
            return placeholder;
        }
    }
}
