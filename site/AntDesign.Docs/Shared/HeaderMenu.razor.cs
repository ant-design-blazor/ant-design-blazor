﻿using System.Globalization;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class HeaderMenu : ComponentBase
    {
        [Parameter] public bool IsMobile { get; set; }

        [Inject] private DemoService DemoService { get; set; }

        [Inject] private ILanguageService LanguageService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] public IJSRuntime JsInterop { get; set; }

        [CascadingParameter] public ConfigProvider ConfigProvider { get; set; }

        private string CurrentLanguage => LanguageService.CurrentCulture.Name;

        private string Direction => ConfigProvider?.Direction;

        private DemoMenuItem[] _menuItems = { };

        private bool _firstRender;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _menuItems = await DemoService.GetMenuAsync();

            LanguageService.LanguageChanged += OnLanguageChanged;
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
                await JsInterop.InvokeVoidAsync("window.DocSearch.init", CurrentLanguage);
            }
        }

        private async void OnLanguageChanged(object sender, CultureInfo culture)
        {
            if (!_firstRender)
            {
                return;
            }
            _menuItems = await DemoService.GetMenuAsync();
            await JsInterop.InvokeVoidAsync("window.DocSearch.init", culture.Name);
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            LanguageService.LanguageChanged -= OnLanguageChanged;
        }
    }
}
