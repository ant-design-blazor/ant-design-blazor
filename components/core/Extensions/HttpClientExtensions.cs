using System;
using System.Net.Http;
using System.Text;
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
                var utf8Json = await res.Content.ReadAsByteArrayAsync(Encoding.UTF8);
                return JsonSerializer.Deserialize<TValue>(utf8Json, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
            }

            return default;
        }


        /// <summary>
        /// 读取为二进制数组并转换为指定的编码
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="dstEncoding">目标编码</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        private static async Task<byte[]> ReadAsByteArrayAsync(this HttpContent httpContent, Encoding dstEncoding)
        {
            var encoding = httpContent.GetEncoding();
            var byteArray = await httpContent.ReadAsByteArrayAsync().ConfigureAwait(false);

            return encoding.Equals(dstEncoding)
                ? byteArray
                : Encoding.Convert(encoding, dstEncoding, byteArray);
        }

        /// <summary>
        /// 获取编码信息
        /// </summary>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        private static Encoding GetEncoding(this HttpContent httpContent)
        {
            var charSet = httpContent.Headers.ContentType?.CharSet;
            if (string.IsNullOrEmpty(charSet) == true)
            {
                return Encoding.UTF8;
            }

            var span = charSet.AsSpan().TrimStart('"').TrimEnd('"');
            if (span.Equals(Encoding.UTF8.WebName, StringComparison.OrdinalIgnoreCase))
            {
                return Encoding.UTF8;
            }

            return Encoding.GetEncoding(span.ToString());
        }
    }
}
