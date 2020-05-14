using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class IconService
    {
        private static readonly ConcurrentDictionary<string, string> _svgCache = new ConcurrentDictionary<string, string>();
        private readonly HttpClient _httpClient;

        public IconService(HttpClient httpClient, NavigationManager navigationManager)
        {
#pragma warning disable CA1062
            httpClient.BaseAddress = new Uri(navigationManager.BaseUri);
#pragma warning restore CA1062
            _httpClient = httpClient;
        }

        public async ValueTask<string> GetIconImg(string type, string theme)
        {
            var iconImg = string.Empty;
            if (_svgCache.TryGetValue($"{theme}-{type}", out var img))
            {
                iconImg = img;
            }
            else
            {
                var res = await _httpClient.GetAsync($"_content/AntBlazor/icons/{theme}/{type}.svg");
                if (res.IsSuccessStatusCode)
                {
                    iconImg = await res.Content.ReadAsStringAsync();
                    _svgCache.TryAdd($"{theme}-{type}", iconImg);
                }
            }
            return iconImg;
        }

        public async ValueTask<string> GetIconSvg(string type, string theme, string width = "1em", string height = "1em", string fill = "currentColor", bool spin = false)
        {
            var svgStyle = $"focusable=\"false\" width=\"{width}\" height=\"{height}\" fill=\"{fill}\"";

            var svgImg = await GetIconImg(type, theme);
            if (!string.IsNullOrEmpty(svgImg))
            {
                return svgImg.Insert(svgImg.IndexOf("svg", StringComparison.Ordinal) + 3, $" {svgStyle} ");
            }

            return svgImg;
        }
    }
}
