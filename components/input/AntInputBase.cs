using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
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

        [Parameter]
        public string placeholder { get; set; }

        [Parameter]
        public string defaultValue { get; set; }

        [Parameter]
        public string prefix { get; set; }

        [Parameter]
        public string suffix { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> onChange { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> onPressEnter { get; set; }

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

        protected async Task OnChangeAsync(ChangeEventArgs args)
        {
            if (onChange.HasDelegate)
            {
                await onChange.InvokeAsync(args);
            }
        }

        protected async Task OnPressEnterAsync(KeyboardEventArgs args)
        {
            if (args.Code == "Enter" && onPressEnter.HasDelegate)
            {
                await onPressEnter.InvokeAsync(args);
            }
        }
    }
}