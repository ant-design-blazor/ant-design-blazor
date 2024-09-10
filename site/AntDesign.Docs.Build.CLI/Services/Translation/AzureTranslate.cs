// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    public class AzureTranslate : ITranslate
    {
        private readonly HttpClient _client;

        private readonly IOptionsSnapshot<AzureTranslateOptions> _options;

        public AzureTranslate(HttpClient client, IOptionsSnapshot<AzureTranslateOptions> options)
        {
            _client = client;
            _options = options;
        }

        public Task BackupTranslations(string lang, bool onlyKeepUsed = true)
        {
            throw new NotImplementedException("Not the best design, but this class doesn't implement this. It is meant for the cache wrapper class.");
        }

        public async Task<string?> TranslateText(string component, string text, string to, string from = "auto")
        {
            if (string.IsNullOrWhiteSpace(_options.Value.Key) || string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            using var request = new HttpRequestMessage();

            request.Headers.Add("Ocp-Apim-Subscription-Key", _options.Value.Key);

            if (!string.IsNullOrWhiteSpace(_options.Value.Region))
            {
                request.Headers.Add("Ocp-Apim-Subscription-Region", _options.Value.Region);
            }

            request.Method = HttpMethod.Post;
            request.Content = RequestContent(text);
            request.RequestUri = BuildRequestUri(from, to);

            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new TranslationTooManyRequestsException();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialized = JsonSerializer.Deserialize<IEnumerable<AzureTranslationResponse>>(
                responseContent, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserialized?.SingleOrDefault()?.Translations.SingleOrDefault()?.Text;
        }

        private Uri BuildRequestUri(string from, string to)
        {
            var url = _client.BaseAddress + $"translate?api-version=3.0&to={to}";
            if (from != "auto")
            {
                url += $"&from={from}";
            }

            return new Uri(url);
        }

        private static HttpContent RequestContent(string text)
        {
            var body = new object[] {
                new {
                    Text = text
                }
            };

            return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        }
    }
}
