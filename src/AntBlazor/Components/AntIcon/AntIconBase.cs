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
        public bool spin { get; set; }

        [Parameter]
        public int rotate { get; set; } = 0;

        [Parameter]
        public string type { get; set; }

        [Parameter]
        public string theme { get; set; } = AntIconThemeType.Outline; //'fill' | 'outline' | 'twotone';

        [Parameter]
        public string twotoneColor { get; set; }

        [Parameter]
        public string iconFont { get; set; }

        [Parameter]
        public string width { get; set; } = "1em";

        [Parameter]
        public string height { get; set; } = "1em";

        [Parameter]
        public string fill { get; set; } = "currentColor";

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

        public void setClassSet()
        {
            string prefixName = "anticon";
            ClassMapper.Add(prefixName)
                .If($"{prefixName}-spin", () => spin || this.type == "loading");

            SvgStyle = $"focusable=\"false\" width=\"{width}\" height=\"{height}\" fill=\"{fill}\"";
        }

        protected override async Task OnInitializedAsync()
        {
            this.setClassSet();

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
            if (SvgCache.TryGetValue($"{theme}-{type}", out var svg))
            {
                _iconSvg = svg;
            }
            else
            {
                _iconSvg = await httpClient.GetStringAsync(new Uri(baseUrl, $"_content/AntBlazor/icons/{theme.ToLower()}/{type.ToLower()}.svg"));
                SvgCache.TryAdd($"{theme}-{type}", _iconSvg);
            }

            SvgImg = _iconSvg.Insert(_iconSvg.IndexOf("svg", StringComparison.Ordinal) + 3, $" {SvgStyle} ");
            StateHasChanged();
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