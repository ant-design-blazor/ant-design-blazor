using System;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using OneOf;

namespace AntDesign
{
    public partial class Result : AntDomComponentBase
    {
        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> SubTitle { get; set; }

        [Parameter]
        public RenderFragment Extra { get; set; }

        /// <summary>
        /// success | error | info | warning | 404 | 403 | 500
        /// default: info
        /// </summary>
        [Parameter]
        public string Status { get; set; } = "info";

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public IconService IconService { get; set; }

        private const string PrefixCls = "ant-result";
        private string _svgImage;

        private ClassMapper IconClassMapper { get; set; } = new ClassMapper();

        private RenderFragment BuildIcon => builder =>
        {
            var iconType = DetermineIconType();

            builder.OpenComponent<Icon>(1);
            builder.AddAttribute(2, "Type", iconType.type);
            builder.AddAttribute(2, "Theme", iconType.theme);
            builder.CloseComponent();
        };

        private (string type, string theme) DetermineIconType()
        {
            if (Icon != null)
            {
                var separatorIndex = Icon.LastIndexOf("-", StringComparison.CurrentCultureIgnoreCase);
                var type = Icon.Substring(0, separatorIndex);
                var theme = Icon.Substring(separatorIndex + 1, Icon.Length - separatorIndex - 1);
                return (type, theme);
            }

            if (Status == "error")
                return ("close-circle", "fill");

            if (Status == "success")
                return ("check-circle", "fill");

            if (Status == "warning")
                return ("warning", "fill");

            if (Status == "403")
                return ("unauthorized", "internal");

            if (Status == "404")
                return ("not-found", "internal");

            if (Status == "500")
                return ("bad-request", "internal");

            return ("info-circle", "fill");
        }

        private bool IsImage => Status.IsIn("403", "404", "500");

        private async ValueTask LoadImage()
        {
            if (!IsImage)
                return;

            var iconType = DetermineIconType();

            _svgImage = await IconService.GetIconImg(iconType.type, iconType.theme);
        }

        private void SetClass()
        {
            ClassMapper.Add(PrefixCls)
                .Add($"{PrefixCls}-{Status}");

            IconClassMapper.Add($"{PrefixCls}-icon")
                .If($"{PrefixCls}-image", () => IsImage);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadImage();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClass();
        }
    }
}
