using System;

namespace AntDesign
{
    public class DateTimeChangedEventArgs<TValue>
    {
        public TValue Date { get; set; }
        public string DateString { get; set; }
    }
}
