using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Docs.Highlight;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using AntDesign.Docs.Shared;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Pages
{
    public partial class Docs : ComponentBase
    {
        [Parameter] public string FileName { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        private DocsFile _file;

        private bool _waitingHighlight = false;

        private string CurrentLanguage => LanguageService.CurrentCulture.Name;

        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ILanguageService LanguageService { get; set; }
        [Inject] private DemoService DemoService { get; set; }
        [Inject] private IPrismHighlighter PrismHighlighter { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            NavigationManager.LocationChanged += async (sender, args) =>
            {
                await SetDocUrl();
                await InvokeAsync(StateHasChanged);
            };

            if (string.IsNullOrWhiteSpace(FileName))
            {
                var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
                var newUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
                var menus = await DemoService.GetMenuAsync();
                var current = menus.FirstOrDefault(x => x.Url == newUrl);
                if (current != null)
                {
                    NavigationManager.NavigateTo($"{CurrentLanguage}/{current.Children[0].Url}");
                }
            }
        }

        private async ValueTask SetDocUrl()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                var baseUrl = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);
                var docUrl = new Uri(baseUrl, $"_content/AntDesign.Docs/docs/{FileName}.{CurrentLanguage}.json").ToString();
                _file = await HttpClient.GetFromJsonAsync<DocsFile>(docUrl);
                _waitingHighlight = true;

                await MainLayout.ChangePrevNextNav(FileName);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await SetDocUrl();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_waitingHighlight)
            {
                _waitingHighlight = false;
                await PrismHighlighter.HighlightAllAsync();
            }
        }
    }
}
