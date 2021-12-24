using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

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
                    _avatarList = Array.Empty<AvatarInfo>();
                }
            }
        }

        [Parameter]
        public List<string> FilePaths
        {
            get => _filePaths;
            set
            {
                if (_filePaths == null || value?.SequenceEqual(_filePaths) != true)
                {
                    _filePaths = value;
                    _waitForRefresh = true;
                    _avatarList = Array.Empty<AvatarInfo>();
                }
            }
        }

        [Inject] public HttpClient HttpClient { get; set; }

        [Inject] public NavigationManager Navigation { get; set; }

        [Inject] private ILanguageService LanguageService { get; set; }

        private AvatarInfo[] _avatarList;
        private List<string> _filePaths;
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
            if (_waitForRefresh)
            {
                _waitForRefresh = false;
                await GetContributors();
            }
        }

        private async Task GetContributors()
        {
            if (FilePath == null && FilePaths?.Any() != true)
                return;
#if DEBUG
            _avatarList = new AvatarInfo[] { new AvatarInfo() { Username = "ElderJames", Url = "https://avatars.githubusercontent.com/u/7550366?s=40&v=4" } };
#else
            if (FilePath != null)
            {
                _avatarList = await HttpClient.GetFromJsonAsync<AvatarInfo[]>($"https://proapi.azurewebsites.net/doc/getAvatarList?filename={FilePath}&owner=ant-design-blazor&repo=ant-design-blazor");
            }
            else
            {
                var taskList = new List<Task<AvatarInfo[]>>();
                foreach (var filePath in FilePaths)
                {
                    taskList.Add(HttpClient.GetFromJsonAsync<AvatarInfo[]>($"https://proapi.azurewebsites.net/doc/getAvatarList?filename={filePath}&owner=ant-design-blazor&repo=ant-design-blazor"));
                }
                await Task.WhenAll(taskList);
                _avatarList = taskList.Select(x => x.Result.AsEnumerable()).Aggregate((x, y) => x.Union(y)).ToArray();
            }
#endif
            StateHasChanged();
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }

        public struct AvatarInfo
        {
            public string Username { get; set; }
            public string Url { get; set; }
            public string ProfileUrl => $"https://github.com/{Username}";
        }
    }
}
