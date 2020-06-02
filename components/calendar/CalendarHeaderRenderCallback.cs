using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class CalendarHeaderRenderArgs
    {
        public DateTime Value { get; set; }
        public string Type { get; set; }
        public EventCallback<DateTime> OnChange { get; set; }
        public EventCallback<string> OnTypeChange { get; set; }
    }
}
