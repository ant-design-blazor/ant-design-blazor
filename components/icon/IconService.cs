using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AntDesign.core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public class IconService
    {
        private static readonly ConcurrentDictionary<string, ValueTask<string>> _svgCache = new ConcurrentDictionary<string, ValueTask<string>>();
        private static IDictionary<string, string[]> _iconfiles;
        private readonly static HttpClient _httpClient = new HttpClient();
        private IJSRuntime _js;

        private Uri _baseAddress;

        public IconService(NavigationManager navigationManager, IJSRuntime js)
        {
            if (navigationManager != null)
                _baseAddress = new Uri(navigationManager.BaseUri);

            _js = js;
        }

        public async ValueTask<string> GetIconImg(string type, string theme)
        {
            if (!await IconExists(theme, type))
            {
                return null;
            }

            var cacheKey = $"{theme}/{type}";
            var iconImg = await _svgCache.GetOrAdd(cacheKey, async key =>
            {
                var res = await _httpClient.GetAsync(new Uri(_baseAddress, $"_content/AntDesign/icons/{key}.svg"));
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsStringAsync();
                }

                return null;
            });

            return iconImg;
        }

        public static string GetStyledSvg(string svgImg, string width = "1em", string height = "1em", string fill = "currentColor", int rotate = 0)
        {
            if (!string.IsNullOrEmpty(svgImg))
            {
                var svgStyle = $"focusable=\"false\" width=\"{width}\" height=\"{height}\" fill=\"{fill}\" {(rotate == 0 ? "" : $"style=\"transform: rotate({rotate}deg);\"")}";
                return svgImg.Insert(svgImg.IndexOf("<svg", StringComparison.Ordinal) + 4, $" {svgStyle} ");
            }

            return null;
        }

        public async ValueTask CreateFromIconfontCN(string scriptUrl)
        {
            if (string.IsNullOrEmpty(scriptUrl))
            {
                return;
            }

            await _js.InvokeVoidAsync(JSInteropConstants.CreateIconFromfontCN, scriptUrl);
        }

        public async Task<IDictionary<string, string[]>> GetAllIcons()
        {
            _iconfiles ??= await _httpClient.GetFromJsonAsync<IDictionary<string, string[]>>(new Uri(_baseAddress, $"_content/AntDesign/icons/icons.json").ToString());
            return _iconfiles;
        }

        public async ValueTask<bool> IconExists(string theme, string type)
        {
            _iconfiles ??= await GetAllIcons();

            if (!_iconfiles.TryGetValue(theme, out var files))
            {
                return false;
            }

            return type.IsIn(files);
        }
    }
}
