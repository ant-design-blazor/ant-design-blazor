// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Services
{
    public class DemoService
    {
        private static ConcurrentCache<string, Type> _showCaseCache;

        private readonly ILocalizationService _localizationService;
        private readonly HttpClient _httpClient;
        private readonly DemoServiceCache _cache;
        private readonly NavigationManager _navigationManager;
        private Uri _baseUrl;

        private string CurrentLanguage => _localizationService.CurrentCulture.Name;

        public DemoService(ILocalizationService localizationService, HttpClient httpClient, DemoServiceCache cache, NavigationManager navigationManager)
        {
            _localizationService = localizationService;
            _httpClient = httpClient;
            _cache = cache;
            _navigationManager = navigationManager;
            _baseUrl = _navigationManager.ToAbsoluteUri(_navigationManager.BaseUri);
            Initialize(localizationService.CurrentCulture.Name);

            _localizationService.LanguageChanged += (sender, args) => Initialize(args.Name);
        }

        private void Initialize(string language)
        {
            _cache.PreFetch(language);
        }

        //public async Task InitializeDemos()
        //{
        //    _showCaseCache ??= new ConcurrentCache<string, RenderFragment>();
        //    var demoTypes = await _httpClient.GetFromJsonAsync<string[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/demoTypes.json").ToString());
        //    foreach (var type in demoTypes)
        //    {
        //        GetShowCase(type);
        //    }
        //}

        public async Task<DemoComponent> GetComponentAsync(string componentName)
        {
            return await _cache.GetComponentAsync(CurrentLanguage, componentName);
        }

        public async Task<DemoMenuItem[]> GetMenuAsync()
        {
            return await _cache.GetMenuAsync(CurrentLanguage);
        }

        public async ValueTask<DemoMenuItem[]> GetCurrentMenuItems()
        {
            var menuItems = await GetMenuAsync();
            var currentSubmenuUrl = GetCurrentSubMenuUrl();
            return menuItems.FirstOrDefault(x => x.Url == currentSubmenuUrl)?.Children ?? Array.Empty<DemoMenuItem>();
        }

        public async ValueTask<DemoMenuItem[]> GetSearchedMenuItems(string searchText)
        {
            var menuItems = await GetCurrentMenuItems();
            if (!string.IsNullOrEmpty(searchText))
            {
                // Filtering itemGroups in a way that only if itemGroup has at least one Children with a title Or subTitle that contains searchText
                menuItems = menuItems
                    .Where(x => x.Children != null && x.Children.Any(c => 
                        c.Title.ToLower().Contains(searchText) ||
                        (!string.IsNullOrEmpty(c.SubTitle) && c.SubTitle.ToLower().Contains(searchText))
                    ))
                    .Select(x => new DemoMenuItem
                    {
                        Order = x.Order,
                        SubTitle = x.SubTitle,
                        Type = x.Type,
                        Cover = x.Cover,
                        Url = x.Url,
                        Title = x.Title,
                        Children = x.Children?.Where(c =>
                            string.IsNullOrEmpty(searchText) ||
                            c.Title.ToLower().Contains(searchText) ||
                            (!string.IsNullOrEmpty(c.SubTitle) && c.SubTitle.ToLower().Contains(searchText)) 
                        ).ToArray()
                    })
                    .ToArray();
            }
            return menuItems;
        }

        public string GetCurrentSubMenuUrl()
        {
            var currentUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
            var originalUrl = currentUrl.IndexOf('/') > 0 ? currentUrl.Substring(currentUrl.IndexOf('/') + 1) : currentUrl;
            return string.IsNullOrEmpty(originalUrl) ? "/" : originalUrl.Split('/')[0];
        }

        public Type GetShowCase(string type)
        {
            _showCaseCache ??= new();
            return _showCaseCache.GetOrAdd(type, t =>
            {
                var showCase = Type.GetType($"{Assembly.GetExecutingAssembly().GetName().Name}.{t}") ?? typeof(Template);
                return showCase;
            });
        }

        public async Task<DemoMenuItem[]> GetPrevNextMenu(string type, string currentTitle)
        {
            var items = Array.Empty<DemoMenuItem>();

            if (type.ToLowerInvariant() == "docs")
            {
                items = (await _cache.GetDocMenuAsync(CurrentLanguage)).OrderBy(x => x.Order).ToArray();
                currentTitle = $"docs/{currentTitle}";
            }
            else
            {
                items = (await _cache.GetDemoMenuAsync(CurrentLanguage))
                .OrderBy(x => x.Order)
                .SelectMany(x => x.Children)
                .ToArray();

                currentTitle = $"components/{currentTitle}";
            }

            for (var i = 0; i < items.Length; i++)
            {
                if (currentTitle.Equals(items[i].Url, StringComparison.InvariantCultureIgnoreCase))
                {
                    var prev = i == 0 ? null : items[i - 1];
                    var next = i == items.Length - 1 ? null : items[i + 1];
                    return new[] { prev, next };
                }
            }

            return new DemoMenuItem[] { null, null };
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
