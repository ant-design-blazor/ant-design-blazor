namespace AntDesign;

/// <summary>
/// Describes the row or vertical position that a <see cref="Table{TItem}"/> should scroll to.
/// </summary>
public sealed class TableScrollToOptions
{
    /// <summary>Zero-based index of the target row in the current table data.</summary>
    public int? Index { get; set; }

    /// <summary>Value returned by the table's <c>RowKey</c> selector for the target row.</summary>
    public object Key { get; set; }

    /// <summary>Absolute vertical scroll offset. Cannot be combined with <see cref="Offset"/>.</summary>
    public double? Top { get; set; }

    /// <summary>Aligns the target row's top edge to this offset in the scroll viewport.</summary>
    public double? Offset { get; set; }

    /// <summary>Target alignment: <c>start</c>, <c>center</c>, <c>end</c>, or <c>nearest</c>.</summary>
    public string Align { get; set; } = "nearest";
}
