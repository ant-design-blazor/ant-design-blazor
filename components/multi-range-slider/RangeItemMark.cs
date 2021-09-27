using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class RangeItemMark
    {
        public double Key { get; }
        public RenderFragment Value { get; }
        public string Style { get; }

        public RangeItemMark(double key, string value, string style = "")
        {
            Key = key;
            Value = (b) => b.AddContent(0, value);
            Style = style;
        }

        public RangeItemMark(double key, RenderFragment value, string style)
        {
            Key = key;
            Value = value;
            Style = style;
        }
    }
}
