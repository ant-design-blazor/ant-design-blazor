using OneOf;

namespace AntDesign;

/// <summary>
/// Search configuration for <see cref="Select{TItemValue, TItem}.ShowSearch"/>.
/// </summary>
public sealed class SelectShowSearchOptions
{
    /// <summary>
    /// Specifies which option field or fields to search. Multiple fields use OR matching.
    /// </summary>
    public OneOf<string, string[]> OptionFilterProp { get; set; }

    /// <summary>
    /// Whether the option list should be filtered by the search text. Defaults to true.
    /// </summary>
    public bool? FilterOption { get; set; }
}
