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
        public string icon { get; set; }

        protected override void OnInitialized()
        {
            Debug.WriteLine($"size: {size}");
            base.OnInitialized();

            //this.presetColor = this.isPresetColor(this.color);
            string prefix = "ant-input";
            this.ClassMapper.Clear()
                .Add($"{prefix}")
                .If($"{prefix}-lg", () => size == AntInputSize.Large)
                .If($"{prefix}-sm", () => size == AntInputSize.Small)
                ;
        }
    }
}