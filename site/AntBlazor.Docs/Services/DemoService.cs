using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign.Docs.Services
{
    public class DemoService
    {
        private static ConcurrentCache<string, ValueTask<IDictionary<string, DemoComponent>>> _componentCache;
        private static ConcurrentCache<string, ValueTask<DemoMenuItem[]>> _menuCache;
        private static ConcurrentCache<string, ValueTask<DemoMenuItem[]>> _demoMenuCache;
        private static ConcurrentCache<string, ValueTask<DemoMenuItem[]>> _docMenuCache;
        private static ConcurrentCache<string, RenderFragment> _showCaseCache;

        private readonly ILanguageService _languageService;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private Uri _baseUrl;

        private string CurrentLanguage => _languageService.CurrentCulture.Name;

        public DemoService(ILanguageService languageService, HttpClient httpClient, NavigationManager navigationManager)
        {
            _languageService = languageService;
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _baseUrl = _navigationManager.ToAbsoluteUri(_navigationManager.BaseUri);

            _languageService.LanguageChanged += async (sender, args) => await InitializeAsync(args.Name);
        }

        private async Task InitializeAsync(string language)
        {
            _menuCache ??= new ConcurrentCache<string, ValueTask<DemoMenuItem[]>>();
            await _menuCache.GetOrAdd(language, async (currentLanguage) =>
            {
                var menuItems = await _httpClient.GetFromJsonAsync<DemoMenuItem[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/menu.{language}.json").ToString());

                return menuItems;
            });

            _componentCache ??= new ConcurrentCache<string, ValueTask<IDictionary<string, DemoComponent>>>();
            await _componentCache.GetOrAdd(language, async (currentLanguage) =>
            {
                var components = await _httpClient.GetFromJsonAsync<DemoComponent[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/components.{language}.json").ToString());

                return components.ToDictionary(x => x.Title.ToLower(), x => x);
            });

            _demoMenuCache ??= new ConcurrentCache<string, ValueTask<DemoMenuItem[]>>();
            await _demoMenuCache.GetOrAdd(language, async (currentLanguage) =>
            {
                var menuItems = await _httpClient.GetFromJsonAsync<DemoMenuItem[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/demos.{language}.json").ToString());

                return menuItems;
            });

            _docMenuCache ??= new ConcurrentCache<string, ValueTask<DemoMenuItem[]>>();
            await _docMenuCache.GetOrAdd(language, async (currentLanguage) =>
            {
                var menuItems = await _httpClient.GetFromJsonAsync<DemoMenuItem[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/docs.{language}.json").ToString());
                return menuItems;
            });
        }

        public async Task InitializeDemos()
        {
            _showCaseCache ??= new ConcurrentCache<string, RenderFragment>();
            var demoTypes = await _httpClient.GetFromJsonAsync<string[]>(new Uri(_baseUrl, $"_content/AntDesign.Docs/meta/demoTypes.json").ToString());
            foreach (var type in demoTypes)
            {
                GetShowCase(type);
            }
        }

        public async Task<DemoComponent> GetComponentAsync(string componentName)
        {
            await InitializeAsync(CurrentLanguage);
            return _componentCache.TryGetValue(CurrentLanguage, out var component)
                ? (await component)[componentName.ToLower()]
                : null;
        }

        public async Task<DemoMenuItem[]> GetMenuAsync()
        {
            await InitializeAsync(CurrentLanguage);
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

        public RenderFragment GetShowCase(string type)
        {
            _showCaseCache ??= new ConcurrentCache<string, RenderFragment>();
            return _showCaseCache.GetOrAdd(type, t =>
            {
                var showCase = Type.GetType($"{Assembly.GetExecutingAssembly().GetName().Name}.{type}");
                void ShowCase(RenderTreeBuilder builder)
                {
                    builder.OpenComponent(0, showCase);
                    builder.CloseComponent();
                }

                return ShowCase;
            });
        }

        public async Task<DemoMenuItem[]> GetPrevNextMenu(string type, string currentTitle)
        {
            var cache = type.ToLowerInvariant() == "docs" ? _docMenuCache : _demoMenuCache;

            var items = cache.TryGetValue(CurrentLanguage, out var menuItems) ? (await menuItems)
                .OrderBy(x => x.Order)
                .SelectMany(x => x.Children)
                .ToArray() : Array.Empty<DemoMenuItem>();

            for (var i = 0; i < items.Length; i++)
            {
                if (currentTitle.Equals(items[i].Title, StringComparison.InvariantCultureIgnoreCase))
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
    }
}
