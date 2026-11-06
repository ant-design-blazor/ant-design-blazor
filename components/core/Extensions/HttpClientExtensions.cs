// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Core.Helpers;

namespace AntDesign.Core.Extensions;

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
            return JsonSerializer.Deserialize<TValue>(utf8Json, JsonSerializerHelper.DefaultOptions);
        }

        return default;
    }


    /// <summary>
    /// Reads as a binary array and converts to the specified encoding
    /// </summary>
    /// <param name="httpContent"></param>
    /// <param name="dstEncoding">The target encoding</param>
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
    /// Get encoding information from <see cref="HttpContent"/>
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

        var span = charSet.AsSpan().Trim('"');
        if (span.Equals(Encoding.UTF8.WebName, StringComparison.OrdinalIgnoreCase))
        {
            return Encoding.UTF8;
        }

        return Encoding.GetEncoding(span.ToString());
    }
}
