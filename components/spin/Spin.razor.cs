using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class Spin : AntDomComponentBase
    {
        [Parameter]
        public string Size { get; set; } = "default";

        [Parameter]
        public string Tip { get; set; } = null;

        [Parameter]
        public int Delay { get; set; } = 0;

        [Parameter]
        public bool Simple { get; set; } = false;

        [Parameter]
        public bool Spinning { get; set; } = true;

        [Parameter]
        public RenderFragment Indicator { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private const string PrefixCls = "ant-spin";

        private bool _isLoading = true;

        private string ContainerClass => _isLoading ? $"{PrefixCls}-blur" : "";

        private void SetClass()
        {
            ClassMapper.Add(PrefixCls)
                .If($"{PrefixCls}-spinning", () => Spinning)
                .If($"{PrefixCls}-lg", () => Size == "large")
                .If($"{PrefixCls}-sm", () => Size == "small")
                .If($"{PrefixCls}-show-text", () => string.IsNullOrWhiteSpace(Tip));
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClass();
        }
    }
}
