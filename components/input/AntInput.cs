using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace AntBlazor
{
    /// <summary>
    ///
    /// </summary>
    public class AntInput : AntInputComponentBase<string>
    {
        protected const string PrefixCls = "ant-input";

        protected int _renderSequence = 0;
        //protected RenderFragment _renderFragment;
        protected bool _allowClear;
        protected string _type = "text";
        protected string _affixWrapperClass = $"{PrefixCls}-affix-wrapper";
        protected string _groupWrapperClass = $"{PrefixCls}-group-wrapper";
        protected string _clearIconClass;
        protected EventCallbackFactory _callbackFactory = new EventCallbackFactory();
        protected ElementReference inputEl { get; set; }

        [Parameter]
        public RenderFragment addOnBefore { get; set; }

        [Parameter]
        public RenderFragment addOnAfter { get; set; }

        [Parameter]
        public string size { get; set; } = AntInputSize.Default;

        [Parameter]
        public string placeholder { get; set; }

        [Parameter]
        public string defaultValue { get; set; }

        [Parameter]
        public int maxLength { get; set; } = -1;

        [Parameter]
        public RenderFragment prefix { get; set; }

        [Parameter]
        public RenderFragment suffix { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> onChange { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> onPressEnter { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrEmpty(defaultValue) && string.IsNullOrEmpty(Value))
            {
                Value = defaultValue;
            }

            //_renderFragment = new RenderFragment(builder => GenerateRenderFragment(builder));

            SetClasses();
        }

        protected virtual void SetClasses()
        {
            ClassMapper.Clear()
                .Add($"{PrefixCls}")
                .If($"{PrefixCls}-lg", () => size == AntInputSize.Large)
                .If($"{PrefixCls}-sm", () => size == AntInputSize.Small);

            if (Attributes is null)
            {
                Attributes = new System.Collections.Generic.Dictionary<string, object>();
            }

            _affixWrapperClass = $"{PrefixCls}-affix-wrapper";
            _groupWrapperClass = $"{PrefixCls}-group-wrapper";

            if (maxLength >= 0)
            {
                Attributes?.Add("maxlength", maxLength);
            }

            if (Attributes.ContainsKey("disabled"))
            {
                // TODO: disable element
                _affixWrapperClass = string.Join(" ", _affixWrapperClass, $"{PrefixCls}-affix-wrapper-disabled");
                ClassMapper.Add($"{PrefixCls}-disabled");
            }

            if (Attributes.ContainsKey("allowClear"))
            {
                _allowClear = true;
                _clearIconClass = $"{PrefixCls}-clear-icon";
                ToggleClearBtn();
            }

            if (size == AntInputSize.Large)
            {
                _affixWrapperClass = string.Join(" ", _affixWrapperClass, $"{PrefixCls}-affix-wrapper-lg");
                _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-group-wrapper-lg");
            }
            else if (size == AntInputSize.Small)
            {
                _affixWrapperClass = string.Join(" ", _affixWrapperClass, $"{PrefixCls}-affix-wrapper-sm");
                _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-group-wrapper-sm");
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClasses();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
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
            if (args.Key == "Enter" && onPressEnter.HasDelegate)
            {
                await onPressEnter.InvokeAsync(args);
            }
        }

        private void ToggleClearBtn()
        {
            if (string.IsNullOrEmpty(Value))
            {
                suffix = null;
                StateHasChanged();
            }
            else
            {
                suffix = new RenderFragment((builder) =>
                {
                    builder.OpenComponent<AntIcon>(_renderSequence++);
                    builder.AddAttribute(_renderSequence++, "type", "close-circle");
                    builder.AddAttribute(_renderSequence++, "onclick", _callbackFactory.Create<MouseEventArgs>(this, (args) =>
                    {
                        Value = string.Empty;
                        ToggleClearBtn();
                    }));
                    builder.CloseComponent();
                });
                StateHasChanged();
            }
        }

        /// <summary>
        /// Invoked when user add/remove content
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual void OnInputAsync(ChangeEventArgs args)
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(Value) && !string.IsNullOrEmpty(args.Value.ToString()))
            {
                flag = false;
            }
            // AntInputComponentBase.Value will be empty, use args.Value
            Value = args.Value.ToString();
            if (_allowClear && flag)
            {
                ToggleClearBtn();
            }
        }



        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            base.BuildRenderTree(builder);

            string container = "input";

            if (addOnBefore != null || addOnAfter != null)
            {
                container = "groupWrapper";
                builder.OpenElement(_renderSequence++, "span");
                builder.AddAttribute(_renderSequence++, "class", _groupWrapperClass);
                builder.AddAttribute(_renderSequence++, "style", Style);
                builder.OpenElement(_renderSequence++, "span");
                builder.AddAttribute(_renderSequence++, "class", $"{PrefixCls}-wrapper {PrefixCls}-group");
            }

            if (addOnBefore != null)
            {
                // addOnBefore
                builder.OpenElement(_renderSequence++, "span");
                builder.AddAttribute(_renderSequence++, "class", $"{PrefixCls}-group-addon");
                builder.AddContent(_renderSequence++, addOnBefore);
                builder.CloseElement();
            }

            if (prefix != null || suffix != null)
            {
                builder.OpenElement(_renderSequence++, "span");
                builder.AddAttribute(_renderSequence++, "class", _affixWrapperClass);
                if (container == "input")
                {
                    container = "affixWrapper";
                    builder.AddAttribute(_renderSequence++, "style", Style);
                }
            }

            if (prefix != null)
            {
                // prefix
                builder.OpenElement(_renderSequence++, "span");
                builder.AddAttribute(_renderSequence++, "class", $"{PrefixCls}-prefix");
                builder.AddContent(_renderSequence++, prefix);
                builder.CloseElement();
            }

            // input
            builder.OpenElement(_renderSequence++, "input");
            builder.AddAttribute(_renderSequence++, "class", ClassMapper.Class);
            if (container == "input")
            {
                builder.AddAttribute(_renderSequence++, "style", Style);
            }
            if (Attributes != null)
            {
                foreach (var pair in Attributes)
                {
                    builder.AddAttribute(_renderSequence++, pair.Key, pair.Value);
                }
            }
            builder.AddAttribute(_renderSequence++, "Id", Id);
            builder.AddAttribute(_renderSequence++, "type", _type);
            builder.AddAttribute(_renderSequence++, "placeholder", placeholder);
            builder.AddAttribute(_renderSequence++, "value", Value);
            builder.AddAttribute(_renderSequence++, "onchange", _callbackFactory.Create(this, OnChangeAsync));
            builder.AddAttribute(_renderSequence++, "onkeypress", _callbackFactory.Create(this, OnPressEnterAsync));
            builder.AddAttribute(_renderSequence++, "oninput", _callbackFactory.Create(this, OnInputAsync));
            builder.CloseElement();

            if (suffix != null)
            {
                // suffix
                builder.OpenElement(_renderSequence++, "span");
                builder.AddAttribute(_renderSequence++, "class", $"{PrefixCls}-suffix");
                builder.AddContent(_renderSequence++, suffix);
                builder.CloseElement();
            }

            if (prefix != null || suffix != null)
            {
                builder.CloseElement();
            }

            if (addOnAfter != null)
            {
                // addOnAfter
                builder.OpenElement(_renderSequence++, "span");
                builder.AddAttribute(_renderSequence++, "class", $"{PrefixCls}-group-addon");
                builder.AddContent(_renderSequence++, addOnAfter);
                builder.CloseElement();
            }

            if (addOnBefore != null || addOnAfter != null)
            {
                builder.CloseElement();
                builder.CloseElement();
            }
        }
    }
}