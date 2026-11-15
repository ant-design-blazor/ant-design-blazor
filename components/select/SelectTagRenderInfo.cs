using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign;

/// <summary>Context passed to <see cref="SelectBase{TItemValue, TItem}.TagRender"/>.</summary>
/// <typeparam name="TItemValue">The option value type.</typeparam>
/// <typeparam name="TItem">The option item type.</typeparam>
public sealed class SelectTagRenderInfo<TItemValue, TItem>
{
    /// <summary>The original option item.</summary>
    public TItem Item { get; init; }

    /// <summary>The selected option value.</summary>
    public TItemValue Value { get; init; }

    /// <summary>The selected option label.</summary>
    public string Label { get; init; }

    /// <summary>Whether the tag can be removed.</summary>
    public bool Closable { get; init; }

    /// <summary>Removes the selected option when invoked.</summary>
    public EventCallback<MouseEventArgs> OnClose { get; init; }
}
