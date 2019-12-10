using System;
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

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Inject]
        private HttpClient httpClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected string SvgImg { get; set; }
        private string SvgStyle { get; set; }
        private string _iconSvg;

        public AntIconBase()
        {
            string prefixName = "anticon";
            ClassMapper.Add(prefixName)
                .If($"{prefixName}-spin", () => Spin || this.Type == "loading");

            SvgStyle = $"focusable=\"false\" width=\"{Width}\" height=\"{Height}\" fill=\"{Fill}\"";
        }

        protected override async Task OnInitializedAsync()
        {
            var baseUrl = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);

            _iconSvg = await httpClient.GetStringAsync(new Uri(baseUrl, $"/_content/AntBlazor/icons/{Theme}/{Type}.svg".ToLower()));

            base.OnInitializedAsync();
        }

        protected void SetupSvgImg()
        {
            SvgImg = _iconSvg.Insert(_iconSvg.IndexOf("svg") + 3, $" {SvgStyle} ");
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetupSvgImg();
        }
    }
}