using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

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

        [Inject] private IStringLocalizer Localizer { get; set; }

        private AvatarInfo[] _avatarList;
        private List<string> _filePaths;
        private bool _waitForRefresh;

        protected override void OnInitialized()
        {
            Navigation.LocationChanged += OnLocationChanged;
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _waitForRefresh = true;
                StateHasChanged();
                return;
            }

            if (_waitForRefresh)
            {
                _waitForRefresh = false;
                _ = GetContributors();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void OnLocationChanged(object _, LocationChangedEventArgs e)
        {
            if (_waitForRefresh)
            {
                _waitForRefresh = true;
            }
        }

        private async Task GetContributors()
        {
            if (FilePaths?.Any() != true)
                return;

            var taskList = FilePaths.Select(filePath => HttpClient.GetFromJsonAsync<AvatarInfo[]>($"https://proapi.azurewebsites.net/doc/getAvatarList?filename={filePath}&owner=ant-design-blazor&repo=ant-design-blazor")).ToArray();
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
