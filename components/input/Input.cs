using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    ///
    /// </summary>
    public class Input<TValue> : AntInputComponentBase<TValue>
    {
        protected const string PrefixCls = "ant-input";

        private bool _allowClear;
        protected string AffixWrapperClass { get; set; } = $"{PrefixCls}-affix-wrapper";
        private bool _hasAffixWrapper;
        protected string GroupWrapperClass { get; set; } = $"{PrefixCls}-group-wrapper";

        protected virtual string InputType => "input";

        //protected string ClearIconClass { get; set; }
        protected static readonly EventCallbackFactory CallbackFactory = new EventCallbackFactory();

        protected virtual bool IgnoreOnChangeAndBlur { get; }

        protected virtual bool EnableOnPressEnter => OnPressEnter.HasDelegate;

        [Inject]
        public DomEventService DomEventService { get; set; }

        [Parameter]
        public string Type { get; set; } = "text";

        [Parameter]
        public RenderFragment AddOnBefore { get; set; }

        [Parameter]
        public RenderFragment AddOnAfter { get; set; }

        [Parameter]
        public string Placeholder { get; set; }

        [Parameter]
        public bool AutoFocus
        {
            get { return _autoFocus; }
            set
            {
                _autoFocus = value;
                if (!_isInitialized && _autoFocus)
                    IsFocused = _autoFocus;
            }
        }

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
        public EventCallback<MouseEventArgs> OnMouseUp { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        [Parameter]
        public int DebounceMilliseconds { get; set; } = 250;

        [Parameter]
        public string WrapperStyle { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        public ForwardRef WrapperRefBack { get; set; }

        private TValue _inputValue;
        private bool _compositionInputting;
        private Timer _debounceTimer;
        private bool _autoFocus;
        private bool _isInitialized;

        private bool DebounceEnabled => DebounceMilliseconds != 0;

        protected bool IsFocused { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrEmpty(DefaultValue?.ToString()) && string.IsNullOrEmpty(Value?.ToString()))
            {
                Value = DefaultValue;
            }

            SetClasses();
            _isInitialized = true;
        }

        protected virtual void SetClasses()
        {
            AffixWrapperClass = $"{PrefixCls}-affix-wrapper {(IsFocused ? $"{PrefixCls}-affix-wrapper-focused" : "")}";
            GroupWrapperClass = $"{PrefixCls}-group-wrapper";

            if (!string.IsNullOrWhiteSpace(Class))
            {
                AffixWrapperClass = string.Join(" ", Class, AffixWrapperClass);
                ClassMapper.OriginalClass = "";
            }

            ClassMapper.Clear()
                .Add($"{PrefixCls}")
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small)
                .If($"{PrefixCls}-rtl", () => RTL)
                ;

            Attributes ??= new Dictionary<string, object>();

            if (MaxLength >= 0 && !Attributes.ContainsKey("maxlength"))
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
            if (args?.Key == "Enter" && InputType != "textarea")
            {
                await ChangeValue(true);
                if (EnableOnPressEnter)
                {
                    await OnPressEnter.InvokeAsync(args);
                    await OnPressEnterAsync();
                }
            }
        }

        protected virtual Task OnPressEnterAsync() => Task.CompletedTask;

        protected async Task OnKeyUpAsync(KeyboardEventArgs args)
        {
            await ChangeValue();

            if (OnkeyUp.HasDelegate) await OnkeyUp.InvokeAsync(args);
        }

        protected virtual async Task OnkeyDownAsync(KeyboardEventArgs args)
        {
            if (OnkeyDown.HasDelegate) await OnkeyDown.InvokeAsync(args);
        }

        protected async Task OnMouseUpAsync(MouseEventArgs args)
        {
            await ChangeValue(true);

            if (OnMouseUp.HasDelegate) await OnMouseUp.InvokeAsync(args);
        }

        internal virtual async Task OnBlurAsync(FocusEventArgs e)
        {
            IsFocused = false;
            if (_hasAffixWrapper)
                SetClasses();
            if (_compositionInputting)
            {
                _compositionInputting = false;
            }

            await ChangeValue(true);

            if (OnBlur.HasDelegate)
            {
                await OnBlur.InvokeAsync(e);
            }
        }

        private async void OnFocusInternal(JsonElement e) => await OnFocusAsync(new());

        internal virtual async Task OnFocusAsync(FocusEventArgs e)
        {
            IsFocused = true;
            if (_hasAffixWrapper)
                SetClasses();
            if (OnFocus.HasDelegate)
            {
                await OnFocus.InvokeAsync(e);
            }
        }

        internal virtual void OnCompositionStart(JsonElement e)
        {
            _compositionInputting = true;
        }

        internal virtual void OnCompositionEnd(JsonElement e)
        {
            _compositionInputting = false;
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
                builder.AddAttribute(35, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, async (args) =>
                {
                    CurrentValue = default;
                    IsFocused = true;
                    await this.FocusAsync(Ref);
                    if (OnChange.HasDelegate)
                        await OnChange.InvokeAsync(Value);
                    ToggleClearBtn();
                }));
                builder.CloseComponent();
            };
        }

        protected void DebounceChangeValue()
        {
            _debounceTimer?.Dispose();
            _debounceTimer = new Timer(DebounceTimerIntervalOnTick, null, DebounceMilliseconds, DebounceMilliseconds);
        }

        protected void DebounceTimerIntervalOnTick(object state)
        {
            InvokeAsync(async () => await ChangeValue(true));
        }

        private async Task ChangeValue(bool ignoreDebounce = false)
        {
            if (DebounceEnabled)
            {
                if (!ignoreDebounce)
                {
                    DebounceChangeValue();
                    return;
                }

                _debounceTimer?.Dispose();
                if (_debounceTimer != null)
                {
                    _debounceTimer = null;
                }
            }

            if (!_compositionInputting)
            {
                if (!EqualityComparer<TValue>.Default.Equals(CurrentValue, _inputValue))
                {
                    CurrentValue = _inputValue;
                    if (OnChange.HasDelegate)
                    {
                        await OnChange.InvokeAsync(Value);
                    }
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                DomEventService.AddEventListener(Ref, "compositionstart", OnCompositionStart);
                DomEventService.AddEventListener(Ref, "compositionend", OnCompositionEnd);
                if (this.AutoFocus)
                {
                    IsFocused = true;
                    await this.FocusAsync(Ref);
                }
                DomEventService.AddEventListener(Ref, "focus", OnFocusInternal, true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "compositionstart", OnCompositionStart);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "compositionend", OnCompositionEnd);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "focus", OnFocusInternal);

            _debounceTimer?.Dispose();

            base.Dispose(disposing);
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
                    _hasAffixWrapper = true;
                    builder.OpenElement(1, "span");
                    builder.AddAttribute(2, "class", GroupWrapperClass);
                    builder.AddAttribute(3, "style", WrapperStyle);
                    builder.OpenElement(4, "span");
                    builder.AddAttribute(5, "class", $"{PrefixCls}-wrapper {PrefixCls}-group");
                }

                if (AddOnBefore != null)
                {
                    _hasAffixWrapper = true;
                    // addOnBefore
                    builder.OpenElement(11, "span");
                    builder.AddAttribute(12, "class", $"{PrefixCls}-group-addon");
                    builder.AddContent(13, AddOnBefore);
                    builder.CloseElement();
                }

                if (Prefix != null || Suffix != null)
                {
                    _hasAffixWrapper = true;
                    builder.OpenElement(21, "span");
                    builder.AddAttribute(22, "class", AffixWrapperClass);
                    if (container == "input")
                    {
                        container = "affixWrapper";
                        builder.AddAttribute(3, "style", WrapperStyle);
                    }
                    if (WrapperRefBack != null)
                    {
                        builder.AddElementReferenceCapture(24, r => WrapperRefBack.Current = r);
                    }
                }

                if (Prefix != null)
                {
                    _hasAffixWrapper = true;
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

                bool needsDisabled = Disabled;
                if (Attributes != null)
                {
                    builder.AddMultipleAttributes(44, Attributes);
                    if (!Attributes.TryGetValue("disabled", out object disabledAttribute))
                    {
                        needsDisabled = ((bool?)disabledAttribute ?? needsDisabled) | Disabled;
                    }
                }

                if (AdditionalAttributes != null)
                {
                    builder.AddMultipleAttributes(45, AdditionalAttributes);
                    if (!AdditionalAttributes.TryGetValue("disabled", out object disabledAttribute))
                    {
                        needsDisabled = ((bool?)disabledAttribute ?? needsDisabled) | Disabled;
                    }
                }

                builder.AddAttribute(50, "Id", Id);
                builder.AddAttribute(51, "type", Type);
                builder.AddAttribute(60, "placeholder", Placeholder);
                builder.AddAttribute(61, "value", CurrentValue);
                builder.AddAttribute(62, "disabled", needsDisabled);

                // onchange 和 onblur 事件会导致点击 OnSearch 按钮时不触发 Click 事件，暂时取消这两个事件
                if (!IgnoreOnChangeAndBlur)
                {
                    builder.AddAttribute(70, "onchange", CallbackFactory.Create(this, OnChangeAsync));
                    builder.AddAttribute(71, "onblur", CallbackFactory.Create(this, OnBlurAsync));
                }

                builder.AddAttribute(72, "onkeypress", CallbackFactory.Create(this, OnKeyPressAsync));
                builder.AddAttribute(73, "onkeydown", CallbackFactory.Create(this, OnkeyDownAsync));
                builder.AddAttribute(74, "onkeyup", CallbackFactory.Create(this, OnKeyUpAsync));
                builder.AddAttribute(75, "oninput", CallbackFactory.Create(this, OnInputAsync));

                //TODO: Use built in @onfocus once https://github.com/dotnet/aspnetcore/issues/30070 is solved
                //builder.AddAttribute(76, "onfocus", CallbackFactory.Create(this, OnFocusAsync));
                builder.AddAttribute(77, "onmouseup", CallbackFactory.Create(this, OnMouseUpAsync));
                builder.AddElementReferenceCapture(90, r => Ref = r);
                builder.CloseElement();

                if (Suffix != null)
                {
                    _hasAffixWrapper = true;
                    // suffix
                    builder.OpenElement(91, "span");
                    builder.AddAttribute(92, "class", $"{PrefixCls}-suffix");
                    builder.AddContent(93, Suffix);
                    builder.CloseElement();
                }

                if (Prefix != null || Suffix != null)
                {
                    builder.CloseElement();
                }

                if (AddOnAfter != null)
                {
                    _hasAffixWrapper = true;
                    // addOnAfter
                    builder.OpenElement(100, "span");
                    builder.AddAttribute(101, "class", $"{PrefixCls}-group-addon");
                    builder.AddContent(102, AddOnAfter);
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
}
