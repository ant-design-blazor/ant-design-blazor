using System;
using System.Collections.Generic;
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

        private List<string> _filePaths;

        private string EditUrl => $"https://github.com/ant-design-blazor/ant-design-blazor/edit/master/{_filePath}";

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
            Name = null;
            await HandleNavigate();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await HandleNavigate();
        }

        private async Task HandleNavigate()
        {
            var fullPageName = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            fullPageName = fullPageName.IndexOf('/') > 0 ? fullPageName.Substring(fullPageName.IndexOf('/') + 1) : fullPageName;
            fullPageName = fullPageName.IndexOf('#') > 0 ? fullPageName.Substring(0, fullPageName.IndexOf('#')) : fullPageName;
            if (fullPageName.StartsWith("docs"))
            {
                return;
            }
            if (fullPageName.Split("/").Length != 2)
            {
                var menus = await DemoService.GetMenuAsync();
                var current = menus.FirstOrDefault(x => x.Url == fullPageName.ToLowerInvariant());
                if (current != null)
                {
                    NavigationManager.NavigateTo($"{CurrentLanguage}/{current.Children[0].Children[0].Url}");
                }
            }
            else
            {
                await MainLayout.ChangePrevNextNav(Name);
                _demoComponent = await DemoService.GetComponentAsync($"{fullPageName}");
                _filePath = $"site/AntDesign.Docs/Demos/Components/{_demoComponent?.Title}/doc/index.{CurrentLanguage}.md";
                _filePaths = new() { _filePath };
                foreach (var item in _demoComponent.DemoList?.Where(x => !x.Debug && !x.Docs.HasValue) ?? Array.Empty<DemoItem>())
                {
                    _filePaths.Add($"site/AntDesign.Docs/Demos/Components/{_demoComponent?.Title}/demo/{item.Name}.md");
                    _filePaths.Add($"site/AntDesign.Docs/{item.Type.Replace(".", "/")}.razor");
                }
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
