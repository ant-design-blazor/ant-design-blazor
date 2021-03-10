using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using AntDesign.Docs.Localization;

namespace AntDesign.Docs.Shared
{
    public partial class ContributorsList : ComponentBase, IDisposable
    {
        [Parameter]
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    _waitForRefresh = true;
                }
            }
        }

        [Inject] public HttpClient HttpClient { get; set; }

        [Inject] public NavigationManager Navigation { get; set; }

        [Inject] private ILanguageService LanguageService { get; set; }

        private AvatarInfo[] _avatarList;
        private string _filePath;
        private bool _waitForRefresh;

        protected override async Task OnInitializedAsync()
        {
            await GetContributors();
            await base.OnInitializedAsync();

            Navigation.LocationChanged += OnLocationChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (_waitForRefresh)
            {
                _waitForRefresh = false;
                await GetContributors();
            }
        }

        private async void OnLocationChanged(object _, LocationChangedEventArgs e)
        {
            await GetContributors();
        }

        private async Task GetContributors()
        {
            if (FilePath == null)
                return;

            _avatarList = await HttpClient.GetFromJsonAsync<AvatarInfo[]>($"https://proapi.azurewebsites.net/doc/getAvatarList?filename={FilePath}&owner=ant-design-blazor&repo=ant-design-blazor");

            StateHasChanged();
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }

        public class AvatarInfo
        {
            public string Username { get; set; }
            public string Url { get; set; }
            public string ProfileUrl => $"https://github.com/{Username}";
        }
    }
}
