using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public class AntAvatarBase : AntDomComponentBase
    {
        [Parameter] public string shape { get; set; } = null;

        [Parameter] public string size { get; set; } = AntSizeLDSType.Default;

        [Parameter] public string text { get; set; }

        [Parameter] public string src { get; set; }

        [Parameter] public string srcSet { get; set; }

        [Parameter] public string alt { get; set; }

        [Parameter] public string icon { get; set; }

        [Parameter] public EventCallback<ErrorEventArgs> Error { get; set; }

        protected bool hasText = false;
        protected bool hasSrc = true;
        protected bool hasIcon = false;

        protected string textStyles = "";

        protected ElementReference textEl { get; set; }

        private string prefixCls = "ant-avatar";

        private Hashtable sizeMap = new Hashtable() { ["large"] = "lg", ["small"] = "sm" };

        private void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-{this.sizeMap[this.size]}", () => sizeMap.ContainsKey(size))
                .If($"{prefixCls}-{this.shape}", () => !string.IsNullOrEmpty(shape))
                .If($"{prefixCls}-icon", () => !string.IsNullOrEmpty(icon))
                .If($"{prefixCls}-image", () => hasSrc)
                ;
        }

        protected override async Task OnParametersSetAsync()
        {
            this.hasText = string.IsNullOrEmpty(this.src) && !string.IsNullOrEmpty(this.text);
            this.hasIcon = string.IsNullOrEmpty(this.src) && !string.IsNullOrEmpty(this.icon);
            this.hasSrc = !string.IsNullOrEmpty(this.src);

            SetClassMap();
            await calcStringSize();
            setSizeStyle();
            await base.OnParametersSetAsync();
        }

        protected async Task ImgError(ErrorEventArgs args)
        {
            await Error.InvokeAsync(args);
            this.hasSrc = false;
            this.hasIcon = false;
            this.hasText = false;
            if (!string.IsNullOrEmpty(this.icon))
            {
                this.hasIcon = true;
            }
            else if (!string.IsNullOrEmpty(this.text))
            {
                this.hasText = true;
            }
            this.SetClassMap();
            await calcStringSize();
            setSizeStyle();
        }

        private void setSizeStyle()
        {
            if (decimal.TryParse(this.size, out var pxSize))
            {
                string size = StyleHelper.ToCssPixel(pxSize.ToString(CultureInfo.InvariantCulture));
                Style += $";width:{size};";
                Style += $"height:{size};";
                Style += $"line-height:{size};";
                if (this.hasIcon)
                {
                    Style += $"font-size:calc(${size} / 2)";
                }
            }
        }

        private async Task calcStringSize()
        {
            if (!this.hasText)
            {
                return;
            }

            var childrenWidth = (await this.JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, this.textEl)).offsetWidth;
            var avatarWidth = (await this.JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, this.Ref)).width;
            var scale = avatarWidth - 8 < childrenWidth ? (avatarWidth - 8) / childrenWidth : 1;
            this.textStyles = $"transform: scale({scale}) translateX(-50%);";
            if (decimal.TryParse(this.size, out var pxSize))
            {
                this.textStyles += $"lineHeight:{pxSize}px;";
            }
        }
    }
}