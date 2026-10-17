// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Services
{
    public class DemoService
    {
        private readonly ILocalizationService _localizationService;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private Uri _baseUrl;

        private string CurrentLanguage => _localizationService.CurrentCulture.Name;

        public DemoService(ILocalizationService localizationService, HttpClient httpClient, NavigationManager navigationManager)
        {
            _localizationService = localizationService;
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _baseUrl = _navigationManager.ToAbsoluteUri(_navigationManager.BaseUri);
        }

        public Task<DemoComponent> GetComponentAsync(string componentName) =>
            Task.FromResult(GeneratedDocsMetadata.GetComponent(CurrentLanguage, componentName));

        public Task<DemoMenuItem[]> GetMenuAsync() => Task.FromResult(GeneratedDocsMetadata.GetMenu(CurrentLanguage));

        public async ValueTask<DemoMenuItem[]> GetCurrentMenuItems()
        {
            var menuItems = await GetMenuAsync();
            var currentSubmenuUrl = GetCurrentSubMenuUrl();
            return menuItems.FirstOrDefault(x => x.Url == currentSubmenuUrl)?.Children ?? Array.Empty<DemoMenuItem>();
        }

        public string GetCurrentSubMenuUrl()
        {
            var currentUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
            var originalUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
            return string.IsNullOrEmpty(originalUrl) ? "/" : originalUrl.Split('/')[0];
        }

        public Type GetShowCase(string type)
        {
            return GeneratedDocsMetadata.GetDemoType(type) ?? typeof(Template);
        }

        public Task<DemoMenuItem[]> GetPrevNextMenu(string type, string currentTitle)
        {
            DemoMenuItem[] items;

            if (type.ToLowerInvariant() == "docs")
            {
                items = GeneratedDocsMetadata.GetDocsMenu(CurrentLanguage).OrderBy(x => x.Order).ToArray();
                currentTitle = $"docs/{currentTitle}";
            }
            else
            {
                items = GeneratedDocsMetadata.GetDemoMenu(CurrentLanguage)
                    .OrderBy(x => x.Order)
                    .SelectMany(x => x.Children ?? Array.Empty<DemoMenuItem>())
                    .ToArray();

                currentTitle = $"components/{currentTitle}";
            }

            for (var i = 0; i < items.Length; i++)
            {
                if (currentTitle.Equals(items[i].Url, StringComparison.InvariantCultureIgnoreCase))
                {
                    var prev = i == 0 ? null : items[i - 1];
                    var next = i == items.Length - 1 ? null : items[i + 1];
                    return Task.FromResult(new[] { prev, next });
                }
            }

            return Task.FromResult(new DemoMenuItem[] { null, null });
        }

        public async Task<Recommend[]> GetRecommend()
        {
            return await _httpClient.GetFromJsonAsync<Recommend[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/data/recommend.{CurrentLanguage}.json").ToString());
        }

        public async Task<Product[]> GetProduct()
        {
            return await _httpClient.GetFromJsonAsync<Product[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/data/products.json").ToString());
        }

        public async Task<MoreProps[]> GetMore()
        {
            return await _httpClient.GetFromJsonAsync<MoreProps[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/data/more-list.{CurrentLanguage}.json").ToString());
        }

        public async Task<Sponsor[]> GetSponsors()
        {
            return await _httpClient.GetFromJsonAsync<Sponsor[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/data/sponsors.{CurrentLanguage}.json").ToString());
        }
    }
}
