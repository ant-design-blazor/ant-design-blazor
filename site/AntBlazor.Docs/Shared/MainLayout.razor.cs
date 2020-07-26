using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        private bool _isCollapsed = true;

        private string CurrentLanguage => LanguageService.CurrentCulture.Name;

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

            NavigationManager.LocationChanged += (_, args) =>
            {
                StateHasChanged();
            };
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
            foreach (var item in prevNext)
            {
                if (item != null)
                {
                    item.Url = $"{CurrentLanguage}/{currentSubmenuUrl}/{item.Title.ToLowerInvariant()}";
                }
            }
            PrevNextNav?.SetPrevNextNav(prevNext[0], prevNext[1]);
        }

        private async void OnLanguageChanged(object sender, CultureInfo culture)
        {
            await JsInterop.InvokeVoidAsync("window.AntDesign.DocSearch.localeChange", culture.Name);
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            LanguageService.LanguageChanged -= OnLanguageChanged;
        }
    }
}
