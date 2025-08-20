// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core
{
    public class HtmlRenderService
    {
        private readonly HtmlRenderer _htmlRenderer;

        public HtmlRenderService(HtmlRenderer htmlRenderer)
        {
            _htmlRenderer = htmlRenderer;
        }

        public async ValueTask<string> RenderAsync(RenderFragment renderFragment)
        {
            var text = await _htmlRenderer.Dispatcher.InvokeAsync(() => _htmlRenderer.RenderComponentAsync(new EmptyComponent(renderFragment), ParameterView.Empty));
            return string.Join("", text.Tokens);
        }

        private class EmptyComponent : IComponent
        {
            private RenderHandle _renderHandle;

            private readonly RenderFragment _content;

            public EmptyComponent(RenderFragment content)
            {
                this._content = content;
            }

            public void Attach(RenderHandle renderHandle)
            {
                _renderHandle = renderHandle;
            }

            public Task SetParametersAsync(ParameterView parameters)
            {
                _renderHandle.Render(_content);
                return Task.CompletedTask;
            }
        }
    }
}
