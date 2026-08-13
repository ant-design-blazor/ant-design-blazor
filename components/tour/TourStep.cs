using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public sealed class TourStep
    {
        /// <summary>
        /// Target element reference or CSS selector (for example <c>#save-button</c>).
        /// <see cref="TargetSelector"/> can also be used when an explicit selector property is preferred.
        /// </summary>
        public object Target { get; set; }
        /// <summary>
        /// CSS selector of the target element, for example <c>#save-button</c>.
        /// When specified, this takes precedence over <see cref="Target" />.
        /// </summary>
        public string TargetSelector { get; set; }
        /// <summary>
        /// Optional cover content displayed above the title.
        /// </summary>
        public RenderFragment Cover { get; set; }
        /// <summary>
        /// Step title.
        /// </summary>
        public RenderFragment Title { get; set; }
        /// <summary>
        /// Step description.
        /// </summary>
        public RenderFragment Description { get; set; }
        /// <summary>
        /// Placement of the panel relative to the target.
        /// </summary>
        public Placement Placement { get; set; } = Placement.Bottom;
        /// <summary>
        /// Overrides <see cref="Tour.Mask"/> for this step.
        /// </summary>
        public bool? Mask { get; set; }
        /// <summary>
        /// Overrides the mask style for this step.
        /// </summary>
        public string MaskStyle { get; set; }
        /// <summary>
        /// Overrides the mask color for this step.
        /// </summary>
        public string MaskColor { get; set; }
        /// <summary>
        /// Whether the close button is shown for this step.
        /// </summary>
        public bool? Closable { get; set; }
        /// <summary>
        /// Overrides the tour visual type for this step.
        /// </summary>
        public TourType? Type { get; set; }
        /// <summary>
        /// Text of the next button.
        /// </summary>
        public string NextText { get; set; }
        /// <summary>
        /// Text of the previous button.
        /// </summary>
        public string PreviousText { get; set; }
        /// <summary>
        /// Text of the finish button.
        /// </summary>
        public string FinishText { get; set; }
    }
}
