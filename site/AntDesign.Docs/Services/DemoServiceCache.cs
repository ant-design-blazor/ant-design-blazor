// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign.Docs.Utils;

namespace AntDesign.Docs.Services
{
    public class DemoServiceCache
    {
        private readonly ConcurrentCache<string, AsyncLazy<IDictionary<string, DemoComponent>>> _componentCache = new();
        private readonly ConcurrentCache<string, AsyncLazy<DemoMenuItem[]>> _menuCache = new();
        private readonly ConcurrentCache<string, AsyncLazy<DemoMenuItem[]>> _demoMenuCache = new();
        private readonly ConcurrentCache<string, AsyncLazy<DemoMenuItem[]>> _docMenuCache = new();

        private readonly HttpClient _httpClient;

        public DemoServiceCache(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void PreFetch(string language)
        {
            PreFetchDemoMenuItems(_menuCache, language, lang => $"_content/AntDesign.Docs/meta/menu.{lang}.json");
            PreFetchDemoMenuItems(_docMenuCache, language, lang => $"_content/AntDesign.Docs/meta/docs.{lang}.json");
            PreFetchComponent(language);
        }

        private void PreFetchComponent(string language)
        {
            _componentCache.GetOrAdd(language, (lang) => new(async () =>
            {
                var components = await _httpClient.GetFromJsonAsync<DemoComponent[]>($"_content/AntDesign.Docs/meta/components.{lang}.json");
                return components.ToDictionary(x => $"{x.Category.ToLower()}/{x.Title.ToLower()}", x => x);
            }));
        }

        private void PreFetchDemoMenuItems(ConcurrentCache<string, AsyncLazy<DemoMenuItem[]>> cache, string language, Func<string, string> srcUrl)
        {
            cache.GetOrAdd(language, (lang) => new(async () =>
            {
                var items = await _httpClient.GetFromJsonAsync<DemoMenuItem[]>(srcUrl(lang));
                return items;
            }));
        }

        public async Task<DemoMenuItem[]> GetMenuAsync(string language)
        {
            return _menuCache.TryGetValue(language, out var menuItems) ? await menuItems : Array.Empty<DemoMenuItem>();
        }

        public async Task<DemoMenuItem[]> GetDemoMenuAsync(string language)
        {
            return _demoMenuCache.TryGetValue(language, out var menuItems) ? await menuItems : Array.Empty<DemoMenuItem>();
        }

        public async Task<DemoMenuItem[]> GetDocMenuAsync(string language)
        {
            return _docMenuCache.TryGetValue(language, out var menuItems) ? await menuItems : Array.Empty<DemoMenuItem>();
        }

        public async Task<DemoComponent> GetComponentAsync(string language, string componentName)
        {
            return _componentCache.TryGetValue(language, out var component)
            ? (await component).TryGetValue(componentName.ToLower(), out var demoComponent) ? demoComponent : null
            : null;
        }
    }
}
