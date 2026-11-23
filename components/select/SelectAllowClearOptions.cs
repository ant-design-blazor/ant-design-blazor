using Microsoft.AspNetCore.Components;

namespace AntDesign;

/// <summary>
/// Configuration for the Select clear button.
/// </summary>
public sealed class SelectAllowClearOptions
{
    /// <summary>Custom clear icon.</summary>
    public RenderFragment ClearIcon { get; set; }

    /// <summary>Disables the clear button when true.</summary>
    public bool? Disabled { get; set; }

    /// <summary>Accessible name of the clear button.</summary>
    public string Label { get; set; }

    /// <summary>Optional explicit tab order of the clear button. The native button is focusable by default.</summary>
    public int? TabIndex { get; set; }
}
