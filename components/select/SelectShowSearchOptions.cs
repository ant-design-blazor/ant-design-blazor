using OneOf;
using Microsoft.AspNetCore.Components;
using System;
using AntDesign.Select.Internal;

namespace AntDesign;

/// <summary>
/// Search configuration for <see cref="Select{TItemValue, TItem}.ShowSearch"/>.
/// </summary>
public class SelectShowSearchOptions
{
    /// <summary>
    /// Whether to clear the search after an option is selected in multiple or tags mode.
    /// </summary>
    public bool? AutoClearSearchValue { get; set; }

    /// <summary>
    /// Specifies which option field or fields to search. Multiple fields use OR matching.
    /// </summary>
    public OneOf<string, string[]> OptionFilterProp { get; set; }

    /// <summary>
    /// Whether the option list should be filtered by the search text. Defaults to true.
    /// </summary>
    public bool? FilterOption { get; set; }

    /// <summary>
    /// Controlled search text. Use <see cref="OnSearch"/> to observe changes.
    /// </summary>
    public string SearchValue { get; set; }

    /// <summary>
    /// Called when the search input changes.
    /// </summary>
    public Action<string> OnSearch { get; set; }

    /// <summary>
    /// Custom icon displayed in the suffix while searching.
    /// </summary>
    public RenderFragment SearchIcon { get; set; }
}

/// <summary>
/// Strongly typed search configuration for Select, including a typed filter sort callback.
/// </summary>
/// <typeparam name="TItemValue">The selected value type.</typeparam>
/// <typeparam name="TItem">The source option type.</typeparam>
public sealed class SelectShowSearchOptions<TItemValue, TItem> : SelectShowSearchOptions
{
    /// <summary>
    /// Whether options are filtered, or a predicate matching React's
    /// <c>filterOption(inputValue, option)</c>. The option context exposes its
    /// value, label, and original source item.
    /// </summary>
    public new Func<string, SelectOptionItem<TItemValue, TItem>, bool> FilterOption { get; set; }

    /// <summary>
    /// Enables or disables filtering when using the typed search configuration.
    /// The <see cref="FilterOption"/> predicate takes precedence when set.
    /// </summary>
    public bool? FilterOptionEnabled { get; set; }

    /// <summary>
    /// Sorts filtered options. The third argument is the current search text.
    /// </summary>
    public Func<SelectOptionItem<TItemValue, TItem>, SelectOptionItem<TItemValue, TItem>, string, int> FilterSort { get; set; }
}
