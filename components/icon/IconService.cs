using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public class IconService
    {
        private static readonly ConcurrentDictionary<string, string> _svgCache = new ConcurrentDictionary<string, string>();
        private readonly HttpClient _httpClient;
        private IJSRuntime _js;

        private Uri _baseAddress;

        public IconService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager, IJSRuntime js)
        {
            if (navigationManager != null)
                _baseAddress = new Uri(navigationManager.BaseUri);

            if (httpClientFactory != null)
                _httpClient = httpClientFactory.CreateClient("AntDesign");

            _js = js;
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
                var res = await _httpClient.GetAsync(new Uri(_baseAddress, $"_content/AntDesign/icons/{theme}/{type}.svg"));
                if (res.IsSuccessStatusCode)
                {
                    iconImg = await res.Content.ReadAsStringAsync();
                    _svgCache.TryAdd($"{theme}-{type}", iconImg);
                }
            }
            return iconImg;
        }

        public static string GetStyledSvg(string svgImg, string width = "1em", string height = "1em", string fill = "currentColor", int rotate = 0, bool spin = false)
        {
            var svgStyle = $"focusable=\"false\" width=\"{width}\" height=\"{height}\" fill=\"{fill}\" {(rotate == 0 ? "" : $"style=\"transform: rotate({rotate}deg);\"")}  {(spin ? "class=\"anticon-spin\"" : "")}";
            if (!string.IsNullOrEmpty(svgImg))
            {
                return svgImg.Insert(svgImg.IndexOf("svg", StringComparison.Ordinal) + 3, $" {svgStyle} ");
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
    }
}
