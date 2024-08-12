using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using AntDesign.Docs.Utils;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign.Docs.Services
{
    public class DemoService
    {
        private static ConcurrentCache<string, AsyncLazy<IDictionary<string, DemoComponent>>> _componentCache;
        private static ConcurrentCache<string, AsyncLazy<DemoMenuItem[]>> _menuCache;
        private static ConcurrentCache<string, AsyncLazy<DemoMenuItem[]>> _demoMenuCache;
        private static ConcurrentCache<string, AsyncLazy<DemoMenuItem[]>> _docMenuCache;
        private static ConcurrentCache<string, Type> _showCaseCache;

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
            Initialize(localizationService.CurrentCulture.Name);

            _localizationService.LanguageChanged += (sender, args) => Initialize(args.Name);
        }

        private void Initialize(string language)
        {
            _menuCache ??= new();
            _menuCache.GetOrAdd(language, (currentLanguage) => new(async () =>
            {
                var menuItems = await _httpClient.GetFromJsonAsync<DemoMenuItem[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/menu.{language}.json").ToString());
                return menuItems;
            }));

            _componentCache ??= new();
            _componentCache.GetOrAdd(language, (currentLanguage) => new(async () =>
            {
                var components = await _httpClient.GetFromJsonAsync<DemoComponent[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/components.{language}.json").ToString());
                return components.ToDictionary(x => $"{x.Category.ToLower()}/{x.Title.ToLower()}", x => x);
            }));

            _demoMenuCache ??= new();
            _demoMenuCache.GetOrAdd(language, (currentLanguage) => new(async () =>
            {
                var menuItems = await _httpClient.GetFromJsonAsync<DemoMenuItem[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/demos.{language}.json").ToString());
                return menuItems;
            }));

            _docMenuCache ??= new();
            _docMenuCache.GetOrAdd(language, (currentLanguage) => new(async () =>
            {
                var menuItems = await _httpClient.GetFromJsonAsync<DemoMenuItem[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/docs.{language}.json").ToString());
                return menuItems;
            }));
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
            return _componentCache.TryGetValue(CurrentLanguage, out var component)
                ? (await component).TryGetValue(componentName.ToLower(), out var demoComponent) ? demoComponent : null
                : null;
        }

        public async Task<DemoMenuItem[]> GetMenuAsync()
        {
            return _menuCache.TryGetValue(CurrentLanguage, out var menuItems) ? await menuItems : Array.Empty<DemoMenuItem>();
        }

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
                items = _docMenuCache.TryGetValue(CurrentLanguage, out var menuItems) ? (await menuItems).OrderBy(x => x.Order).ToArray() : Array.Empty<DemoMenuItem>();
                currentTitle = $"docs/{currentTitle}";
            }
            else
            {
                items = _demoMenuCache.TryGetValue(CurrentLanguage, out var menuItems) ? (await menuItems)
                .OrderBy(x => x.Order)
                .SelectMany(x => x.Children)
                .ToArray() : Array.Empty<DemoMenuItem>();

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
