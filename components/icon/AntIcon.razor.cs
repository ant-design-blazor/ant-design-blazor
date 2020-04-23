using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntBlazor
{
    public partial class AntIcon : AntDomComponentBase
    {
        [Parameter]
        public bool Spin { get; set; }

        [Parameter]
        public OneOf<int, string> Rotate { get; set; } = 0;

        [Parameter]
        public string Type { get; set; }

        /// <summary>
        /// 'fill' | 'outline' | 'twotone';
        /// </summary>
        [Parameter]
        public string Theme { get; set; } = AntIconThemeType.Outline;

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

        [Parameter]
        public string TabIndex { get; set; }

        [CascadingParameter]
        public AntButton Button { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> Onclick { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private static readonly ConcurrentDictionary<string, string> s_svgCache = new ConcurrentDictionary<string, string>();

        private string SvgImg { get; set; }
        private string SvgStyle { get; set; }
        private string _iconSvg;

        private Uri _baseUrl;

        public void SetClassSet()
        {
            string prefixName = "anticon";
            ClassMapper.Add(prefixName)
                .If($"{prefixName}-spin", () => Spin || this.Type == "loading");

            SvgStyle = $"focusable=\"false\" width=\"{Width}\" height=\"{Height}\" fill=\"{Fill}\"";
        }

        protected override async Task OnInitializedAsync()
        {
            this.SetClassSet();

            _baseUrl = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);

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
            try
            {
                if (s_svgCache.TryGetValue($"{Theme}-{Type}", out var svg))
                {
                    _iconSvg = svg;
                }
                else
                {
                    _iconSvg = await HttpClient.GetStringAsync(new Uri(_baseUrl, $"_content/AntBlazor/icons/{Theme.ToLower()}/{Type.ToLower()}.svg"));
                    s_svgCache.TryAdd($"{Theme}-{Type}", _iconSvg);
                }

                SvgImg = _iconSvg.Insert(_iconSvg.IndexOf("svg", StringComparison.Ordinal) + 3, $" {SvgStyle} ");
            }
            catch
            {
                // ignored
            }

            StateHasChanged();
        }

        public async Task OnClick(MouseEventArgs args)
        {
            if (Onclick.HasDelegate)
            {
                await Onclick.InvokeAsync(args);
            }
        }
    }
}
