using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign;

public class FloatButtonBadge
{
    /// <summary>
    /// Number to show in badge
    /// </summary>
    [Parameter]
    public int? Count { get; set; }

    /// <summary>
    /// Whether to display a dot instead of count
    /// </summary>
    [Parameter]
    public bool Dot { get; set; }

    /// <summary>
    /// Set Badge color
    /// </summary>
    [Parameter]
    public OneOf<BadgeColor?, string> Color { get; set; }

    /// <summary>
    /// Max count to show
    /// </summary>
    [Parameter]
    public int OverflowCount { get; set; } = 99;
}
