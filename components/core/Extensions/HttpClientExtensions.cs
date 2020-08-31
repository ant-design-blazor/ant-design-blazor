using System;
using System.Net.Http;
using System.Text.Json;
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

            var res = await client.GetAsync(requestUri, cancellationToken);
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<TValue>(json, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                }, cancellationToken);
            }

            return default;
        }
    }
}
