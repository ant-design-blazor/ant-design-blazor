using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Shared
{
    public partial class MainLayout : IDisposable
    {
        private bool _isCollapsed = true;

        private string CurrentLanguage => LanguageService.CurrentCulture.Name;

        private string _currentSubmenuUrl;

        private DemoMenuItem[] MenuItems { get; set; } = { };

        private DemoMenuItem[] SiderMenuItems { get; set; } = { };

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILanguageService LanguageService { get; set; }

        [Inject]
        public DemoService DemoService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //await GetCurrentMenuItems();
            //StateHasChanged();

            LanguageService.LanguageChanged += OnLanguageChanged;

            NavigationManager.LocationChanged += (_, args) =>
            {
                StateHasChanged();
            };
        }

        private async ValueTask GetCurrentMenuItems()
        {
            var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var originalUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;

            var shouldNavigate = this._currentSubmenuUrl == null && string.IsNullOrEmpty(originalUrl);

            MenuItems = await DemoService.GetMenuAsync();
            this._currentSubmenuUrl ??= string.IsNullOrEmpty(originalUrl) ? this.MenuItems[0].Url : originalUrl.Split('/')[0];
            SiderMenuItems = MenuItems.FirstOrDefault(x => x.Url == this._currentSubmenuUrl)?.Children ?? Array.Empty<DemoMenuItem>();

            if (shouldNavigate)
            {
                NavigationManager.NavigateTo($"{CurrentLanguage}/{this._currentSubmenuUrl}");
            }
        }

        private async ValueTask ChangeMenuItem(string url)
        {
            this._currentSubmenuUrl = url;
            await GetCurrentMenuItems();
        }

        private void ChangeLanguage(string language)
        {
            var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var newUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
            NavigationManager.NavigateTo($"{language}/{newUrl}");
        }

        private async void OnLanguageChanged(object sender, CultureInfo args)
        {
            await GetCurrentMenuItems();
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            LanguageService.LanguageChanged -= OnLanguageChanged;
        }
    }
}
