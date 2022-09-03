using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public string TwotoneColor
        {
            get => _primaryColor;
            set
            {
                if (_primaryColor != value)
                {
                    _primaryColor = value;
                    _twotoneColorChanged = true;
                }
            }
        }

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

        internal string SecondaryColor => _secondaryColor;

        protected string _svgImg;
        private string _primaryColor = "#1890ff";
        private string _secondaryColor = null;
        private bool _twotoneColorChanged = false;

        private Dictionary<string, object> _attributes = new();

        protected override async Task OnInitializedAsync()
        {
            if (Type == "loading")
            {
                Spin = true;
            }

            Button?.Icons.Add(this);

            ClassMapper.Add($"anticon")
                .GetIf(() => $"anticon-{Type}", () => !string.IsNullOrWhiteSpace(Type));

            if (OnClick.HasDelegate)
            {
                _attributes.Add("onclick", (Delegate)HandleOnClick);
            }

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            SetupSvgImg();

            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Theme == IconThemeType.Twotone)
            {
                if (firstRender || _twotoneColorChanged)
                {
                    _twotoneColorChanged = false;
                    await ChangeTwoToneColor();

                    SetupSvgImg();

                    await InvokeAsync(StateHasChanged);
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void SetupSvgImg()
        {
            if (Component != null)
            {
                return;
            }

            _svgImg = IconService.GetStyledSvg(this);
        }

        private async Task ChangeTwoToneColor()
        {
            _secondaryColor = await IconService.GetSecondaryColor(_primaryColor);
        }

        private async Task HandleOnClick(MouseEventArgs args)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Button?.Icons.Remove(this);

            base.Dispose(disposing);
        }
    }
}
