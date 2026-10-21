// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign;

public class SplitterPanel : ComponentBase, IDisposable
{
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string DefaultSize { get; set; }

    [Parameter]
    public string Min { get; set; }

    [Parameter]
    public string Max { get; set; }

    [Parameter]
    public string Size { get; set; }

    [Parameter]
    public bool Collapsible { get; set; }

    /// <summary>
    /// Controls the visibility of collapse icons. True keeps them visible, false hides them,
    /// and null shows them while the divider is hovered (the default).
    /// </summary>
    [Parameter]
    public bool? ShowCollapsibleIcon { get; set; }

    [Parameter]
    public bool Resizable { get; set; } = true;

    [CascadingParameter]
    private Splitter Splitter { get; set; }

    private bool _isCollapsed;
    public bool IsCollapsed => _isCollapsed;
    private string _currentSize;
    internal string EffectiveSize => _isCollapsed ? "0px" : _currentSize ?? Size ?? DefaultSize;

    private string _prevSize;

    internal void ToggleCollapse()
    {
        var currentSize = EffectiveSize;
        _isCollapsed = !_isCollapsed;
        if (_isCollapsed)
        {
            _prevSize = currentSize;
        }
        else
        {
            _currentSize = _prevSize ?? Size ?? DefaultSize;
        }

        Splitter.UpdatePaneState();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Splitter?.AddPane(this);
    }

    public void Dispose()
    {
        Splitter?.RemovePane(this);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var isSizeChanged = parameters.IsParameterChanged(nameof(Size), Size);
        var isResizableChanged = parameters.IsParameterChanged(nameof(Resizable), Resizable);
        var isDefaultSizeChanged = parameters.IsParameterChanged(nameof(DefaultSize), DefaultSize);
        var isMinChanged = parameters.IsParameterChanged(nameof(Min), Min);
        var isMaxChanged = parameters.IsParameterChanged(nameof(Max), Max);
        var isShowCollapsibleIconChanged = parameters.IsParameterChanged(nameof(ShowCollapsibleIcon), ShowCollapsibleIcon);

        await base.SetParametersAsync(parameters);

        if (isSizeChanged)
            _currentSize = Size;
        else if (isDefaultSizeChanged && Size == null)
            _currentSize = DefaultSize;

        if (isSizeChanged || isResizableChanged || isDefaultSizeChanged || isMinChanged || isMaxChanged || isShowCollapsibleIconChanged)
        {
            Splitter.UpdatePaneState();
        }
    }

    internal void SetCurrentSize(string size) => _currentSize = size;
}
