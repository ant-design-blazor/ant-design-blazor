// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.Icons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
    Semantic vector graphics. Before use icons。
    </summary>
    <seealso cref="AntDesign.IconFont"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.General, "https://gw.alipayobjects.com/zos/alicdn/rrwbSt3FQ/Icon.svg", Title = "Icon", SubTitle = "图标")]
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
        public IconThemeType Theme { get; set; } = IconThemeType.Outline;

        /// <summary>
        /// Specify the primary color when using the TwoTone theme. Other themes ignore this parameter.
        /// </summary>
        [Parameter]
        public string TwoToneColor
        {
            get => _primaryColor;
            set
            {
                _primaryColor = string.IsNullOrWhiteSpace(value) ? null : value;
                Theme = IconThemeType.TwoTone;
            }
        }

        /// <summary>
        /// Specify the secondary color when using the TwoTone theme. When omitted, the SVG's original secondary color is kept unless <see cref="TwoToneColor"/> is set.
        /// </summary>
        [Parameter]
        public string TwoToneSecondaryColor { get; set; }

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

        /// <summary>
        /// Custom component template receiving the SVG attributes forwarded by the icon wrapper.
        /// </summary>
        [Parameter]
        public RenderFragment<IReadOnlyDictionary<string, object>> ComponentTemplate { get; set; }

        private IReadOnlyDictionary<string, object> ComponentAttributes => new Dictionary<string, object>
        {
            ["width"] = Width,
            ["height"] = Height,
            ["fill"] = Fill,
            ["focusable"] = "false",
            ["aria-hidden"] = "true"
        };

        [Inject]
        public IconService IconService { get; set; }

        internal string SecondaryColor => !string.IsNullOrWhiteSpace(TwoToneSecondaryColor)
            ? TwoToneSecondaryColor
            : !string.IsNullOrWhiteSpace(_primaryColor) ? Colors.ColorPalette.Generate(_primaryColor)[0] : null;

        protected string _svgImg;
        private string _primaryColor;

        private Dictionary<string, object> _attributes = new();

        private string IconTabIndex => TabIndex ?? (OnClick.HasDelegate ? "-1" : null);

        protected override void OnInitialized()
        {
            // React icons do not create an ID unless the caller supplies one.
            // Avoid consuming a generated DOM ID here: icons are often nested
            // inside controls whose own IDs are part of their accessibility tree.
            // Explicit Id values remain supported by the inherited parameter.
        }

        protected override async Task OnInitializedAsync()
        {
            ClassMapper.Add($"anticon")
                .GetIf(() => $"anticon-{Type}", () => !string.IsNullOrWhiteSpace(Type) && string.IsNullOrEmpty(IconFont))
                .GetIf(() => "anticon-spin", () => Spin || Type == "loading");

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            _attributes.Clear();

            if (OnClick.HasDelegate)
            {
                _attributes.Add("onclick", (Delegate)HandleOnClick);
            }

            var accessibleName = !string.IsNullOrWhiteSpace(AriaLabel)
                ? AriaLabel
                : !string.IsNullOrWhiteSpace(Type) ? Type.ToLowerInvariant() : null;
            // React's createFromIconfontCN wrapper does not infer an accessible
            // name from the font glyph's type; only forward an explicit label.
            if (IconFont == null || !string.IsNullOrWhiteSpace(AriaLabel))
            {
                _attributes.Add("aria-label", accessibleName);
            }

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

            SetupSvgImg();

            base.OnParametersSet();
        }

        private void SetupSvgImg()
        {
            if (Component != null || ComponentTemplate != null)
            {
                return;
            }

            _svgImg = IconService.GetStyledSvg(this);
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
