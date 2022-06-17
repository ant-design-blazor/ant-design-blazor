﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Icon : AntDomComponentBase
    {
        [Parameter]
        public string Alt { get; set; }

        [Parameter]
        public bool Spin { get; set; }

        [Parameter]
        public int Rotate { get; set; } = 0;

        [Parameter]
        public string Type { get; set; }

        /// <summary>
        /// 'fill' | 'outline' | 'twotone';
        /// </summary>
        [Parameter]
        public string Theme { get; set; } = IconThemeType.Outline;

        [Parameter]
        public string TwotoneColor { get; set; } = "#1890ff";

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

        [Parameter]
        public bool StopPropagation { get; set; }

        [CascadingParameter]
        public Button Button { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Parameter]
        public RenderFragment Component { get; set; }

        [Inject]
        public IconService IconService { get; set; }

        protected string _svgImg;

        protected override void Dispose(bool disposing)
        {
            Button?.Icons.Remove(this);

            base.Dispose(disposing);
        }

        protected override async Task OnInitializedAsync()
        {
            if (Type == "loading")
            {
                Spin = true;
            }

            await SetupSvgImg();

            Button?.Icons.Add(this);

            ClassMapper.Add($"anticon")
                .GetIf(() => $"anticon-{Type}", () => !string.IsNullOrWhiteSpace(Type));

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetupSvgImg(true);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task SetupSvgImg(bool rendered = false)
        {
            if (Component != null)
            {
                return;
            }

            string svgClass = Spin ? "anticon-spin" : null;

            var svg = !string.IsNullOrEmpty(IconFont) ?
                $"<svg><use xlink:href=#{IconFont} /></svg>"
                : IconService.GetIconImg(Type.ToLowerInvariant(), Theme.ToLowerInvariant());

            _svgImg = IconService.GetStyledSvg(svg, svgClass, Width, Height, Fill, Rotate);

            if (rendered && Theme == IconThemeType.Twotone)
            {
                _svgImg = await IconService.GetTwotoneSvgIcon(_svgImg, TwotoneColor);
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task HandleOnClick(MouseEventArgs args)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }
        }
    }
}
