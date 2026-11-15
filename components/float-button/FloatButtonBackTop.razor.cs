using System;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign;

/**
<summary>
<para>Time to return to top.</para>
</summary>
*/
public partial class FloatButtonBackTop : AntDomComponentBase
{
    /// <summary>
    /// Time to return to top (ms). This property is ignored when enables reduced motion (`prefers-reduced-motion: reduce`)
    /// </summary>
    /// <default value="450" />
    [Parameter]
    public int Duration { get; set; } = 450;

    /// <summary>
    /// Show the current scroll progress ring around the BackTop button edge
    /// </summary>
    /// <default value="false" />
    [Parameter]
    public bool ShowProgress { get; set; }

    /// <summary>
    /// Specifies the scrollable area dom node
    /// </summary>
    [Parameter]
    public string TargetSelector { get; set; } = "window";

    /// <summary>
    /// The BackTop button will not show until the scroll height reaches this value
    /// </summary>
    /// <default value="400" />
    [Parameter]
    public double VisibilityHeight { get; set; } = 400;

    /// <summary>
    /// A callback function, which can be executed when you click the button
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Set the icon component of button
    /// </summary>
    [Parameter]
    public string Icon { get; set; } = "vertical-align-top";

    /// <summary>
    /// Setting button type
    /// </summary>
    [Parameter]
    public FloatButtonType Type { get; set; } = FloatButtonType.Default;

    /// <summary>
    /// Setting button shape
    /// </summary>
    [Parameter]
    public FloatButtonShape Shape { get; set; } = FloatButtonShape.Circle;

    /// <summary>
    /// The text shown in the tooltip
    /// </summary>
    [Parameter]
    public string Tooltip { get; set; }

    /// <summary>
    /// Whether the button is disabled
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    private string _boundBackTopButtonId;

    private string ProgressStyle => $"{Style};visibility:hidden;opacity:0;pointer-events:none;transition:opacity 200ms linear;";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (RefBack?.Current.Id is string buttonId && buttonId != _boundBackTopButtonId)
        {
            await JsInvokeAsync(JSInteropConstants.BackTopComponentHelper.BindBackTop, RefBack.Current, TargetSelector, Duration, VisibilityHeight, ShowProgress);
            _boundBackTopButtonId = buttonId;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task OnButtonClick(MouseEventArgs args)
    {
        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(args);
    }

    protected override void Dispose(bool disposing)
    {
        if (_boundBackTopButtonId != null && RefBack?.Current is ElementReference button)
        {
            _ = Js.InvokeVoidAsync(JSInteropConstants.BackTopComponentHelper.UnbindBackTop, button);
        }
        base.Dispose(disposing);
    }
}
