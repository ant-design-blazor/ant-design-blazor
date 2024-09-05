// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class DatePickerType : EnumValue<DatePickerType>
    {
        internal const string DATE = "date";
        internal const string WEEK = "week";
        internal const string MONTH = "month";
        internal const string QUARTER = "quarter";
        internal const string YEAR = "year";
        internal const string DECADE = "decade";
        internal const string TIME = "time";

        private DatePickerType(string name, int value) : base(name, value)
        {
        }

        public static readonly DatePickerType Date = new(DATE, 1);
        public static readonly DatePickerType Week = new(WEEK, 2);
        public static readonly DatePickerType Month = new(MONTH, 3);
        public static readonly DatePickerType Quarter = new(QUARTER, 4);
        public static readonly DatePickerType Year = new(YEAR, 5);
        public static readonly DatePickerType Decade = new(DECADE, 6);
        public static readonly DatePickerType Time = new(TIME, 7);
    }
}
