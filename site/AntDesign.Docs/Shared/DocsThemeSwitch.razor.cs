using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class DocsThemeSwitch : ComponentBase, IAsyncDisposable
    {
        [Inject] private IStringLocalizer Localizer { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [CascadingParameter] private AntDesign.Docs.Components.DocsThemeProvider SiteTheme { get; set; }

        private const string ThemeStorageKey = "ant-design-blazor-doc-theme";
        private const string HappyWorkStorageKey = "ant-design-blazor-doc-happy-work";
        private string _appearance = "light";
        private string _themeMode = "light";
        private bool _compact;
        private bool _happyWorkEnabled;
        private bool _popoverVisible;
        private bool _initialized;
        private IJSObjectReference _happyWorkModule;
        private IJSObjectReference _themeModule;
        private DotNetObjectReference<DocsThemeSwitch> _selfReference;

        private string ThemeFileUrl => $"_content/AntDesign/css/antd-v6/{(_compact ? $"{_appearance}-compact" : _appearance)}.css";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender || _initialized) return;

            _initialized = true;
            _happyWorkModule = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/AntDesign.Docs/js/happy-work.js");
            _themeModule = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/AntDesign.Docs/js/docs-theme.js");
            var savedTheme = await JS.InvokeAsync<string>("localStorage.getItem", ThemeStorageKey);
            _happyWorkEnabled = await JS.InvokeAsync<string>("localStorage.getItem", HappyWorkStorageKey) == "true";
            if (!string.IsNullOrWhiteSpace(savedTheme))
            {
                _compact = savedTheme.EndsWith("-compact", System.StringComparison.Ordinal);
                _themeMode = savedTheme.StartsWith("auto", System.StringComparison.Ordinal) ? "auto" : savedTheme.StartsWith("dark", System.StringComparison.Ordinal) ? "dark" : "light";
                _appearance = _themeMode == "auto"
                    ? await _happyWorkModule.InvokeAsync<string>("getSystemAppearance")
                    : _themeMode;
                StateHasChanged();
            }

            await ApplyThemeAsync();
            await _happyWorkModule.InvokeVoidAsync("setEnabled", _happyWorkEnabled);
            _selfReference = DotNetObjectReference.Create(this);
            await _happyWorkModule.InvokeVoidAsync("watchSystemAppearance", _selfReference);
        }

        private async Task ChangeAppearance(string appearance)
        {
            await ClosePopoverAsync();
            _themeMode = appearance;
            _appearance = appearance == "dark" ? "dark" : appearance == "auto"
                ? await _happyWorkModule.InvokeAsync<string>("getSystemAppearance")
                : "light";
            await SaveAndApplyThemeAsync();
        }

        private async Task ToggleCompact()
        {
            await ClosePopoverAsync();
            _compact = !_compact;
            await SaveAndApplyThemeAsync();
        }

        private async Task ToggleHappyWork()
        {
            await ClosePopoverAsync();
            _happyWorkEnabled = !_happyWorkEnabled;
            await JS.InvokeVoidAsync("localStorage.setItem", HappyWorkStorageKey, _happyWorkEnabled ? "true" : "false");
            await _happyWorkModule.InvokeVoidAsync("setEnabled", _happyWorkEnabled);
        }

        private async Task SaveAndApplyThemeAsync()
        {
            var savedTheme = _themeMode == "auto" ? (_compact ? "auto-compact" : "auto") : _compact ? $"{_appearance}-compact" : _appearance;
            await JS.InvokeVoidAsync("localStorage.setItem", ThemeStorageKey, savedTheme);
            await ApplyThemeAsync();
        }

        private Task ClosePopoverAsync()
        {
            _popoverVisible = false;
            return Task.CompletedTask;
        }

        private async Task ApplyThemeAsync()
        {
            await JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "html", new Dictionary<string, string>
            {
                ["data-theme"] = _appearance == "dark" ? "dark" : "default",
                ["data-theme-compact"] = _compact ? "true" : "false"
            });

            if (_themeModule != null)
            {
                await JS.InvokeAsync<bool>("window.setDocsThemeStylesheet", ThemeFileUrl);
            }
            else
            {
                await JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "#docs-theme-v6", new Dictionary<string, string>
                {
                    ["href"] = ThemeFileUrl
                });
            }

            // Publish the new ConfigProvider theme only after the matching
            // generated stylesheet has loaded. Open body-portaled overlays
            // then re-read tokens from their triggers in the new theme.
            if (SiteTheme != null) await SiteTheme.ApplyAsync(_appearance, _compact);
        }

        [JSInvokable]
        public async Task OnSystemAppearanceChanged(string appearance)
        {
            if (_themeMode != "auto") return;
            _appearance = appearance == "dark" ? "dark" : "light";
            await ApplyThemeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_happyWorkModule != null)
            {
                try
                {
                    await _happyWorkModule.InvokeVoidAsync("stopWatchingSystemAppearance");
                    await _happyWorkModule.DisposeAsync();
                }
                catch (JSDisconnectedException) { }
            }
            if (_themeModule != null)
            {
                try
                {
                    await _themeModule.DisposeAsync();
                }
                catch (JSDisconnectedException) { }
            }
            _selfReference?.Dispose();
        }
    }
}
