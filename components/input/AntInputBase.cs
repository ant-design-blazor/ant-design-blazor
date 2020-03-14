using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AntBlazor
{
    // TODO:addonAfter, addonBefore, disabled, allowClear

    /// <summary>
    ///
    /// </summary>
    public class AntInputBase : AntInputComponentBase<string>
    {
        protected bool _allowClear;

        [Parameter]
        public string size { get; set; } = AntInputSize.Default;

        [Parameter]
        public string placeholder { get; set; }

        [Parameter]
        public string defaultValue { get; set; }

        [Parameter]
        public int maxLength { get; set; } = -1;

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

            string prefixCls = "ant-input";
            ClassMapper.Clear()
                .Add($"{prefixCls}")
                .If($"{prefixCls}-lg", () => size == AntInputSize.Large)
                .If($"{prefixCls}-sm", () => size == AntInputSize.Small);

            if (Attributes is null)
            {
                Attributes = new System.Collections.Generic.Dictionary<string, object>();
            }

            if (maxLength >= 0)
            {
                Attributes?.Add("maxlength", maxLength);
            }

            if (Attributes.ContainsKey("disabled"))
            {
                // TODO: disable element
            }

            if (Attributes.ContainsKey("allowClear"))
            {
                // TODO: show clear button
                _allowClear = true;
                if (!string.IsNullOrEmpty(defaultValue) || !string.IsNullOrEmpty(Value))
                {
                    suffix = "close-circle";
                }
            }
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

        protected async Task OnInputAsync(ChangeEventArgs args)
        {
            // AntInputComponentBase.Value will be empty, use args.Value
            Value = args.Value.ToString();

            if (_allowClear)
            {
                // TODO: toggle clear button
                if (string.IsNullOrEmpty(Value))
                {
                    suffix = string.Empty;
                    this.StateHasChanged();
                }
                else
                {
                    suffix = "close-circle";
                    this.StateHasChanged();
                }
            }
        }
    }
}