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
        private bool _initialized;
        private Dropdown _dropdown;
        private IJSObjectReference _happyWorkModule;
        private DotNetObjectReference<DocsThemeSwitch> _selfReference;

        private string ThemeFileUrl => $"_content/AntDesign/css/antd-v6/{(_compact ? $"{_appearance}-compact" : _appearance)}.css";

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
            await CloseDropdownAsync();
            _themeMode = appearance;
            _appearance = appearance == "dark" ? "dark" : appearance == "auto"
                ? await _happyWorkModule.InvokeAsync<string>("getSystemAppearance")
                : "light";
            await SaveAndApplyThemeAsync();
        }

        private async Task ToggleCompact()
        {
            await CloseDropdownAsync();
            _compact = !_compact;
            await SaveAndApplyThemeAsync();
        }

        private async Task ToggleHappyWork()
        {
            await CloseDropdownAsync();
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

        private async Task CloseDropdownAsync()
        {
            if (_dropdown != null)
            {
                await _dropdown.Close();
            }
        }

        private async Task ApplyThemeAsync()
        {
            if (SiteTheme != null) await SiteTheme.ApplyAsync(_appearance, _compact);
            await JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "html", new Dictionary<string, string>
            {
                ["data-theme"] = _appearance == "dark" ? "dark" : "default",
                ["data-theme-compact"] = _compact ? "true" : "false"
            });
            await JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "#docs-theme-v6", new Dictionary<string, string>
            {
                ["href"] = ThemeFileUrl
            });
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
            _selfReference?.Dispose();
        }
    }
}
