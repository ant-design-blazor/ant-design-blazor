using System;
using System.Linq;
using System.Threading.Tasks;
using AntBlazor.Docs.Localization;
using AntBlazor.Docs.Services;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Docs.Shared
{
    public partial class MainLayout
    {
        private bool _isCollapsed = true;

        private string CurrentLanguage => LanguageService.CurrentCulture.Name;

        private string _currentSubmenu = "文档";

        private MenuItem[] MenuItems { get; set; } = { };

        private MenuItem[] SiderMenuItems { get; set; } = { };

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILanguageService LanguageService { get; set; }

        [Inject]
        public DemoService DemoService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetCurrentMenuItems();
            StateHasChanged();
            ChangeMenuItem(this.MenuItems[0].Title);

            LanguageService.LanguageChanged += async (_, args) =>
            {
                await GetCurrentMenuItems();
                await InvokeAsync(StateHasChanged);
            };

            NavigationManager.LocationChanged += (_, args) =>
            {
                StateHasChanged();
            };
        }

        private async ValueTask GetCurrentMenuItems()
        {
            MenuItems = await DemoService.GetMenuAsync();
        }

        private void ChangeMenuItem(string title)
        {
            this._currentSubmenu = title;
            SiderMenuItems = MenuItems.FirstOrDefault(x => x.Title == title)?.Children ?? Array.Empty<MenuItem>();
            StateHasChanged();
        }

        private void ChangeLanguage(string language)
        {
            var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var newUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
            NavigationManager.NavigateTo($"{language}/{newUrl}");
        }
    }
}
