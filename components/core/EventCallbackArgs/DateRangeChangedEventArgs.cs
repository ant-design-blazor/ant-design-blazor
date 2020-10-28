using System;

namespace AntDesign
{
    public class DateRangeChangedEventArgs
    {
        public DateTime?[] Dates { get; set; }
        public string[] DateStrings { get; set; }
    }
}
