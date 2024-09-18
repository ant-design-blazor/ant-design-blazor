// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace AntDesign.Core.Internal;

public class KeepAlive : IComponent
{
    private RenderHandle _renderHandle;
    private bool _rendered = false;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public void Attach(RenderHandle renderHandle)
    {
        _renderHandle = renderHandle;
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (!_rendered && ChildContent != null)
        {
            _rendered = true;
            _renderHandle.Render(ChildContent);
        }

        return Task.CompletedTask;
    }
}
