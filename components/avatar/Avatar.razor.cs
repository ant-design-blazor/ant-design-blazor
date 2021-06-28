using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Avatar : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent
        {
            get => _childContent;
            set
            {
                _childContent = value;
                _waitingCaclSize = true;
            }
        }

        [Parameter]
        public string Shape { get; set; } = null;

        [Parameter]
        public string Size { get; set; } = AntSizeLDSType.Default;

        [Parameter]
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    _waitingCaclSize = true;
                }
            }
        }

        [Parameter]
        public string Src { get; set; }

        [Parameter]
        public string SrcSet { get; set; }

        [Parameter]
        public string Alt { get; set; }

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public EventCallback<ErrorEventArgs> OnError { get; set; }

        [CascadingParameter]
        public AvatarGroup Group { get; set; }

        [CascadingParameter(Name = "position")]
        public string Position { get; set; }

        /// <summary>
        /// more than group max count
        /// </summary>
        internal bool Overflow { get; set; }

        private bool _hasText;
        private bool _hasSrc = true;
        private bool _hasIcon;

        private string _textStyles = "";
        private string _sizeStyles = "";

        protected ElementReference TextEl { get; set; }

        private string _prefixCls = "ant-avatar";

        private readonly Hashtable _sizeMap = new Hashtable() { ["large"] = "lg", ["small"] = "sm" };

        private string _text;
        private RenderFragment _childContent;
        private bool _waitingCaclSize;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Group?.AddAvatar(this);

            SetClassMap();
            SetSizeStyle();
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
            if (firstRender || _waitingCaclSize)
            {
                _waitingCaclSize = false;
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

            _waitingCaclSize = true;
        }

        private void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(_prefixCls)
                .GetIf(() => $"{_prefixCls}-{_sizeMap[Size]}", () => _sizeMap.ContainsKey(Size))
                .GetIf(() => $"{_prefixCls}-{Shape}", () => !string.IsNullOrEmpty(Shape))
                .If($"{_prefixCls}-icon", () => !string.IsNullOrEmpty(Icon))
                .If($"{_prefixCls}-image", () => _hasSrc);
        }

        private void SetSizeStyle()
        {
            if (decimal.TryParse(Size, out var pxSize))
            {
                string size = new CssSizeLength(pxSize).ToString();
                _sizeStyles = $"width:{size};height:{size};line-height:{size};";
                if (_hasIcon)
                {
                    _sizeStyles += $"font-size:calc(${size} / 2);";
                }
            }
        }

        private async Task CalcStringSize()
        {
            if (!_hasText)
            {
                return;
            }

            var childrenWidth = (await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, TextEl))?.OffsetWidth ?? 0;
            var avatarWidth = (await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref))?.Width ?? 0;
            var scale = childrenWidth != 0 && avatarWidth - 8 < childrenWidth ? (avatarWidth - 8) / childrenWidth : 1;
            _textStyles = $"transform: scale({new CssSizeLength(scale, true)}) translateX(-50%);";
            if (decimal.TryParse(Size, out var pxSize))
            {
                _textStyles += $"lineHeight:{(CssSizeLength)pxSize};";
            }

            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            Group?.RemoveAvatar(this);

            base.Dispose(disposing);
        }
    }
}
