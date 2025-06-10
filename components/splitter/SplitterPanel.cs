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
    public string Size { get; set; } = "50%";

    [Parameter]
    public bool Collapsible { get; set; }

    [Parameter]
    public bool Resizable { get; set; } = true;

    [CascadingParameter]
    private Splitter Splitter { get; set; }

    private bool _isCollapsed;
    public bool IsCollapsed => _isCollapsed;

    private string _prevSize;

    internal void ToggleCollapse()
    {
        var isSecondPane = Splitter.GetPaneIndex(this) == 1;
        var otherPane = Splitter.GetPanes()[isSecondPane ? 0 : 1];

        // If the other panel is collapsed, restore it first
        if (otherPane._isCollapsed)
        {
            otherPane._isCollapsed = false;
            otherPane.Size = otherPane._prevSize;
        }

        _isCollapsed = !_isCollapsed;
        if (_isCollapsed)
        {
            _prevSize = Size;
            Size = Min ?? "0px";

            if (isSecondPane)
            {
                // When second panel is collapsed, first panel should take full size
                otherPane.Size = "100%";
            }
            else
            {
                // When first panel is collapsed, second panel should take full size
                otherPane.Size = "100%";
            }
        }
        else
        {
            Size = _prevSize;
            // Restore other panel to default size
            otherPane.Size = "50%";
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

        await base.SetParametersAsync(parameters);

        if (isSizeChanged || isResizableChanged)
        {
            Splitter.UpdatePaneState();
        }
    }
}
