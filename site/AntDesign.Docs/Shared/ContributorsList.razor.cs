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
            if (FilePaths?.Any() != true)
                return;
            var taskList = new List<Task<AvatarInfo[]>>();
            foreach (var filePath in FilePaths)
            {
                taskList.Add(HttpClient.GetFromJsonAsync<AvatarInfo[]>($"https://proapi.azurewebsites.net/doc/getAvatarList?filename={filePath}&owner=ant-design-blazor&repo=ant-design-blazor"));
            }
            await Task.WhenAll(taskList);
            _avatarList = taskList.SelectMany(x => x.Result).Distinct().ToArray();
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
