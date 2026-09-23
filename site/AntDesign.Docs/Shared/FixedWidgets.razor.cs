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

        private const string ThemeStorageKey = "ant-design-blazor-doc-theme";
        private const string HappyWorkStorageKey = "ant-design-blazor-doc-happy-work";
        private string _appearance = "light";
        private bool _compact;
        private bool _happyWorkEnabled;
        private bool _initialized;
        private IJSObjectReference _happyWorkModule;

        private string _v6ThemeFileUrl => $"_content/AntDesign/css/antd-v6/{(_compact ? $"{_appearance}-compact" : _appearance)}.css";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender || _initialized) return;

            _initialized = true;
            _happyWorkModule = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/AntDesign.Docs/js/happy-work.js");
            var savedTheme = await JS.InvokeAsync<string>("localStorage.getItem", ThemeStorageKey);
            _happyWorkEnabled = await JS.InvokeAsync<string>("localStorage.getItem", HappyWorkStorageKey) == "true";
            if (!string.IsNullOrWhiteSpace(savedTheme))
            {
                _compact = savedTheme.EndsWith("-compact", System.StringComparison.Ordinal);
                _appearance = savedTheme.StartsWith("dark", System.StringComparison.Ordinal) ? "dark" : "light";
                StateHasChanged();
            }

            await ApplyThemeAsync();
            await _happyWorkModule.InvokeVoidAsync("setEnabled", _happyWorkEnabled);
        }

        private async Task ChangeAppearance(string appearance)
        {
            _appearance = appearance == "dark" ? "dark" : "light";
            await SaveAndApplyThemeAsync();
        }

        private async Task ToggleCompact()
        {
            _compact = !_compact;
            await SaveAndApplyThemeAsync();
        }

        private async Task ToggleHappyWork()
        {
            _happyWorkEnabled = !_happyWorkEnabled;
            await JS.InvokeVoidAsync("localStorage.setItem", HappyWorkStorageKey, _happyWorkEnabled ? "true" : "false");
            await _happyWorkModule.InvokeVoidAsync("setEnabled", _happyWorkEnabled);
        }

        private async Task SaveAndApplyThemeAsync()
        {
            await JS.InvokeVoidAsync("localStorage.setItem", ThemeStorageKey, _compact ? $"{_appearance}-compact" : _appearance);
            await ApplyThemeAsync();
        }

        private async Task ApplyThemeAsync()
        {
            await JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "html", new Dictionary<string, string>
            {
                ["data-theme"] = _appearance == "dark" ? "dark" : "default",
                ["data-theme-compact"] = _compact ? "true" : "false"
            });
            await JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "#docs-theme-v6", new Dictionary<string, string>
            {
                ["href"] = _v6ThemeFileUrl
            });
        }
    }
}
