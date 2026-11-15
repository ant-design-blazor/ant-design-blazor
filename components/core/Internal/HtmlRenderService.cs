// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core
{
    public class HtmlRenderService
    {
#if NET8_0_OR_GREATER
        private readonly Microsoft.AspNetCore.Components.Web.HtmlRenderer _officialHtmlRenderer;
        private readonly AntDesign.Core.HtmlRenderer _legacyHtmlRenderer;

        public HtmlRenderService(AntDesign.Core.HtmlRenderer htmlRenderer)
        {
            _legacyHtmlRenderer = htmlRenderer;
        }

        internal HtmlRenderService(Microsoft.AspNetCore.Components.Web.HtmlRenderer htmlRenderer)
        {
            _officialHtmlRenderer = htmlRenderer;
        }
#else
        private readonly AntDesign.Core.HtmlRenderer _legacyHtmlRenderer;

        public HtmlRenderService(AntDesign.Core.HtmlRenderer htmlRenderer)
        {
            _legacyHtmlRenderer = htmlRenderer;
        }
#endif

        public async ValueTask<string> RenderAsync(RenderFragment renderFragment)
        {
#if NET8_0_OR_GREATER
            if (_officialHtmlRenderer != null)
            {
                var parameters = ParameterView.FromDictionary(new System.Collections.Generic.Dictionary<string, object>
                {
                    [nameof(EmptyComponent.Content)] = renderFragment
                });
                var rendered = await _officialHtmlRenderer.Dispatcher.InvokeAsync(() => _officialHtmlRenderer.RenderComponentAsync<EmptyComponent>(parameters));
                return rendered.ToHtmlString();
            }
#endif
            var text = await _legacyHtmlRenderer.Dispatcher.InvokeAsync(() => _legacyHtmlRenderer.RenderComponentAsync(new EmptyComponent(renderFragment), ParameterView.Empty));
            return string.Join("", text.Tokens);
        }

        private class EmptyComponent : IComponent
        {
            private RenderHandle _renderHandle;

            public EmptyComponent()
            {
            }

            public EmptyComponent(RenderFragment content)
            {
                Content = content;
            }

            [Parameter]
            public RenderFragment Content { get; set; }

            public void Attach(RenderHandle renderHandle)
            {
                _renderHandle = renderHandle;
            }

            public Task SetParametersAsync(ParameterView parameters)
            {
                parameters.SetParameterProperties(this);
                _renderHandle.Render(Content);
                return Task.CompletedTask;
            }
        }
    }
}
