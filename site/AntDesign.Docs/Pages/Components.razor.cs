using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Docs.Shared;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace AntDesign.Docs.Pages
{
    public partial class Components : ComponentBase, IDisposable
    {
        [Parameter]
        public string Locale { get; set; }
        [Parameter]
        public string Name { get; set; }

        [Inject]
        private DemoService DemoService { get; set; }

        [Inject]
        private ILocalizationService LocalizationService { get; set; }

        [Inject]
        private IStringLocalizer Localizer { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        private DemoComponent _demoComponent;

        private bool _expanded;

        private bool _expandAllCode;

        private string CurrentLanguage => LocalizationService.CurrentCulture.Name;

        private string _filePath;

        private List<string> _filePaths;

        private bool _rendered;

        private bool _changed = true;

        private string EditUrl => $"https://github.com/ant-design-blazor/ant-design-blazor/edit/master/{_filePath}";

        private List<DemoItem> _demos = [];

        private string _currentPageName;

        protected override void OnInitialized()
        {
            LocalizationService.LanguageChanged += OnLanguageChanged;
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLanguageChanged(object sender, CultureInfo args)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                _changed = true;
                InvokeAsync(StateHasChanged);
            }
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            _changed = true;
            InvokeAsync(StateHasChanged);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _rendered = true;
                StateHasChanged();
                return;
            }

            if (_rendered && _changed)
            {
                _changed = false;
                _ = HandleNavigate();
            }
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
            _currentPageName = fullPageName;

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
                _demoComponent = await DemoService.GetComponentAsync($"{fullPageName}");
                StateHasChanged();

                _filePath = $"site/AntDesign.Docs/Demos/Components/{_demoComponent?.Title}/doc/index.{CurrentLanguage}.md";
                await LoadDemos(_currentPageName);
                await MainLayout.ChangePrevNextNav(Name);
            }
        }

        private async Task LoadDemos(string pageName)
        {
            _demos = [];
            _filePaths = new() { _filePath };
            foreach (var item in _demoComponent.DemoList?.Where(x => !x.Debug && !x.Docs.HasValue).OrderBy(x => x.Order) ?? Enumerable.Empty<DemoItem>())
            {
                // compoennt is changed
                if (pageName != _currentPageName)
                {
                    return;
                }
                _filePaths.Add($"site/AntDesign.Docs/Demos/Components/{_demoComponent?.Title}/demo/{item.Name}.md");
                _filePaths.Add($"site/AntDesign.Docs/{item.Type.Replace(".", "/")}.razor");
                _demos.Add(item);
                StateHasChanged();
                await Task.Delay(100);
            }
        }

        public void Dispose()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
