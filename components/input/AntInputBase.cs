using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace AntBlazor
{
    /// <summary>
    ///
    /// </summary>
    public class AntInputBase : AntInputComponentBase<string>
    {
        [Parameter]
        public string size { get; set; } = AntInputSize.Default;

        [Parameter]
        public string placeholder { get; set; }

        [Parameter]
        public string prefix { get; set; }

        [Parameter]
        public string suffix { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            //this.presetColor = this.isPresetColor(this.color);
            string prefixCls = "ant-input";
            this.ClassMapper.Clear()
                .Add($"{prefixCls}")
                .If($"{prefixCls}-lg", () => size == AntInputSize.Large)
                .If($"{prefixCls}-sm", () => size == AntInputSize.Small);
        }
    }
}