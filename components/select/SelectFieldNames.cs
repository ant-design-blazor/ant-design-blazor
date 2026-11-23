namespace AntDesign;

/// <summary>
/// Maps the option fields used by <see cref="Select{TItemValue, TItem}"/>.
/// </summary>
public sealed class SelectFieldNames
{
    /// <summary>The option text field. Defaults to <c>label</c>.</summary>
    public string Label { get; set; } = "label";

    /// <summary>Optional source field for the native option title attribute.</summary>
    public string Title { get; set; }

    /// <summary>The option value field. Defaults to <c>value</c>.</summary>
    public string Value { get; set; } = "value";

    /// <summary>The nested child options field. Defaults to <c>options</c>.</summary>
    public string Options { get; set; } = "options";

    /// <summary>The label field used by an option group. Defaults to <c>label</c>.</summary>
    public string GroupLabel { get; set; } = "label";

    /// <summary>Optional source field for an option group's class name.</summary>
    public string GroupClassName { get; set; }

    /// <summary>Optional source field for an option group's native title attribute.</summary>
    public string GroupTitle { get; set; }
}
