using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace AntBlazor
{
    // TODO: allowClear, password

    /// <summary>
    ///
    /// </summary>
    public class AntInputBase : AntInputComponentBase<string>
    {
        protected bool _allowClear;
        protected string _disabledWrapper;
        protected ElementReference inputEl { get; set; }

        [Parameter]
        public RenderFragment AddOnBefore { get; set; }

        [Parameter]
        public RenderFragment AddOnAfter { get; set; }

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

            if (!string.IsNullOrEmpty(defaultValue))
            {
                Value = defaultValue;
            }

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
                _disabledWrapper = "ant-input-affix-wrapper-disabled";
                ClassMapper.Add($"{prefixCls}-disabled");
            }

            if (Attributes.ContainsKey("allowClear"))
            {
                _allowClear = true;
                ToggleClearBtn();
            }

            //AddOnBefore = (builder) =>
            //{
            //    builder.OpenElement(0, "p");
            //    builder.AddContent(1, "https://");
            //    builder.CloseElement();
            //};
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
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

        private void ToggleClearBtn()
        {
            if (string.IsNullOrEmpty(Value))
            {
                suffix = string.Empty;
                StateHasChanged();
            }
            else
            {
                suffix = "close-circle";
                StateHasChanged();
            }
        }

        /// <summary>
        /// Invoked when user add/remove content
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task OnInputAsync(ChangeEventArgs args)
        {
            // AntInputComponentBase.Value will be empty, use args.Value
            Value = args.Value.ToString();
            if (_allowClear)
            {
                ToggleClearBtn();
            }

            return Task.CompletedTask;
        }

        protected virtual void ClearContent()
        {
            if (_allowClear)
            {
                Value = string.Empty;
                ToggleClearBtn();
            }
        }
    }
}