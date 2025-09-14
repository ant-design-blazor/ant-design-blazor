// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class HeaderMenu : ComponentBase
    {
        [Parameter] public bool IsMobile { get; set; }

        [Inject] private DemoService DemoService { get; set; }

        [Inject] private ILocalizationService LocalizationService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] public IJSRuntime JsInterop { get; set; }

        [Inject] public VersionService VersionService { get; set; }

        [CascadingParameter] public ConfigProvider ConfigProvider { get; set; }

        private string CurrentLanguage => LocalizationService.CurrentCulture.Name;

        private string Direction => ConfigProvider?.Direction;

        private DemoMenuItem[] _menuItems = { };

        private bool _firstRender;
        private string _version;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _menuItems = await DemoService.GetMenuAsync();
            _version = await VersionService.GetCurrentVersion();

            LocalizationService.LanguageChanged += OnLanguageChanged;
        }

        private void ChangeLanguage(string language)
        {
            var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var newUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
            NavigationManager.NavigateTo($"{language}/{newUrl}");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _firstRender = true;
                await JsInterop.InvokeVoidAsync("window.DocSearchInit", CurrentLanguage);
            }
        }

        private async void OnLanguageChanged(object sender, CultureInfo culture)
        {
            if (!_firstRender)
            {
                return;
            }
            _menuItems = await DemoService.GetMenuAsync();
            await JsInterop.InvokeVoidAsync("window.DocSearchInit", culture.Name);
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
        }
    }
}
