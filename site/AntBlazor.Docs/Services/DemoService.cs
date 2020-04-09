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
        private static IDictionary<string, IDictionary<string, DemoComponent>> _componentCache;

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
            _componentCache ??= new Dictionary<string, IDictionary<string, DemoComponent>>();
            if (!_componentCache.ContainsKey(CurrentLanguage))
            {
                var baseUrl = _navigationManager.ToAbsoluteUri(_navigationManager.BaseUri);
                var components = await _httpClient.GetFromJsonAsync<DemoComponent[]>(new Uri(baseUrl, $"_content/AntBlazor.Docs/meta/demo_{CurrentLanguage}.json").ToString());
                if (components.Any())
                {
                    _componentCache.Add(CurrentLanguage, components.ToDictionary(x => x.Title, x => x));
                }
            }
        }

        public async Task<DemoComponent> GetComponentAsync(string componentName)
        {
            await InitializeAsync();
            return _componentCache[CurrentLanguage][componentName];
        }

        //public MenuItem[] GetMenuAsync()
        //{
        //}
    }
}