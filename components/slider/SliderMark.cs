using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class SliderMark
    {
        /// <summary>
        /// Number for mark to be on. Must be in the range of the Min and Max of the containing Slider
        /// </summary>
        public double Key { get; }

        /// <summary>
        /// Display content for the mark
        /// </summary>
        public RenderFragment Value { get; }

        /// <summary>
        /// Style for the mark
        /// </summary>
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
