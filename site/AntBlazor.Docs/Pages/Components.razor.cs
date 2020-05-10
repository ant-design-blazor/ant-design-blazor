using System;
using System.Linq;
using System.Threading.Tasks;
using AntBlazor.Docs.Localization;
using AntBlazor.Docs.Services;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Docs.Pages
{
    public partial class Components
    {
        [Parameter]
        public string Name { get; set; }

        [Inject]
        private DemoService DemoService { get; set; }

        [Inject]
        private ILanguageService LanguageService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private DemoComponent _demoComponent;

        private string CurrentLanguage => LanguageService.CurrentCulture.Name;

        protected override async Task OnInitializedAsync()
        {
            LanguageService.LanguageChanged += async (sender, args) =>
            {
                _demoComponent = await DemoService.GetComponentAsync(Name);
                await InvokeAsync(StateHasChanged);
            };

            NavigationManager.LocationChanged += (_, args) =>
            {
                StateHasChanged();
            };

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

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                _demoComponent = await DemoService.GetComponentAsync(Name);
            }
        }
    }
}
