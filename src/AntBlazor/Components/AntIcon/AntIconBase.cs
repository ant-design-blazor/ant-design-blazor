using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public class AntIconBase : AntDomComponentBase
    {
        [Parameter]
        public bool Spin { get; set; }

        [Parameter]
        public int Rotate { get; set; } = 0;

        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public string Theme { get; set; } = AntIconThemeType.Outline; //'fill' | 'outline' | 'twotone';

        [Parameter]
        public string TwotoneColor { get; set; }

        [Parameter]
        public string IconFont { get; set; }

        [Parameter]
        public string Width { get; set; } = "1em";

        [Parameter]
        public string Height { get; set; } = "1em";

        [Parameter]
        public string Fill { get; set; } = "currentColor";

        [CascadingParameter]
        public AntButton Button { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> onclick { get; set; }

        [Inject]
        private HttpClient httpClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private static readonly ConcurrentDictionary<string, string> SvgCache = new ConcurrentDictionary<string, string>();

        protected string SvgImg { get; set; }
        private string SvgStyle { get; set; }
        private string _iconSvg;

        private Uri baseUrl;

        public AntIconBase()
        {
            string prefixName = "anticon";
            ClassMapper.Add(prefixName)
                .If($"{prefixName}-spin", () => Spin || this.Type == "loading");

            SvgStyle = $"focusable=\"false\" width=\"{Width}\" height=\"{Height}\" fill=\"{Fill}\"";
        }

        protected override async Task OnInitializedAsync()
        {
            baseUrl = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);

            if (this is AntIcon icon)
            {
                Button?.Icons.Add(icon);
            }

            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await SetupSvgImg();
            await base.OnParametersSetAsync();
        }

        protected async Task SetupSvgImg()
        {
            if (SvgCache.TryGetValue($"{Theme}-{Type}", out var svg))
            {
                _iconSvg = svg;
            }
            else
            {
                _iconSvg = await httpClient.GetStringAsync(new Uri(baseUrl, $"_content/AntBlazor/icons/{Theme.ToLower()}/{Type.ToLower()}.svg"));
                SvgCache.TryAdd($"{Theme}-{Type}", _iconSvg);
            }

            SvgImg = _iconSvg.Insert(_iconSvg.IndexOf("svg", StringComparison.Ordinal) + 3, $" {SvgStyle} ");
        }

        public async Task OnClick(MouseEventArgs args)
        {
            if (onclick.HasDelegate)
            {
                await onclick.InvokeAsync(args);
            }
        }
    }
}