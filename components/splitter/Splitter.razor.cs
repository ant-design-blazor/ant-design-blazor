// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign;

public partial class Splitter<TSize> : AntDomComponentBase where TSize : struct
{
    /// <summary>
    /// The left or top pane in the SplitContainer.
    /// </summary>
    [Parameter]
    public RenderFragment FirstPane { get; set; }

    /// <summary>
    /// The right or bottom pane in the SplitContainer.
    /// </summary>
    [Parameter]
    public RenderFragment SecondPane { get; set; }

    /// <summary>
    /// Determines the minimum distance of pixels of the splitter from the left or the top edge of first pane.
    /// </summary>
    [Parameter]
    public TSize FirstPaneMinSize { get; set; }

    /// <summary>
    /// Determines pixel distance of the splitter from the left or top edge.<br/>
    /// You can specify the pane size to only either the <see cref="FirstPaneSize"/> or the <see cref="SecondPaneSize"/> parameter.
    /// If you specify both the <see cref="FirstPaneSize"/> or the <see cref="SecondPaneSize"/> parameters, then the splitter won't work.
    /// </summary>
    [Parameter]
    public TSize FirstPaneSize { get; set; }

    /// <summary>
    /// A callback that will be invoked when the size of the first pane is changed.
    /// </summary>
    [Parameter]
    public EventCallback<TSize> FirstPaneSizeChanged { get; set; }

    /// <summary>
    /// Determines the minimum distance of pixels of the splitter from the right or the bottom edge of second pane.
    /// </summary>
    [Parameter]
    public TSize SecondPaneMinSize { get; set; }

    /// <summary>
    /// Determines pixel distance of the splitter from the right or bottom edge.<br/>
    /// You can specify the pane size to only either the <see cref="FirstPaneSize"/> or the <see cref="SecondPaneSize"/> parameter.
    /// If you specify both the <see cref="FirstPaneSize"/> or the <see cref="SecondPaneSize"/> parameters, then the splitter won't work.
    /// </summary>
    [Parameter]
    public TSize SecondPaneSize { get; set; }

    /// <summary>
    /// A callback that will be invoked when the size of the second pane is changed.
    /// </summary>
    [Parameter]
    public EventCallback<TSize> SecondPaneSizeChanged { get; set; }

    [Parameter]
    public SplitterLayout Layout { get; set; } = SplitterLayout.Horizontal;

    /// <summary>
    /// Determines the unit of the pane size. (Default: <see cref="UnitOfPaneSize.Pixel"/>)
    /// </summary>
    [Parameter]
    public UnitOfPaneSize UnitOfPaneSize { get; set; } = UnitOfPaneSize.Pixel;

    private DotNetObjectReference<Splitter<TSize>> _thisRef;

    private ElementReference _containerElementRef;

    private IJSObjectReference _handler;

    /// <summary>
    /// Determines if the spliter is vertical or horizontal.
    /// </summary>
    private bool Vertical => Layout == SplitterLayout.Horizontal;

    private string _prefixCls = "ant-splitter";

    /// <summary>
    /// Represents a component consisting of a movable bar that divides a container's display area into two resizable panes.
    /// </summary>
    [DynamicDependency(nameof(UpdateSize))]
    public Splitter()
    {
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ClassMapper.Add($"{_prefixCls}")
            .If(" ant-splitter-horizontal", () => Layout == SplitterLayout.Horizontal);
    }

    internal string GetPaneStyle(TSize? paneSize, TSize? minPaneSize)
    {
        var styleKey = Vertical ? "width" : "height";
        var format = this.UnitOfPaneSize == UnitOfPaneSize.Pixel ?
            "{0}px;" :
            "calc({0:0.###}% - calc(var(--splitter-bar-size) / 2));";

        static string GetSizeText(string prefix, string styleKey, string format, TSize? size, string defaultValue)
        {
            return size != null ? $"{prefix}{styleKey}:{string.Format(format, size)}" : defaultValue;
        }
        ;

        return GetSizeText("min-", styleKey, format, minPaneSize, "") + GetSizeText("", styleKey, format, paneSize, "flex:1;");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this._thisRef = DotNetObjectReference.Create(this);
            this._handler = await JsInvokeAsync<IJSObjectReference>(JSInteropConstants.SplitterHelper.Attach, this._thisRef, this._containerElementRef);
        }
    }

    [JSInvokable]
    public async Task UpdateSize(bool isFirstPane, double size)
    {
        TSize nextSize = THelper.ChangeType<TSize>(size);
        var eventCallback = isFirstPane ? this.FirstPaneSizeChanged : this.SecondPaneSizeChanged;
        await eventCallback.InvokeAsync(nextSize);
    }

    public async ValueTask DisposeAsync()
    {
        if (this._handler != null)
        {
            try
            {
                await this._handler.InvokeVoidAsync("dispose");
                await this._handler.DisposeAsync();
            }
#if NET6_0_OR_GREATER
            catch (JSDisconnectedException) { }
#endif
            catch { }
        }

        this._thisRef?.Dispose();
    }

}