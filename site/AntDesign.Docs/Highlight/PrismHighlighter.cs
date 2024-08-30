// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Highlight
{
    public class PrismHighlighter : IPrismHighlighter
    {
        private readonly IJSRuntime jsRuntime;

        public PrismHighlighter(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async ValueTask<MarkupString> HighlightAsync(string code, string language)
        {
            string highlighted = await jsRuntime.InvokeAsync<string>("XPrism.highlight", code, language);

            return new MarkupString(highlighted);
        }

        public async Task HighlightAllAsync()
        {
            await jsRuntime.InvokeVoidAsync("XPrism.highlightAll");
        }
    }
}
