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

[Documentation(DocumentationCategory.Components, DocumentationType.Layout, "", Columns = 1, Title = "Splitter", SubTitle = "分隔面板")]
public partial class Splitter : AntDomComponentBase
{
    /// <summary>Semantic classes keyed by root, panel, dragger, dragger.default, and dragger.active.</summary>
    [Parameter]
    public IReadOnlyDictionary<string, string> ClassNames { get; set; }

    /// <summary>Semantic inline styles keyed by root, panel, dragger, dragger.default, and dragger.active.</summary>
    [Parameter]
    public IReadOnlyDictionary<string, string> Styles { get; set; }

    /// <summary>Custom content rendered in the splitter drag handle.</summary>
    [Parameter]
    public RenderFragment DraggerIcon { get; set; }

    /// <summary>Hides the default drag-handle indicator when no custom icon is supplied.</summary>
    [Parameter]
    public bool HideDraggerIndicator { get; set; }

    /// <summary>Custom collapse icon for the start side of each divider.</summary>
    [Parameter]
    public RenderFragment CollapseIconStart { get; set; }

    /// <summary>Custom collapse icon for the end side of each divider.</summary>
    [Parameter]
    public RenderFragment CollapseIconEnd { get; set; }

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

    /// <summary>Updates panel sizes only after a drag completes.</summary>
    [Parameter]
    public bool Lazy { get; set; }

    /// <summary>Raised when a dragger is double-clicked.</summary>
    [Parameter]
    public EventCallback<int> OnDraggerDoubleClick { get; set; }

    /// <summary>
    /// Determines the unit of the pane size. (Default: <see cref="UnitOfPaneSize.Pixel"/>)
    /// </summary>
    // [Parameter]

    private DotNetObjectReference<Splitter> _thisRef;

#if NET5_0_OR_GREATER
    private IJSObjectReference _handler;
    private Task _attachTask = Task.CompletedTask;
#endif
    private bool _disposed;
    /// <summary>
    /// Determines if the spliter is vertical or horizontal.
    /// </summary>
    private bool Vertical => Layout == SplitterLayout.Horizontal;

    private string _prefixCls = "ant-splitter";

    private IList<SplitterPanel> _panes = [];

    private string[] _paneSizes = [];
    private int _activeDraggerIndex = -1;

    internal int GetPaneIndex(SplitterPanel panel) => _panes.IndexOf(panel);

    internal IList<SplitterPanel> GetPanes() => _panes;

    /// <summary>
    /// Represents a component consisting of a movable bar that divides a container's display area into two resizable panes.
    /// </summary>

#if NET5_0_OR_GREATER
    [DynamicDependency(nameof(UpdateSize))]
    [DynamicDependency(nameof(DraggerDoubleClick))]
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
        if (_panes.Count >= 2)
            UpdatePaneState();
    }

    internal void RemovePane(SplitterPanel pane)
    {
        _panes.Remove(pane);
    }

    protected override bool ShouldRender() => _panes.Count >= 2;

    internal void UpdatePaneState()
    {
        if (_panes.Count < 2)
            return;

        FirstPane = _panes[0].ChildContent;
        FirstPaneMinSize = _panes[0].Min;
        FirstPaneSize = _panes[0].Size;

        SecondPane = _panes[1].ChildContent;
        SecondPaneMinSize = _panes[1].Min;
        SecondPaneSize = _panes[1].Size;

        StateHasChanged();
    }

    internal string GetPaneStyle(SplitterPanel pane)
    {
        var size = pane.EffectiveSize;
        var styles = new List<string>
        {
            size == null ? "flex-basis:auto;flex-grow:1" : $"flex-basis:{size};flex-grow:0",
        };

        if (pane.IsCollapsed)
            styles.Add("overflow:hidden");

        return string.Join(";", styles) + ";";
    }

    internal string SemanticClass(string slot, string baseClass)
    {
        var customClass = ClassNames != null && ClassNames.TryGetValue(slot, out var value) ? value : null;
        return string.Join(" ", new[] { baseClass, customClass }.Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    private string GetPanelClass(SplitterPanel pane)
    {
        var baseClass = pane.IsCollapsed ? "ant-splitter-panel ant-splitter-panel-hidden" : "ant-splitter-panel";
        return SemanticClass("panel", baseClass);
    }

    internal string SemanticStyle(string slot, string baseStyle = null)
    {
        var customStyle = Styles != null && Styles.TryGetValue(slot, out var value) ? value : null;
        var styles = new[] { baseStyle, customStyle }.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim().TrimEnd(';') + ";");
        return string.Join(" ", styles);
    }

    private bool IsDraggerDisabled(int index) => !_panes[index].Resizable || !_panes[index + 1].Resizable;

    private string GetCollapseIconType(SplitterPanel pane, bool isStart)
    {
        if (Vertical)
            return (isStart ^ pane.IsCollapsed) ? "left" : "right";

        return (isStart ^ pane.IsCollapsed) ? "up" : "down";
    }

    private static string GetCollapseVisibilityClass(SplitterPanel pane) => pane.ShowCollapsibleIcon switch
    {
        true => "ant-splitter-bar-collapse-bar-always-visible",
        false => "ant-splitter-bar-collapse-bar-always-hidden",
        _ => "ant-splitter-bar-collapse-bar-hover-only",
    };

    private string GetDraggerClass(int index)
    {
        var isActive = _activeDraggerIndex == index;
        var customizeClass = DraggerIcon != null || HideDraggerIndicator ? "ant-splitter-bar-dragger-customize" : null;
        var classes = new List<string> { "ant-splitter-bar-dragger" };
        if (customizeClass != null)
            classes.Add(customizeClass);
        if (IsDraggerDisabled(index))
            classes.Add("ant-splitter-bar-dragger-disabled");
        var stateClass = isActive ? "ant-splitter-bar-dragger-active" : null;
        var stateSlot = isActive ? "dragger.active" : "dragger.default";
        if (stateClass != null)
            classes.Add(stateClass);
        return SemanticClass(stateSlot, SemanticClass("dragger", string.Join(" ", classes)));
    }

    private string GetDraggerStyle(int index)
    {
        var stateSlot = _activeDraggerIndex == index ? "dragger.active" : "dragger.default";
        var stateStyle = Styles != null && Styles.TryGetValue(stateSlot, out var value) ? value : null;
        return SemanticStyle("dragger", stateStyle);
    }

    [JSInvokable]
    public Task SetDragging(bool isDragging, int index = -1)
    {
        if (_disposed) return Task.CompletedTask;
        var nextIndex = isDragging ? index : -1;
        if (_activeDraggerIndex == nextIndex)
            return Task.CompletedTask;

        _activeDraggerIndex = nextIndex;
        return InvokeStateHasChangedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_disposed)
        {
            this._thisRef = DotNetObjectReference.Create(this);

#if NET5_0_OR_GREATER
            var paneOptions = _panes.Select(pane => new
            {
                resizable = pane.Resizable,
                min = pane.Min,
                max = pane.Max,
                size = pane.Size,
                defaultSize = pane.DefaultSize,
            }).ToArray();
            _attachTask = AttachInteropAsync(paneOptions);
            await _attachTask;
#endif
        }
    }

    [JSInvokable]
    public Task InitializeSizes(string[] paneSizes)
    {
        if (_disposed) return Task.CompletedTask;
        for (var i = 0; i < Math.Min(_panes.Count, paneSizes.Length); i++)
            _panes[i].SetCurrentSize(paneSizes[i]);

        return InvokeStateHasChangedAsync();
    }

    [JSInvokable]
    public async Task UpdateSize(string[] paneSizes)
    {
        if (_disposed) return;
        _paneSizes = paneSizes;
        for (var i = 0; i < Math.Min(_panes.Count, paneSizes.Length); i++)
            _panes[i].SetCurrentSize(paneSizes[i]);

        await InvokeStateHasChangedAsync();

        if (OnResize.HasDelegate)
        {
            await OnResize.InvokeAsync(_paneSizes);
        }
    }

    [JSInvokable]
    public Task DraggerDoubleClick(int index) => OnDraggerDoubleClick.InvokeAsync(index);

    protected override void Dispose(bool disposing)
    {
        _disposed = true;
#if NET5_0_OR_GREATER
        _ = DisposeInteropAsync();
#else
        this._thisRef?.Dispose();
#endif
        base.Dispose(disposing);
    }

#if NET5_0_OR_GREATER
    private async Task AttachInteropAsync(object paneOptions)
    {
        _handler = await JsInvokeAsync<IJSObjectReference>(JSInteropConstants.SplitterHelper.Attach, _thisRef, Ref, Lazy, paneOptions);
    }

    private async Task DisposeInteropAsync()
    {
        // A responsive host may unmount while attach is awaiting InitializeSizes.
        // Keep the .NET reference alive until that handshake and JS cleanup end.
        try
        {
            await _attachTask;
            if (_handler != null)
            {
                await _handler.InvokeVoidAsync("dispose");
                await _handler.DisposeAsync();
            }
        }
#if NET6_0_OR_GREATER
        catch (JSDisconnectedException) { }
#endif
        catch (TaskCanceledException) { }
        catch (ObjectDisposedException) { }
        finally { _thisRef?.Dispose(); }
    }
#endif

}
