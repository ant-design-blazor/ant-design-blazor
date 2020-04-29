using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace AntBlazor
{
    public class AntSliderMark
    {
        public double Key { get; }
        public RenderFragment Value { get; }
        public string Style { get; }

        public AntSliderMark(double key, string value)
        {
            Key = key;
            Value = (b) => b.AddContent(0, value);
        }

        public AntSliderMark(double key, RenderFragment value, string style)
        {
            Key = key;
            Value = value;
            Style = style;
        }
    }
}