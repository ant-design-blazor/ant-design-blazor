// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace AntDesign;

public partial class Splitter : AntDomComponentBase
{
    /// <summary>
    /// The left or top pane in the SplitContainer.
    /// </summary>
    // [Parameter]
    public RenderFragment FirstPane { get; set; }

    /// <summary>
    /// The right or bottom pane in the SplitContainer.
    /// </summary>
    // [Parameter]
    public RenderFragment SecondPane { get; set; }

    /// <summary>
    /// Determines the minimum distance of pixels of the splitter from the left or the top edge of first pane.
    /// </summary>
    // [Parameter]
    public string FirstPaneMinSize { get; set; }

    /// <summary>
    /// Determines pixel distance of the splitter from the left or top edge.<br/>
    /// You can specify the pane size to only either the <see cref="FirstPaneSize"/> or the <see cref="SecondPaneSize"/> parameter.
    /// If you specify both the <see cref="FirstPaneSize"/> or the <see cref="SecondPaneSize"/> parameters, then the splitter won't work.
    /// </summary>
    // [Parameter]
    public string FirstPaneSize { get; set; }

    /// <summary>
    /// A callback that will be invoked when the size of the first pane is changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> FirstPaneSizeChanged { get; set; }

    /// <summary>
    /// Determines the minimum distance of pixels of the splitter from the right or the bottom edge of second pane.
    /// </summary>
    // [Parameter]
    public string SecondPaneMinSize { get; set; }

    /// <summary>
    /// Determines pixel distance of the splitter from the right or bottom edge.<br/>
    /// You can specify the pane size to only either the <see cref="FirstPaneSize"/> or the <see cref="SecondPaneSize"/> parameter.
    /// If you specify both the <see cref="FirstPaneSize"/> or the <see cref="SecondPaneSize"/> parameters, then the splitter won't work.
    /// </summary>
    // [Parameter]
    public string SecondPaneSize { get; set; }

    /// <summary>
    /// A callback that will be invoked when the size of the second pane is changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> SecondPaneSizeChanged { get; set; }

    [Parameter]
    public SplitterLayout Layout { get; set; } = SplitterLayout.Horizontal;

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback<string[]> OnResize { get; set; }

    /// <summary>
    /// Determines the unit of the pane size. (Default: <see cref="UnitOfPaneSize.Pixel"/>)
    /// </summary>
    // [Parameter]

    private DotNetObjectReference<Splitter> _thisRef;

#if NET5_0_OR_GREATER
    private IJSObjectReference _handler;
#endif
    /// <summary>
    /// Determines if the spliter is vertical or horizontal.
    /// </summary>
    private bool Vertical => Layout == SplitterLayout.Horizontal;

    private string _prefixCls = "ant-splitter";

    private IList<SplitterPanel> _panes = [];

    private bool Disabled => _panes.Any(x => !x.Resizable);

    private string[] _paneSizes = [];

    internal int GetPaneIndex(SplitterPanel panel) => _panes.IndexOf(panel);

    internal IList<SplitterPanel> GetPanes() => _panes;

    /// <summary>
    /// Represents a component consisting of a movable bar that divides a container's display area into two resizable panes.
    /// </summary>

#if NET5_0_OR_GREATER
    [DynamicDependency(nameof(UpdateSize))]
#endif
    public Splitter()
    {
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ClassMapper.Add($"{_prefixCls}")
            .If($"{_prefixCls}-horizontal", () => Layout == SplitterLayout.Horizontal)
            .If($"{_prefixCls}-vertical", () => Layout == SplitterLayout.Vertical)
            ;
    }

    internal void AddPane(SplitterPanel pane)
    {
        _panes.Add(pane);

        if (_panes.Count == 2)
        {
            UpdatePaneState();
        }
    }

    internal void RemovePane(SplitterPanel pane)
    {
        _panes.Remove(pane);
    }

    protected override bool ShouldRender() => _panes.Count == 2;

    internal void UpdatePaneState()
    {
        if (_panes.Count != 2)
            return;

        FirstPane = _panes[0].ChildContent;
        FirstPaneMinSize = _panes[0].Min;
        FirstPaneSize = _panes[0].Size;

        SecondPane = _panes[1].ChildContent;
        SecondPaneMinSize = _panes[1].Min;
        SecondPaneSize = _panes[1].Size;

        StateHasChanged();
    }

    internal string GetPaneStyle(string paneSize, string minPaneSize)
    {
        var styleKey = Vertical ? "width" : "height";
        var format = "{0}";

        static string GetSizeText(string prefix, string styleKey, string format, string size, string defaultValue)
        {
            return size != null ? $"{prefix}{styleKey}:{string.Format(format, size)};" : defaultValue;
        }
        ;

        return GetSizeText("min-", styleKey, format, minPaneSize, "") + GetSizeText("", styleKey, format, paneSize, "flex:1;");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this._thisRef = DotNetObjectReference.Create(this);

#if NET5_0_OR_GREATER
            this._handler = await JsInvokeAsync<IJSObjectReference>(JSInteropConstants.SplitterHelper.Attach, this._thisRef, this.Ref);
#endif
        }
    }

    [JSInvokable]
    public async Task UpdateSize(string[] paneSizes)
    {
        _paneSizes = paneSizes;
        _panes[0].Size = paneSizes[0];
        _panes[1].Size = paneSizes[1];

        if (OnResize.HasDelegate)
        {
            await OnResize.InvokeAsync(_paneSizes);
        }
    }

    protected override void Dispose(bool disposing)
    {
#if NET5_0_OR_GREATER
        if (this._handler != null)
        {
            try
            {
                _ = this._handler.InvokeVoidAsync("dispose");
                _ = this._handler.DisposeAsync();
            }
#if NET6_0_OR_GREATER
            catch (JSDisconnectedException) { }
#endif
            catch { }
        }
#endif
        this._thisRef?.Dispose();
        base.Dispose(disposing);
    }

}
