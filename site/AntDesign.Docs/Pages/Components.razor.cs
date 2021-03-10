using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using AntDesign.Docs.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign.Docs.Pages
{
    public partial class Components : ComponentBase, IDisposable
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

        private bool _expandAllCode;

        private string CurrentLanguage => LanguageService.CurrentCulture.Name;

        private string _filePath;

        private string EditUrl => $"https://github.com/ant-design-blazor/ant-design-blazor/blob/master/{_filePath}";

        protected override async Task OnInitializedAsync()
        {
            LanguageService.LanguageChanged += OnLanguageChanged;
            NavigationManager.LocationChanged += OnLocationChanged;

            await HandleNavigate();
        }

        private async void OnLanguageChanged(object sender, CultureInfo args)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                await InvokeAsync(HandleNavigate);
                await InvokeAsync(StateHasChanged);
            }
        }

        private async void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            await HandleNavigate();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await HandleNavigate();
        }

        private async Task HandleNavigate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
                var newUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
                var menus = await DemoService.GetMenuAsync();
                var current = menus.FirstOrDefault(x => x.Url == newUrl.ToLowerInvariant());
                if (current != null)
                {
                    NavigationManager.NavigateTo($"{CurrentLanguage}/{current.Children[0].Children[0].Url}");
                }
            }
            else
            {
                await MainLayout.ChangePrevNextNav(Name);
                _demoComponent = await DemoService.GetComponentAsync(Name);
                _filePath = $"site/AntDesign.Docs/Demos/Components/{_demoComponent?.Title}/doc/index.{CurrentLanguage}.md";

                StateHasChanged();
            }
        }

        public void Dispose()
        {
            LanguageService.LanguageChanged -= OnLanguageChanged;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
