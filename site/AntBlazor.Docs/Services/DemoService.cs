using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntBlazor.Docs.Localization;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Docs.Services
{
    public class DemoService
    {
        private static ConcurrentCache<string, ValueTask<IDictionary<string, DemoComponent>>> _componentCache;
        private static ConcurrentCache<string, ValueTask<MenuItem[]>> _menuCache;

        private readonly ILanguageService _languageService;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;

        private string CurrentLanguage => _languageService.CurrentCulture.Name;

        public DemoService(ILanguageService languageService, HttpClient httpClient, NavigationManager navigationManager)
        {
            _languageService = languageService;
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        private async Task InitializeAsync()
        {
            _componentCache ??= new ConcurrentCache<string, ValueTask<IDictionary<string, DemoComponent>>>();
            await _componentCache.GetOrAdd(CurrentLanguage, async (currentLanguage) =>
            {
                var baseUrl = _navigationManager.ToAbsoluteUri(_navigationManager.BaseUri);
                var components = await _httpClient.GetFromJsonAsync<DemoComponent[]>(
                    new Uri(baseUrl, $"_content/AntBlazor.Docs/meta/demo.{CurrentLanguage}.json").ToString());

                return components.ToDictionary(x => x.Title.ToLower(), x => x);
            });

            _menuCache ??= new ConcurrentCache<string, ValueTask<MenuItem[]>>();
            await _menuCache.GetOrAdd(CurrentLanguage, async (currentLanguage) =>
            {
                var baseUrl = _navigationManager.ToAbsoluteUri(_navigationManager.BaseUri);
                var menuItems = await _httpClient.GetFromJsonAsync<MenuItem[]>(
                    new Uri(baseUrl, $"_content/AntBlazor.Docs/meta/menu.{CurrentLanguage}.json").ToString());

                return menuItems;
            });
        }

        public async Task<DemoComponent> GetComponentAsync(string componentName)
        {
            await InitializeAsync();
            return _componentCache.TryGetValue(CurrentLanguage, out var component)
                ? (await component)[componentName.ToLower()]
                : null;
        }

        public async Task<MenuItem[]> GetMenuAsync()
        {
            await InitializeAsync();
            return _menuCache.TryGetValue(CurrentLanguage, out var menuItems) ? await menuItems : Array.Empty<MenuItem>();
        }
    }
}
