using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    ///
    /// </summary>
    public class Input<TValue> : AntInputComponentBase<TValue>, IAutoCompleteInput
    {
        protected const string PrefixCls = "ant-input";

        private bool _allowClear;
        protected string AffixWrapperClass { get; set; } = $"{PrefixCls}-affix-wrapper";
        protected string GroupWrapperClass { get; set; } = $"{PrefixCls}-group-wrapper";

        //protected string ClearIconClass { get; set; }
        protected static readonly EventCallbackFactory CallbackFactory = new EventCallbackFactory();

        protected virtual bool IgnoreOnChangeAndBlur { get; }

        [Parameter]
        public string Type { get; set; } = "text";

        [Parameter]
        public RenderFragment AddOnBefore { get; set; }

        [Parameter]
        public RenderFragment AddOnAfter { get; set; }

        [Parameter]
        public string Placeholder { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; }

        [Parameter]
        public TValue DefaultValue { get; set; }

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
        public EventCallback<TValue> OnChange { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnPressEnter { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnkeyUp { get; set; }
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnkeyDown { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        private TValue _inputValue;

        [CascadingParameter]
        public AutoComplete AutoComplete { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrEmpty(DefaultValue?.ToString()) && string.IsNullOrEmpty(Value?.ToString()))
            {
                Value = DefaultValue;
            }

            SetClasses();

            AutoComplete?.SetInputComponent(this);
        }

        protected virtual void SetClasses()
        {
            AffixWrapperClass = $"{PrefixCls}-affix-wrapper";
            GroupWrapperClass = $"{PrefixCls}-group-wrapper";

            if (!string.IsNullOrWhiteSpace(Class))
            {
                AffixWrapperClass = string.Join(" ", Class, AffixWrapperClass);
                ClassMapper.OriginalClass = "";
            }

            ClassMapper.Clear()
                .If($"{PrefixCls}", () => Type != "number")
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small);

            Attributes ??= new Dictionary<string, object>();

            if (MaxLength >= 0)
            {
                Attributes?.Add("maxlength", MaxLength);
            }

            if (Disabled)
            {
                // TODO: disable element
                AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-affix-wrapper-disabled");
                ClassMapper.Add($"{PrefixCls}-disabled");
            }

            if (AllowClear)
            {
                _allowClear = true;
                //ClearIconClass = $"{PrefixCls}-clear-icon";
                ToggleClearBtn();
            }

            if (Size == InputSize.Large)
            {
                AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-affix-wrapper-lg");
                GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-group-wrapper-lg");
            }
            else if (Size == InputSize.Small)
            {
                AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-affix-wrapper-sm");
                GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-group-wrapper-sm");
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClasses();
        }

        public async Task Focus()
        {
            await JsInvokeAsync(JSInteropConstants.focus, Ref);
        }

        protected virtual async Task OnChangeAsync(ChangeEventArgs args)
        {
            if (CurrentValueAsString != args?.Value?.ToString())
            {
                if (OnChange.HasDelegate)
                {
                    await OnChange.InvokeAsync(Value);
                }
            }
        }

        protected async Task OnKeyPressAsync(KeyboardEventArgs args)
        {
            if (args != null && args.Key == "Enter" && OnPressEnter.HasDelegate)
            {
                await OnPressEnter.InvokeAsync(args);
            }
        }

        protected async Task OnKeyUpAsync(KeyboardEventArgs args)
        {
            if (!EqualityComparer<TValue>.Default.Equals(CurrentValue, _inputValue))
            {
                CurrentValue = _inputValue;
                if (OnChange.HasDelegate)
                {
                    await OnChange.InvokeAsync(Value);
                }
            }

            if (OnkeyUp.HasDelegate) await OnkeyUp.InvokeAsync(args);
        }

        protected async Task OnkeyDownAsync(KeyboardEventArgs args)
        {
            await AutoComplete?.InputKeyDown(args);

            if (OnkeyDown.HasDelegate) await OnkeyDown.InvokeAsync(args);
        }

        private async Task OnBlurAsync(FocusEventArgs e)
        {
            await AutoComplete?.InputBlur(e);

            if (OnBlur.HasDelegate)
            {
                await OnBlur.InvokeAsync(e);
            }
        }

        private async Task OnFocusAsync(FocusEventArgs e)
        {
            await AutoComplete?.InputFocus(e);

            if (OnFocus.HasDelegate)
            {
                await OnFocus.InvokeAsync(e);
            }
        }
        private void ToggleClearBtn()
        {
            Suffix = (builder) =>
            {
                builder.OpenComponent<Icon>(31);
                builder.AddAttribute(32, "Type", "close-circle");
                builder.AddAttribute(33, "Class", GetClearIconCls());
                if (string.IsNullOrEmpty(Value?.ToString()))
                {
                    builder.AddAttribute(34, "Style", "visibility: hidden;");
                }
                else
                {
                    builder.AddAttribute(34, "Style", "visibility: visible;");
                }
                builder.AddAttribute(35, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, (args) =>
                {
                    CurrentValue = default;
                    if (OnChange.HasDelegate)
                        OnChange.InvokeAsync(Value);
                    ToggleClearBtn();
                }));
                builder.CloseComponent();
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (this.AutoFocus)
            {
                await this.Focus();
            }
        }

        protected virtual string GetClearIconCls()
        {
            return $"{PrefixCls}-clear-icon";
        }

        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);
            _inputValue = value;
        }

        /// <summary>
        /// Invoked when user add/remove content
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual async void OnInputAsync(ChangeEventArgs args)
        {
            bool flag = !(!string.IsNullOrEmpty(Value?.ToString()) && args != null && !string.IsNullOrEmpty(args.Value.ToString()));

            if (TryParseValueFromString(args?.Value.ToString(), out TValue value, out var error))
            {
                _inputValue = value;
            }

            if (_allowClear && flag)
            {
                ToggleClearBtn();
            }

            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args);
            }

            await AutoComplete?.InputInput(args);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder != null)
            {
                base.BuildRenderTree(builder);

                string container = "input";

                if (AddOnBefore != null || AddOnAfter != null)
                {
                    container = "groupWrapper";
                    builder.OpenElement(1, "span");
                    builder.AddAttribute(2, "class", GroupWrapperClass);
                    builder.AddAttribute(3, "style", Style);
                    builder.OpenElement(4, "span");
                    builder.AddAttribute(5, "class", $"{PrefixCls}-wrapper {PrefixCls}-group");
                }

                if (AddOnBefore != null)
                {
                    // addOnBefore
                    builder.OpenElement(11, "span");
                    builder.AddAttribute(12, "class", $"{PrefixCls}-group-addon");
                    builder.AddContent(13, AddOnBefore);
                    builder.CloseElement();
                }

                if (Prefix != null || Suffix != null)
                {
                    builder.OpenElement(21, "span");
                    builder.AddAttribute(22, "class", AffixWrapperClass);
                    if (container == "input")
                    {
                        container = "affixWrapper";
                        builder.AddAttribute(23, "style", Style);
                    }
                }

                if (Prefix != null)
                {
                    // prefix
                    builder.OpenElement(31, "span");
                    builder.AddAttribute(32, "class", $"{PrefixCls}-prefix");
                    builder.AddContent(33, Prefix);
                    builder.CloseElement();
                }

                // input
                builder.OpenElement(41, "input");
                builder.AddAttribute(42, "class", ClassMapper.Class);
                if (container == "input")
                {
                    builder.AddAttribute(43, "style", Style);
                }

                if (Attributes != null)
                {
                    builder.AddMultipleAttributes(44, Attributes);
                }

                if (AdditionalAttributes != null)
                {
                    builder.AddMultipleAttributes(45, AdditionalAttributes);
                }

                builder.AddAttribute(50, "Id", Id);
                if (Type != "number")
                {
                    builder.AddAttribute(51, "type", Type);
                }

                builder.AddAttribute(60, "placeholder", Placeholder);
                builder.AddAttribute(61, "value", CurrentValue);

                // onchange 和 onblur 事件会导致点击 OnSearch 按钮时不触发 Click 事件，暂时取消这两个事件
                if (!IgnoreOnChangeAndBlur)
                {
                    builder.AddAttribute(62, "onchange", CallbackFactory.Create(this, OnChangeAsync));
                    builder.AddAttribute(65, "onblur", CallbackFactory.Create(this, OnBlurAsync));
                }

                builder.AddAttribute(63, "onkeypress", CallbackFactory.Create(this, OnKeyPressAsync));
                builder.AddAttribute(63, "onkeydown", CallbackFactory.Create(this, OnkeyDownAsync));
                builder.AddAttribute(63, "onkeyup", CallbackFactory.Create(this, OnKeyUpAsync));
                builder.AddAttribute(64, "oninput", CallbackFactory.Create(this, OnInputAsync));
                builder.AddAttribute(66, "onfocus", CallbackFactory.Create(this, OnFocusAsync));
                builder.AddElementReferenceCapture(68, r => Ref = r);
                builder.CloseElement();

                if (Suffix != null)
                {
                    // suffix
                    builder.OpenElement(71, "span");
                    builder.AddAttribute(72, "class", $"{PrefixCls}-suffix");
                    builder.AddContent(73, Suffix);
                    builder.CloseElement();
                }

                if (Prefix != null || Suffix != null)
                {
                    builder.CloseElement();
                }

                if (AddOnAfter != null)
                {
                    // addOnAfter
                    builder.OpenElement(81, "span");
                    builder.AddAttribute(82, "class", $"{PrefixCls}-group-addon");
                    builder.AddContent(83, AddOnAfter);
                    builder.CloseElement();
                }

                if (AddOnBefore != null || AddOnAfter != null)
                {
                    builder.CloseElement();
                    builder.CloseElement();
                }
            }
        }


        #region IAutoCompleteInput

        public void SetValue(object value)
        {
            this.CurrentValue = (TValue)value;
        }

        #endregion
    }
}
