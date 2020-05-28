using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    internal class DatePickerPlaceholder
    {
        public const string Date = "请选择日期";
        public const string Week = "请选择周";
        public const string Month = "请选择月份";
        public const string Quarter = "请选择季度";
        public const string Year = "请选择年份";
        public const string Time = "请选择时间";

        public static string GetPlaceholderByType(string pickerType)
        {
            var placeholder = pickerType switch
            {
                DatePickerType.Date => DatePickerPlaceholder.Date,
                DatePickerType.Week => DatePickerPlaceholder.Week,
                DatePickerType.Month => DatePickerPlaceholder.Month,
                DatePickerType.Quarter => DatePickerPlaceholder.Quarter,
                DatePickerType.Year => DatePickerPlaceholder.Year,
                DatePickerType.Time => DatePickerPlaceholder.Time,
                _ => "",
            };

            return placeholder;
        }
    }
}
