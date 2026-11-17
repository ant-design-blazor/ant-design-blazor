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

        public Task<DemoComponent> GetComponentAsync(string componentName)
        {
            var component = GeneratedDemoIndex.GetComponents(CurrentLanguage).FirstOrDefault(item =>
                string.Equals($"{item.Category}/{item.Title}", componentName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(component);
        }

        public Task<DemoMenuItem[]> GetMenuAsync() => Task.FromResult((CurrentLanguage == "zh-CN" ? Menuzh_CN.Data : Menuen_US.Data).ToArray());

        public static string GetLocalizedUrl(string language, string url)
        {
            var path = (url ?? string.Empty).TrimStart('/');
            if (path.StartsWith("en-US/", StringComparison.OrdinalIgnoreCase)) path = path[6..];
            else if (path.StartsWith("zh-CN/", StringComparison.OrdinalIgnoreCase)) path = path[6..];
            return $"/{language}/{path}";
        }

        public async ValueTask<DemoMenuItem[]> GetCurrentMenuItems()
        {
            var menuItems = await GetMenuAsync();
            var currentSubmenuUrl = GetCurrentSubMenuUrl();
            var currentPrefix = $"/{CurrentLanguage}/{currentSubmenuUrl}";
            return menuItems.FirstOrDefault(x => x.Url.StartsWith(currentPrefix, StringComparison.OrdinalIgnoreCase)
                || (x.Children ?? Array.Empty<DemoMenuItem>()).Any(child => child.Url.StartsWith(currentPrefix, StringComparison.OrdinalIgnoreCase)))?.Children
                ?? Array.Empty<DemoMenuItem>();
        }

        public string GetCurrentSubMenuUrl()
        {
            var currentUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
            var originalUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
            return string.IsNullOrEmpty(originalUrl) ? "/" : originalUrl.Split('/')[0];
        }

        public DemoItem GetDemo(string componentId)
        {
            foreach (var component in GeneratedDemoIndex.GetComponents(CurrentLanguage))
            {
                var demo = component.DemoList?.FirstOrDefault(item =>
                    string.Equals($"components-{component.Title.ToLowerInvariant()}-demo-{item.Name}", componentId, StringComparison.Ordinal));
                if (demo == null) continue;
                return demo;
            }
            return null;
        }

        public Task<DemoMenuItem[]> GetPrevNextMenu(string type, string currentTitle)
        {
            DemoMenuItem[] items;

            if (type.ToLowerInvariant() == "docs")
            {
                items = GetMenuAsync().Result.FirstOrDefault(x => x.Title == (CurrentLanguage == "zh-CN" ? "文档" : "Docs"))?.Children
                    ?.OrderBy(x => x.Order).ToArray() ?? Array.Empty<DemoMenuItem>();
                currentTitle = $"/docs/{currentTitle}";
            }
            else
            {
                items = GetMenuAsync().Result
                    .Where(x => x.Title != (CurrentLanguage == "zh-CN" ? "文档" : "Docs"))
                    .SelectMany(x => x.Children ?? Array.Empty<DemoMenuItem>())
                    .SelectMany(x => x.Children ?? new[] { x })
                    .OrderBy(x => x.Order).ToArray();

                currentTitle = $"/components/{currentTitle}";
            }

            for (var i = 0; i < items.Length; i++)
            {
                if (items[i].Url.EndsWith(currentTitle, StringComparison.InvariantCultureIgnoreCase))
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
