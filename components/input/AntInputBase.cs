using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntBlazor
{
    /// <summary>
    ///
    /// </summary>
    public class AntInputBase : AntInputComponentBase<string>
    {
        [Parameter]
        public string size { get; set; } = AntInputSize.Default;

        protected override void OnInitialized()
        {
            Debug.WriteLine($"size: {size}");
            base.OnInitialized();

            //this.presetColor = this.isPresetColor(this.color);
            string prefix = "ant-input";
            this.ClassMapper.Clear()
                .If($"{prefix}-lg", () => size == AntInputSize.Large)
                .If($"{prefix}-sm", () => size == AntInputSize.Small)
                ;
        }
    }
}