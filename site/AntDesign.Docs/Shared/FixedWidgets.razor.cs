// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class FixedWidgets
    {
        [Inject] private IStringLocalizer Localizer { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        private ElementReference _linkRef;

        private string _themeFileUrl;


        private async Task ChangeTheme(string theme)
        {
            _themeFileUrl = $"_content/AntDesign.Docs/css/{theme}.css";

            _ = JS.InvokeVoidAsync(JSInteropConstants.AddElementToBody, _linkRef);
            _ = JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "html", new Dictionary<string, string>
            {
                ["data-theme"] = theme == "compactdark" ? "dark" : theme
            });
        }
    }
}
