using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AntDesign.Docs.Services
{
    public class VersionService
    {
        private readonly HttpClient _httpClient;
        private readonly Lazy<Task<string>> _versionTask;

        public VersionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _versionTask = new Lazy<Task<string>>(async () =>
            {
                try
                {
                    var versionInfo = await _httpClient.GetFromJsonAsync<VersionInfo>("https://antblazor.com/_content/AntDesign.Docs/meta/version.json");
                    return versionInfo?.Version ?? "1.0.0";
                }
                catch
                {
                    return "1.0.0";
                }
            });
        }

        public Task<string> GetCurrentVersion()
        {
            return _versionTask.Value;
        }
    }
}
