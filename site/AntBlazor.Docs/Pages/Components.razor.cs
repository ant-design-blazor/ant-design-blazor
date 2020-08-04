using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using AntDesign.Docs.Shared;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Pages
{
    public partial class Components : ComponentBase
    {
        [Parameter]
        public string Name { get; set; }

        [Inject]
        private DemoService DemoService { get; set; }

        [Inject]
        private ILanguageService LanguageService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        private DemoComponent _demoComponent;

        private bool _expanded;

        private string CurrentLanguage => LanguageService.CurrentCulture.Name;

        protected override async Task OnInitializedAsync()
        {
            LanguageService.LanguageChanged += async (sender, args) =>
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    _demoComponent = await DemoService.GetComponentAsync(Name);
                    await MainLayout.ChangePrevNextNav(Name);
                    await InvokeAsync(StateHasChanged);
                }
            };

            NavigationManager.LocationChanged += async (sender, args) =>
            {
                await HandleNavigate();
            };

            await HandleNavigate();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                _demoComponent = await DemoService.GetComponentAsync(Name);
                await MainLayout.ChangePrevNextNav(Name);
            }
        }

        private async Task HandleNavigate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
                var newUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
                var menus = await DemoService.GetMenuAsync();
                var current = menus.FirstOrDefault(x => x.Url == newUrl);
                if (current != null)
                {
                    NavigationManager.NavigateTo($"{CurrentLanguage}/{current.Children[0].Children[0].Url}");
                }
            }
        }
    }
}
