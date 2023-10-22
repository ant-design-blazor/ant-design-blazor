using System;

namespace AntDesign
{
    public class DateRangeChangedEventArgs<TValue>
    {
        public TValue Dates { get; set; }
        public string[] DateStrings { get; set; }
    }
}
