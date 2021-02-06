using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Shared
{
    public partial class GithubButton
    {
        private string _org = "ant-design-blazor";
        private string _repo = "ant-design-blazor";
        private static int _starCount = 0;

        [Parameter]
        public string Responsive { get; set; }

        [Inject] public HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (_starCount > 0)
                    return;

                var res = await HttpClient.GetFromJsonAsync<GithubResponse>($"https://api.github.com/repos/{this._org}/{this._repo}");
                _starCount = res.StargazersCount;
            }
            catch
            {
            }
        }

        private class GithubResponse
        {
            [JsonPropertyName("stargazers_count")]
            public int StargazersCount { get; set; }
        }
    }
}
