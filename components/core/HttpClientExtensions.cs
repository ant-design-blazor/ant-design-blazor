using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AntBlazor
{
    public static class HttpClientExtensions
    {
        public static async ValueTask<T> GetJsonAsync<T>(this HttpClient httpClient, string url)
        {
            var s = await httpClient.GetStreamAsync(url);
            return await JsonSerializer.DeserializeAsync<T>(s, options: new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            });
        }

        public static async ValueTask<T> GetJsonAsync<T>(this HttpClient httpClient, Uri url)
        {
            var s = await httpClient.GetStreamAsync(url);
            return await JsonSerializer.DeserializeAsync<T>(s, options: new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            });
        }
    }
}