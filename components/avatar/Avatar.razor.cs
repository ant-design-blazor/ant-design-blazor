// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Avatars can be used to represent people or objects. It supports images, icons, or letters.</para>
    </summary>
    <seealso cref="AvatarGroup" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/aBcnbw68hP/Avatar.svg", Title = "Avatar", SubTitle = "头像")]
    public partial class Avatar : AntDomComponentBase
    {
        /// <summary>
        /// Content to display inside avatar shape. Takes priority over <see cref="Text"/>
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent
        {
            get => _childContent;
            set
            {
                _childContent = value;
                _waitingCalcSize = true;
            }
        }

        /// <summary>
        /// Shape of the avatar
        /// </summary>
        /// <default value="AvatarShape.Circle"/>
        [Parameter]
        public AvatarShape? Shape { get; set; }

        /// <summary>
        /// Size of the avatar. See <see cref="AvatarSize"/> for possible values.
        /// </summary>
        /// <default value="AvatarSize.Default"/>
        [Parameter]
        public OneOf<AvatarSize, string> Size
        {
            get => _size;
            set
            {
                if (_size.IsT0 && value.IsT0 && _size.AsT0 != value.AsT0)
                {
                    _size = value;
                    SetSizeStyle();
                }

                if (_size.IsT1 && value.IsT1 && _size.AsT1 != value.AsT1)
                {
                    _size = value;
                    SetSizeStyle();
                }

                if (_size.IsT0 != value.IsT0)
                {
                    _size = value;
                    SetSizeStyle();
                }
            }
        }

        /// <summary>
        /// Text string to display in the avatar. Typical use is for displaying initials.
        /// </summary>
        [Parameter]
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    _waitingCalcSize = true;
                }
            }
        }

        /// <summary>
        /// Image src for the avatar. If this fails to load, <see cref="Icon"/> and <see cref="ChildContent"/>/<see cref="Text"/> will continue to show.
        /// </summary>
        [Parameter]
        public string Src { get; set; }

        /// <summary>
        /// A list of sources to use for different screen resolutions. Passed straight to the <c>img</c> tag.
        /// </summary>
        [Parameter]
        public string SrcSet { get; set; }

        /// <summary>
        /// Alternate text for the image
        /// </summary>
        [Parameter]
        public string Alt { get; set; }

        /// <summary>
        /// Icon to display
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Callback executed when image passed to <see cref="Src"/> fails to load
        /// </summary>
        [Parameter]
        public EventCallback<ErrorEventArgs> OnError { get; set; }

        [CascadingParameter]
        public AvatarGroup Group { get; set; }

        [CascadingParameter(Name = "position")]
        internal string Position { get; set; }

        /// <summary>
        /// more than group max count
        /// </summary>
        internal bool Overflow { get; set; }

        private bool _hasText;
        private bool _hasSrc = true;
        private bool _hasIcon;

        private string _textStyles = string.Empty;
        private string _sizeStyles = string.Empty;

        protected ElementReference TextEl { get; set; }

        private readonly Dictionary<AvatarShape, string> _shapeMap = new()
        {
            [AvatarShape.Square] = "square",
            [AvatarShape.Circle] = "circle"
        };

        private string _text;
        private RenderFragment _childContent;
        private bool _waitingCalcSize;
        private OneOf<AvatarSize, string> _size = AvatarSize.Default;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Group?.AddAvatar(this);

            SetClassMap();
        }

        protected override void OnParametersSet()
        {
            _hasText = string.IsNullOrEmpty(Src) && (!string.IsNullOrEmpty(_text) || _childContent != null);
            _hasIcon = string.IsNullOrEmpty(Src) && !string.IsNullOrEmpty(Icon);
            _hasSrc = !string.IsNullOrEmpty(Src);

            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender || _waitingCalcSize)
            {
                _waitingCalcSize = false;
                await CalcStringSize();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task ImgError(ErrorEventArgs args)
        {
            await OnError.InvokeAsync(args);
            _hasSrc = false;
            _hasIcon = false;
            _hasText = false;
            if (!string.IsNullOrEmpty(Icon))
            {
                _hasIcon = true;
            }
            else if (!string.IsNullOrEmpty(_text))
            {
                _hasText = true;
            }

            _waitingCalcSize = true;
        }

        private void SetClassMap()
        {
            const string PrefixCls = "ant-avatar";

            ClassMapper
                .Add(PrefixCls)
                .GetIf(() => $"{PrefixCls}-{_shapeMap[Shape.GetValueOrDefault(AvatarShape.Square)]}", () => Shape.HasValue)
                .If($"{PrefixCls}-sm", () => _size.IsT0 && _size.AsT0 == AvatarSize.Small)
                .If($"{PrefixCls}-lg", () => _size.IsT0 && _size.AsT0 == AvatarSize.Large)
                .If($"{PrefixCls}-icon", () => !string.IsNullOrEmpty(Icon))
                .If($"{PrefixCls}-image", () => _hasSrc);
        }

        private void SetSizeStyle()
        {
            var size = string.Empty;

            if (_size.IsT0)
                size = _size.AsT0.ToString().ToLowerInvariant();
            else
                size = _size.AsT1;

            if (!CssSizeLength.TryParse(size, out var cssSize)) return;

            _sizeStyles = $"width:{cssSize};height:{cssSize};line-height:{cssSize};";
            _sizeStyles += $"font-size:calc({cssSize} / 2);";
        }

        private async Task CalcStringSize()
        {
            if (!_hasText)
            {
                return;
            }

            var childrenWidth = (await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, TextEl))?.OffsetWidth ?? 0;
            var avatarWidth = (await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref))?.Width ?? 0;
            var scale = childrenWidth != 0 && avatarWidth - 8 < childrenWidth
                ? (avatarWidth - 8) / childrenWidth
                : 1;

            _textStyles = $"transform: scale({new CssSizeLength(scale, true)}) translateX(-50%);";

            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            Group?.RemoveAvatar(this);

            base.Dispose(disposing);
        }
    }
}
