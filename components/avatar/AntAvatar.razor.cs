using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class AntAvatar : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Shape { get; set; } = null;

        [Parameter]
        public string Size { get; set; } = AntSizeLDSType.Default;

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string Src { get; set; }

        [Parameter]
        public string SrcSet { get; set; }

        [Parameter]
        public string Alt { get; set; }

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public EventCallback<ErrorEventArgs> Error { get; set; }

        private bool _hasText = false;
        private bool _hasSrc = true;
        private bool _hasIcon = false;

        private string _textStyles = "";

        protected ElementReference TextEl { get; set; }

        private string _prefixCls = "ant-avatar";

        private readonly Hashtable _sizeMap = new Hashtable() { ["large"] = "lg", ["small"] = "sm" };

        private void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(_prefixCls)
                .If($"{_prefixCls}-{this._sizeMap[this.Size]}", () => _sizeMap.ContainsKey(Size))
                .If($"{_prefixCls}-{this.Shape}", () => !string.IsNullOrEmpty(Shape))
                .If($"{_prefixCls}-icon", () => !string.IsNullOrEmpty(Icon))
                .If($"{_prefixCls}-image", () => _hasSrc)
                ;
        }

        protected override async Task OnParametersSetAsync()
        {
            this._hasText = string.IsNullOrEmpty(this.Src) && !string.IsNullOrEmpty(this.Text);
            this._hasIcon = string.IsNullOrEmpty(this.Src) && !string.IsNullOrEmpty(this.Icon);
            this._hasSrc = !string.IsNullOrEmpty(this.Src);

            SetClassMap();
            await CalcStringSize();
            SetSizeStyle();
            await base.OnParametersSetAsync();
        }

        protected async Task ImgError(ErrorEventArgs args)
        {
            await Error.InvokeAsync(args);
            this._hasSrc = false;
            this._hasIcon = false;
            this._hasText = false;
            if (!string.IsNullOrEmpty(this.Icon))
            {
                this._hasIcon = true;
            }
            else if (!string.IsNullOrEmpty(this.Text))
            {
                this._hasText = true;
            }
            this.SetClassMap();
            await CalcStringSize();
            SetSizeStyle();
        }

        private void SetSizeStyle()
        {
            if (decimal.TryParse(this.Size, out var pxSize))
            {
                string size = StyleHelper.ToCssPixel(pxSize.ToString(CultureInfo.InvariantCulture));
                Style += $";width:{size};";
                Style += $"height:{size};";
                Style += $"line-height:{size};";
                if (this._hasIcon)
                {
                    Style += $"font-size:calc(${size} / 2)";
                }
            }
        }

        private async Task CalcStringSize()
        {
            if (!this._hasText)
            {
                return;
            }

            var childrenWidth = (await this.JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, this.TextEl)).offsetWidth;
            var avatarWidth = (await this.JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, this.Ref)).width;
            var scale = avatarWidth - 8 < childrenWidth ? (avatarWidth - 8) / childrenWidth : 1;
            this._textStyles = $"transform: scale({scale}) translateX(-50%);";
            if (decimal.TryParse(this.Size, out var pxSize))
            {
                this._textStyles += $"lineHeight:{pxSize}px;";
            }
        }
    }
}
