using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    internal class AntDatePickerPlaceholder
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
                AntDatePickerType.Date => AntDatePickerPlaceholder.Date,
                AntDatePickerType.Week => AntDatePickerPlaceholder.Week,
                AntDatePickerType.Month => AntDatePickerPlaceholder.Month,
                AntDatePickerType.Quarter => AntDatePickerPlaceholder.Quarter,
                AntDatePickerType.Year => AntDatePickerPlaceholder.Year,
                AntDatePickerType.Time => AntDatePickerPlaceholder.Time,
                _ => "",
            };

            return placeholder;
        }
    }
}
