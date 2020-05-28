using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace AntDesign
{
    public class SliderMark
    {
        public double Key { get; }
        public RenderFragment Value { get; }
        public string Style { get; }

        public SliderMark(double key, string value)
        {
            Key = key;
            Value = (b) => b.AddContent(0, value);
        }

        public SliderMark(double key, RenderFragment value, string style)
        {
            Key = key;
            Value = value;
            Style = style;
        }
    }
}
