using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        private bool _isCollapsed = false;
        private bool _drawerVisible = false;

        public string CurrentLanguage => LanguageService.CurrentCulture.Name;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILanguageService LanguageService { get; set; }

        [Inject]
        public DemoService DemoService { get; set; }

        [Inject]
        public IJSRuntime JsInterop { get; set; }

        internal PrevNextNav PrevNextNav { get; set; }

        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();
            await DemoService.InitializeDemos();

            LanguageService.LanguageChanged += OnLanguageChanged;
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsInterop.InvokeVoidAsync("window.AntDesign.DocSearch.init", CurrentLanguage);
            }
        }

        public async Task ChangePrevNextNav(string currentTitle)
        {
            if (string.IsNullOrWhiteSpace(currentTitle))
                return;

            var currentSubmenuUrl = DemoService.GetCurrentSubMenuUrl();
            var prevNext = await DemoService.GetPrevNextMenu(currentSubmenuUrl, currentTitle);

            PrevNextNav?.SetPrevNextNav(prevNext[0], prevNext[1]);
        }

        private async void OnLanguageChanged(object sender, CultureInfo culture)
        {
            await JsInterop.InvokeVoidAsync("window.AntDesign.DocSearch.localeChange", culture.Name);
            await InvokeAsync(StateHasChanged);
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            StateHasChanged();
        }

        private void OnBreakpoint(BreakpointType breakpoint)
        {
            _isCollapsed = breakpoint.IsIn(BreakpointType.Sm, BreakpointType.Xs);
        }

        public void Dispose()
        {
            LanguageService.LanguageChanged -= OnLanguageChanged;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
