// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class CalendarMode : EnumValue<CalendarMode>
    {
        internal const string MONTH = "month";
        internal const string YEAR = "year";

        public static CalendarMode Month = new CalendarMode(MONTH, 0);
        public static CalendarMode Year = new CalendarMode(YEAR, 1);

        public CalendarMode(string name, int value) : base(name, value)
        {
        }
    }
}
