// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
    Semantic vector graphics. Before use icons。
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.General, "https://gw.alipayobjects.com/zos/alicdn/rrwbSt3FQ/Icon.svg", Title = "Icon",SubTitle = "图标")]
    public partial class Icon : AntDomComponentBase
    {
        /// <summary>
        /// Alternative text for the icon
        /// </summary>
        [Parameter]
        public string Alt { get; set; }

        /// <summary>
        /// Rotate icon with animation
        /// </summary>
        /// <default value="img" />
        [Parameter]
        public string Role { get; set; } = "img";

        /// <summary>
        /// Sets the value of the aria-label attribute
        /// </summary>
        [Parameter]
        public string AriaLabel { get; set; }

        /// <summary>
        /// Rotate icon with animation
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Spin { get; set; }

        /// <summary>
        /// Rotate by n degrees - does not work in IE9
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int Rotate { get; set; } = 0;

        /// <summary>
        /// Icon name to show
        /// </summary>
        [Parameter]
        public string Type { get; set; }

        /// <summary>
        /// Which theme of icon - 'fill' | 'outline' | 'twotone';
        /// </summary>
        /// <default value="IconThemeType.Outline" />
        [Parameter]
        public string Theme { get; set; } = IconThemeType.Outline;

        /// <summary>
        /// Specify the primary color when using the TwoTone theme. Other themes ignore this parameter.
        /// </summary>
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

        /// <summary>
        /// The type of <see cref="AntDesign.IconFont" />
        /// </summary>
        [Parameter]
        public string IconFont { get; set; }

        /// <summary>
        /// Width of the icon
        /// </summary>
        /// <default value="1em" />
        [Parameter]
        public string Width { get; set; } = "1em";

        /// <summary>
        /// Height of the icon
        /// </summary>
        /// <default value="1em" />
        [Parameter]
        public string Height { get; set; } = "1em";

        /// <summary>
        /// Fill value for the icon's SVG
        /// </summary>
        /// <default value="currentColor" />
        [Parameter]
        public string Fill { get; set; } = "currentColor";

        /// <summary>
        /// Tabindex for the wrapping span
        /// </summary>
        [Parameter]
        public string TabIndex { get; set; }

        /// <summary>
        /// Stop propagation of the click event on the icon
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool StopPropagation { get; set; }

        /// <summary>
        /// OnClick event for the icon
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// The component to render as a custom icon
        /// </summary>
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
            ClassMapper.Add($"anticon")
                .GetIf(() => $"anticon-{Type}", () => !string.IsNullOrWhiteSpace(Type));

            if (OnClick.HasDelegate)
            {
                _attributes.Add("onclick", (Delegate)HandleOnClick);
            }

            _attributes.Add("aria-label", AriaLabel);

            if (String.IsNullOrEmpty(Role))
            {
                _attributes.Add("aria-hidden", "true");
            }
            else
            {
                _attributes.Add("role", Role);
            }

            if (Role == "img")
            {
                _attributes.Add("alt", Alt);
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
            if (Theme == IconThemeType.Twotone && (firstRender || _twotoneColorChanged))
            {
                _twotoneColorChanged = false;
                await ChangeTwoToneColor();

                SetupSvgImg();

                await InvokeAsync(StateHasChanged);
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
    }
}
