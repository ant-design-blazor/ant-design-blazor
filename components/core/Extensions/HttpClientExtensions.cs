using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AntDesign.core.Extensions
{
    internal static class HttpClientExtensions
    {
        public static async Task<TValue> GetFromJsonAsync<TValue>(this HttpClient client, string requestUri, CancellationToken cancellationToken = default)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var res = await client.GetAsync(requestUri);
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStreamAsync();
                return await System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(json);
            }

            return default;
        }
    }
}
