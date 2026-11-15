namespace AntDesign;

/// <summary>Options for Table sort direction tooltips.</summary>
public sealed class TableSorterTooltipOptions
{
    /// <summary>Choose whether the tooltip targets the full header or only the sorter icon.</summary>
    public string Target { get; set; } = "full-header";
}
