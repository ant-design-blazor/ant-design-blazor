using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign;

/**
<summary>
<para>Time to return to top.</para>
</summary>
*/
public partial class FloatButtonBackTop : AntDomComponentBase
{
    [Inject]
    private IDomEventListener DomEventListener { get; set; }

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

    private bool _visible;
    internal bool _hidden = true;
    private double _scrollProgress;

    private string ProgressStyle => ShowProgress
        ? $"--ant-float-btn-progress:{_scrollProgress.ToString(CultureInfo.InvariantCulture)}turn;{Style}"
        : Style;

    protected override async Task OnFirstAfterRenderAsync()
    {
        if (string.IsNullOrWhiteSpace(TargetSelector) || TargetSelector == "window")
            DomEventListener.AddExclusive<JsonElement>("window", "scroll", OnScroll);
        else
            DomEventListener.AddExclusive<JsonElement>(TargetSelector, "scroll", OnScroll);

        if (ShowProgress)
        {
            DomEventListener.AddExclusive<JsonElement>("window", "resize", OnScroll);
        }

        await UpdateScrollState();

        await base.OnFirstAfterRenderAsync();
    }

    private async Task OnButtonClick(MouseEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(TargetSelector) || TargetSelector == "window")
            await JsInvokeAsync<DomRect>(JSInteropConstants.BackTop);
        else
            await JsInvokeAsync<DomRect>(JSInteropConstants.BackTop, TargetSelector);

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(args);
    }

    private async Task OnScroll(JsonElement obj)
    {
        await UpdateScrollState();
    }

    private async Task UpdateScrollState()
    {
        double offset;
        HtmlElement scrollElement;

        if (string.IsNullOrWhiteSpace(TargetSelector) || TargetSelector is "window" or "document")
        {
            var scrollInfo = await JsInvokeAsync<JsonElement>(JSInteropConstants.GetScroll);
            offset = scrollInfo.GetProperty("y").GetDouble();
            scrollElement = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, "html");
        }
        else
        {
            scrollElement = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, TargetSelector);
            offset = (double)scrollElement.ScrollTop;
        }

        var maxScroll = Math.Max((double)scrollElement.ScrollHeight - (double)scrollElement.ClientHeight, 0);
        if (ShowProgress)
        {
            _scrollProgress = maxScroll > 0 ? Math.Clamp(offset / maxScroll, 0, 1) : 0;
        }

        var visible = offset >= VisibilityHeight;

        if (visible == _visible)
        {
            if (visible && _hidden)
            {
                _hidden = false;
                await InvokeAsync(StateHasChanged);
            }
            else if (ShowProgress)
            {
                await InvokeAsync(StateHasChanged);
            }

            return;
        }

        _visible = visible;
        if (_visible)
        {
            _hidden = false;
        }
        else
        {
            await Task.Delay(300);
            if (!_visible)
            {
                _hidden = true;
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    protected override void Dispose(bool disposing)
    {
        DomEventListener?.DisposeExclusive();
        base.Dispose(disposing);
    }
}
