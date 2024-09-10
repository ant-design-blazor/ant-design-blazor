// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    public class GoogleTranslate : ITranslate
    {
        private readonly HttpClient _client;

        public GoogleTranslate(HttpClient client)
        {
            _client = client;
        }

        private static string RequestPath
        {
            get
            {
                var url = "translate_a/single?client=at";
                url += "&dt=t";  // return sentences
                url += "&dt=rm"; // add translit to sentences
                url += "&dj=1";  // result as pretty json instead of deep nested arrays

                return url;
            }
        }

        public Task BackupTranslations(string lang, bool onlyKeepUsed = true)
        {
            throw new NotImplementedException("Not the best design, but this class doesn't implement this. It is meant for the cache wrapper class.");
        }

        public async Task<string?> TranslateText(string component, string text, string to, string from = "auto")
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var content = RequestContent(text, to, from);
            var response = await _client.PostAsync(RequestPath, content);
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new TranslationTooManyRequestsException();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialized = JsonSerializer.Deserialize<GoogleTranslationResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var translations = deserialized?.Sentences.Select(x => x.Trans);
            if (translations is null || !translations.Any())
            {
                return null;
            }

            return string.Join(string.Empty, translations);
        }

        private static HttpContent RequestContent(string text, string to, string from)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("sl", from),
                new KeyValuePair<string, string>("tl", to),
                new KeyValuePair<string, string>("q", text)
            });

            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded;charset=utf-8");

            return content;
        }
    }
}
