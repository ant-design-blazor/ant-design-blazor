using System;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign.Docs.Shared
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        private bool _drawerVisible = false;

        public string CurrentLanguage => LanguageService.CurrentCulture.Name;

        private bool _isMobile;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILanguageService LanguageService { get; set; }

        [Inject]
        public DemoService DemoService { get; set; }

        internal PrevNextNav PrevNextNav { get; set; }

        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();
            await DemoService.InitializeDemos();

            NavigationManager.LocationChanged += OnLocationChanged;
        }

        public async Task ChangePrevNextNav(string currentTitle)
        {
            if (string.IsNullOrWhiteSpace(currentTitle))
                return;

            var currentSubmenuUrl = DemoService.GetCurrentSubMenuUrl();
            var prevNext = await DemoService.GetPrevNextMenu(currentSubmenuUrl, currentTitle);

            PrevNextNav?.SetPrevNextNav(prevNext[0], prevNext[1]);
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
