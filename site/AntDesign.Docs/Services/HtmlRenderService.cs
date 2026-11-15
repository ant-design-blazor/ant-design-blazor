// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign.Docs.Services;

internal sealed class HtmlRenderService(HtmlRenderer htmlRenderer)
{
    public async ValueTask<string> RenderAsync(RenderFragment renderFragment)
    {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object>
        {
            [nameof(ContentComponent.Content)] = renderFragment
        });

        var rendered = await htmlRenderer.Dispatcher.InvokeAsync(
            () => htmlRenderer.RenderComponentAsync<ContentComponent>(parameters));

        return rendered.ToHtmlString();
    }

    private sealed class ContentComponent : ComponentBase
    {
        [Parameter]
        public RenderFragment Content { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.AddContent(0, Content);
        }
    }
}
