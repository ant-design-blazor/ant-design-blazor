using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign;

public partial class FloatButtonGroup : AntDomComponentBase
{
    /// <summary>Semantic class names for the group, items, list, and trigger.</summary>
    [Parameter]
    public IDictionary<string, string> ClassNames { get; set; }

    /// <summary>Semantic inline styles for the group, items, list, and trigger.</summary>
    [Parameter]
    public IDictionary<string, string> Styles { get; set; }

    internal string GetSemanticClass(string name) =>
        ClassNames != null && ClassNames.TryGetValue(name, out var value) ? value : string.Empty;

    internal string GetSemanticStyle(string name) =>
        Styles != null && Styles.TryGetValue(name, out var value) ? value : string.Empty;

    /// <summary>
    /// Setting button shape of children
    /// </summary>
    /// <default value="circle" />
    [Parameter]
    public FloatButtonShape Shape { get; set; } = FloatButtonShape.Circle;

    /// <summary>
    /// Customize menu animation placement
    /// </summary>
    /// <default value="top" />
    [Parameter]
    public FloatButtonGroupPlacement Placement { get; set; } = FloatButtonGroupPlacement.Top;

    /// <summary>
    /// Which action can trigger menu open/close
    /// </summary>
    [Parameter]
    public FloatButtonGroupTrigger Trigger { get; set; } = FloatButtonGroupTrigger.None;

    /// <summary>
    /// Whether the menu is visible or not, use it with trigger
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Customize close button icon
    /// </summary>
    [Parameter]
    public string CloseIcon { get; set; } = "close";

    /// <summary>
    /// Callback executed when active menu is changed, use it with trigger
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnOpenChange { get; set; }

    /// <summary>
    /// Set the handler to handle `click` event (only work in `Menu mode`)
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Set the icon component of button
    /// </summary>
    [Parameter]
    public string Icon { get; set; } = "file-text";

    /// <summary>
    /// Setting button type
    /// </summary>
    [Parameter]
    public FloatButtonType Type { get; set; } = FloatButtonType.Default;

    /// <summary>
    /// Whether the button is disabled
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Content of the group.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Content for the trigger button.
    /// </summary>
    [Parameter]
    public RenderFragment TriggerContent { get; set; }

    /// <summary>
    /// Sets the value of the aria-label attribute
    /// </summary>
    [Parameter]
    public string AriaLabel { get; set; }

    internal bool IsMenuMode => Trigger != FloatButtonGroupTrigger.None;

    internal bool IsOpen => IsMenuMode && Open;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetClass();
    }

    private void SetClass()
    {
        const string prefix = "ant-float-btn-group";
        ClassMapper
            .Add(prefix)
            .If("ant-float-btn-group-individual", () => Shape == FloatButtonShape.Circle)
            .If($"{prefix}-{Trigger.ToString().ToLowerInvariant()}", () => IsMenuMode)
            .If($"{prefix}-menu-mode", () => IsMenuMode)
            .Add($"{prefix}-{Placement.ToString().ToLowerInvariant()}")
            .If($"{prefix}-open", () => IsOpen)
            .If("ant-float-btn-group-rtl", () => RTL);
    }

    internal bool IsIndividual => Shape == FloatButtonShape.Circle;

    private async Task OnTriggerClick(MouseEventArgs args)
    {
        if (Disabled)
            return;

        if (Trigger == FloatButtonGroupTrigger.Click)
            await SetOpen(!Open);

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(args);
    }

    private async Task OnMouseEnter()
    {
        if (Trigger == FloatButtonGroupTrigger.Hover)
            await SetOpen(true);
    }

    private async Task OnMouseLeave()
    {
        if (Trigger == FloatButtonGroupTrigger.Hover)
            await SetOpen(false);
    }

    private async Task SetOpen(bool value)
    {
        if (Open == value)
            return;

        Open = value;
        await OpenChanged.InvokeAsync(value);
        await OnOpenChange.InvokeAsync(value);
        await InvokeAsync(StateHasChanged);
    }
}
