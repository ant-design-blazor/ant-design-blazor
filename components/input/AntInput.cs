using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
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
        protected bool _allowClear;
        protected string _affixWrapperClass = $"{PrefixCls}-affix-wrapper";
        protected string _groupWrapperClass = $"{PrefixCls}-group-wrapper";
        protected string _clearIconClass;
        protected EventCallbackFactory _callbackFactory = new EventCallbackFactory();
        protected ElementReference InputEl { get; set; }

        [Parameter]
        public string Type { get; set; } = "text";

        [Parameter]
        public RenderFragment AddOnBefore { get; set; }

        [Parameter]
        public RenderFragment AddOnAfter { get; set; }

        [Parameter]
        public string Size { get; set; } = AntInputSize.Default;

        [Parameter]
        public string Placeholder { get; set; }

        [Parameter]
        public string DefaultValue { get; set; }

        [Parameter]
        public int MaxLength { get; set; } = -1;

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool AllowClear { get; set; }

        [Parameter]
        public RenderFragment Prefix { get; set; }

        [Parameter]
        public RenderFragment Suffix { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnPressEnter { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        public Dictionary<string,object> Attributes { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrEmpty(DefaultValue) && string.IsNullOrEmpty(Value))
            {
                Value = DefaultValue;
            }

            SetClasses();
        }

        protected virtual void SetClasses()
        {
            ClassMapper.Clear()
                .If($"{PrefixCls}", () => Type != "number")
                .If($"{PrefixCls}-lg", () => Size == AntInputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == AntInputSize.Small);

            if (Attributes is null)
            {
                Attributes = new Dictionary<string, object>();
            }

            _affixWrapperClass = $"{PrefixCls}-affix-wrapper";
            _groupWrapperClass = $"{PrefixCls}-group-wrapper";

            if (MaxLength >= 0)
            {
                Attributes?.Add("maxlength", MaxLength);
            }

            if (Disabled)
            {
                // TODO: disable element
                _affixWrapperClass = string.Join(" ", _affixWrapperClass, $"{PrefixCls}-affix-wrapper-disabled");
                ClassMapper.Add($"{PrefixCls}-disabled");
            }

            if (AllowClear)
            {
                _allowClear = true;
                _clearIconClass = $"{PrefixCls}-clear-icon";
                ToggleClearBtn();
            }

            if (Size == AntInputSize.Large)
            {
                _affixWrapperClass = string.Join(" ", _affixWrapperClass, $"{PrefixCls}-affix-wrapper-lg");
                _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-group-wrapper-lg");
            }
            else if (Size == AntInputSize.Small)
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
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(args);
            }
        }

        protected async Task OnPressEnterAsync(KeyboardEventArgs args)
        {
            if (args.Key == "Enter" && OnPressEnter.HasDelegate)
            {
                await OnPressEnter.InvokeAsync(args);
            }
        }

        private void ToggleClearBtn()
        {
            Suffix = new RenderFragment((builder) =>
            {
                builder.OpenComponent<AntIcon>(31);
                builder.AddAttribute(32, "type", "close-circle");
                if (string.IsNullOrEmpty(Value))
                {
                    builder.AddAttribute(33, "style", "visibility: hidden;");
                }
                else
                {
                    builder.AddAttribute(33, "style", "visibility: visible;");
                }
                builder.AddAttribute(34, "onclick", _callbackFactory.Create<MouseEventArgs>(this, (args) =>
                {
                    Value = string.Empty;
                    ToggleClearBtn();
                }));
                builder.CloseComponent();
            });

        }

        /// <summary>
        /// Invoked when user add/remove content
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async virtual void OnInputAsync(ChangeEventArgs args)
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

            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args);
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            string container = "input";

            if (AddOnBefore != null || AddOnAfter != null)
            {
                container = "groupWrapper";
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", _groupWrapperClass);
                builder.AddAttribute(2, "style", Style);
                builder.OpenElement(3, "span");
                builder.AddAttribute(4, "class", $"{PrefixCls}-wrapper {PrefixCls}-group");
            }

            if (AddOnBefore != null)
            {
                // addOnBefore
                builder.OpenElement(5, "span");
                builder.AddAttribute(6, "class", $"{PrefixCls}-group-addon");
                builder.AddContent(7, AddOnBefore);
                builder.CloseElement();
            }

            if (Prefix != null || Suffix != null)
            {
                builder.OpenElement(8, "span");
                builder.AddAttribute(9, "class", _affixWrapperClass);
                if (container == "input")
                {
                    container = "affixWrapper";
                    builder.AddAttribute(10, "style", Style);
                }
            }

            if (Prefix != null)
            {
                // prefix
                builder.OpenElement(11, "span");
                builder.AddAttribute(12, "class", $"{PrefixCls}-prefix");
                builder.AddContent(13, Prefix);
                builder.CloseElement();
            }

            // input
            builder.OpenElement(14, "input");
            builder.AddAttribute(15, "class", ClassMapper.Class);
            if (container == "input")
            {
                builder.AddAttribute(16, "style", Style);
            }
            if (Attributes != null)
            {
                foreach (var pair in Attributes)
                {
                    builder.AddAttribute(17, pair.Key, pair.Value);
                }
            }
            builder.AddAttribute(18, "Id", Id);
            if (Type != "number")
            {
                builder.AddAttribute(19, "type", Type);
            }
            builder.AddAttribute(20, "placeholder", Placeholder);
            builder.AddAttribute(21, "value", Value);
            builder.AddAttribute(22, "onchange", _callbackFactory.Create(this, OnChangeAsync));
            builder.AddAttribute(23, "onkeypress", _callbackFactory.Create(this, OnPressEnterAsync));
            builder.AddAttribute(24, "oninput", _callbackFactory.Create(this, OnInputAsync));
            builder.CloseElement();

            if (Suffix != null)
            {
                // suffix
                builder.OpenElement(25, "span");
                builder.AddAttribute(26, "class", $"{PrefixCls}-suffix");
                builder.AddContent(27, Suffix);
                builder.CloseElement();
            }

            if (Prefix != null || Suffix != null)
            {
                builder.CloseElement();
            }

            if (AddOnAfter != null)
            {
                // addOnAfter
                builder.OpenElement(28, "span");
                builder.AddAttribute(29, "class", $"{PrefixCls}-group-addon");
                builder.AddContent(30, AddOnAfter);
                builder.CloseElement();
            }

            if (AddOnBefore != null || AddOnAfter != null)
            {
                builder.CloseElement();
                builder.CloseElement();
            }
        }
    }
}
