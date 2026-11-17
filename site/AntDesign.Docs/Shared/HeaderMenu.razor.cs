// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class HeaderMenu : ComponentBase, IDisposable
    {
        // Keep this aligned with the current React v6 documentation package.
        private const string VersionLabel = "6.7.1";

        private sealed record VersionOption(string Label, string Url);

        private static readonly VersionOption[] VersionOptions =
        {
            new(VersionLabel, "__current__"),
            new("5.x", "https://5x.ant.design"),
            new("4.x", "https://4x.ant.design"),
            new("3.x", "https://3x.ant.design"),
            new("2.x", "https://2x.ant.design"),
            new("1.x", "https://1x.ant.design"),
            new("0.12.x", "https://012x.ant.design"),
            new("0.11.x", "https://011x.ant.design"),
            new("0.10.x", "https://010x.ant.design"),
            new("0.9.x", "https://09x.ant.design"),
        };

        private string CurrentVersionUrl => "__current__";

        [Parameter] public bool IsMobile { get; set; }

        [Inject] private DemoService DemoService { get; set; }

        [Inject] private ILocalizationService LocalizationService { get; set; }

        [Inject] private IStringLocalizer Localizer { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] public IJSRuntime JsInterop { get; set; }

        [CascadingParameter] public ConfigProvider ConfigProvider { get; set; }

        private string CurrentLanguage => LocalizationService.CurrentCulture.Name;

        private string Direction => ConfigProvider?.Direction;

        private DemoMenuItem[] _menuItems = { };
        private string[] _openMenuKeys = Array.Empty<string>();

        private bool _firstRender;
        private bool _disposed;

        internal void ResetOpenSubmenus()
        {
            if (_openMenuKeys.Length == 0) return;
            _openMenuKeys = Array.Empty<string>();
            StateHasChanged();
        }

        private sealed record HeaderNavigationItem(string Key, string Title, string Url);
        private IReadOnlyList<HeaderNavigationItem> NavigationItems
        {
            get
            {
                var items = _menuItems.OrderBy(item => item.Order)
                    .Select(item => new HeaderNavigationItem(item.Url, item.Title, GetSectionUrl(item.Url))).ToList();
                if (CurrentLanguage == "zh-CN") items.Add(new("support", "支持 & 服务", "zh-CN/support"));
                items.Add(new("ai", "AI", $"https://x.antblazor.com/{CurrentLanguage}"));
                return items;
            }
        }

        private string GetSectionUrl(string url)
        {
            var localizedUrl = DemoService.GetLocalizedUrl(CurrentLanguage, url);
            var uri = new Uri(new Uri(NavigationManager.BaseUri), localizedUrl);
            var path = uri.AbsolutePath.TrimEnd('/');
            var lastSlash = path.LastIndexOf('/');
            if (lastSlash > 0 && string.Equals(path[(lastSlash + 1)..], "overview", StringComparison.OrdinalIgnoreCase))
            {
                path = path[..lastSlash];
            }
            return path;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _menuItems = await DemoService.GetMenuAsync();

            LocalizationService.LanguageChanged += OnLanguageChanged;
        }

        private void ChangeLanguage(string language)
        {
            var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var newUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
            NavigationManager.NavigateTo($"{language}/{newUrl}");
        }

        private void OnVersionSelected(VersionOption option)
        {
            if (option == null) return;

            var current = new Uri(NavigationManager.Uri);
            var relativePath = current.AbsolutePath;
            var languagePrefix = $"/{CurrentLanguage}";
            if (relativePath.StartsWith(languagePrefix, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath[languagePrefix.Length..];
            }

            if (option.Url == CurrentVersionUrl)
            {
                NavigationManager.NavigateTo($"/{CurrentLanguage}{relativePath}{current.Query}{current.Fragment}", forceLoad: true);
                return;
            }

            var target = new UriBuilder(option.Url)
            {
                Path = relativePath,
                Query = current.Query.TrimStart('?'),
                Fragment = current.Fragment.TrimStart('#')
            };
            NavigationManager.NavigateTo(target.Uri.AbsoluteUri, forceLoad: true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _firstRender = true;
                if (!IsMobile || ShowSearch) await JsInterop.InvokeVoidAsync("window.DocSearchInit", CurrentLanguage);
            }
        }

        private async void OnLanguageChanged(object sender, CultureInfo culture)
        {
            if (!_firstRender || _disposed)
            {
                return;
            }
            _menuItems = await DemoService.GetMenuAsync();
            if (!IsMobile || ShowSearch) await JsInterop.InvokeVoidAsync("window.DocSearchInit", culture.Name);
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            LocalizationService.LanguageChanged -= OnLanguageChanged;
        }
    }
}
