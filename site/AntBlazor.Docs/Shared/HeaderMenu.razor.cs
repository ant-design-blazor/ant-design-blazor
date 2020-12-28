using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Shared
{
    public partial class HeaderMenu : ComponentBase
    {
        [Parameter] public bool IsMobile { get; set; }

        [Inject] private DemoService DemoService { get; set; }

        [Inject] private ILanguageService LanguageService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        string CurrentLanguage => LanguageService.CurrentCulture.Name;
        private DemoMenuItem[] _menuItems = { };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _menuItems = await DemoService.GetMenuAsync();

            LanguageService.LanguageChanged += async (sender, args) =>
            {
                _menuItems = await DemoService.GetMenuAsync();
                await InvokeAsync(StateHasChanged);
            };
        }

        private void ChangeLanguage(string language)
        {
            var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var newUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
            NavigationManager.NavigateTo($"{language}/{newUrl}");
        }
    }
}
