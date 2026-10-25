// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class IconFont : Icon
    {
        /// <summary>One iconfont script URL, or an ordered array of resources, as in React's createFromIconfontCN.</summary>
        [Parameter]
        public OneOf<string, string[]> ScriptUrl { get; set; }

        private string[] _requestedScriptUrls = Array.Empty<string>();

        protected override void OnParametersSet()
        {
            IconFont = Type;
            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            var scripts = ScriptUrl.Match(url => new[] { url }, urls => urls ?? Array.Empty<string>())
                .Where(url => !string.IsNullOrWhiteSpace(url)).ToArray();
            if (_requestedScriptUrls.SequenceEqual(scripts)) return;
            _requestedScriptUrls = scripts;
            await IconService.CreateFromIconfontCN(scripts);
        }
    }
}
