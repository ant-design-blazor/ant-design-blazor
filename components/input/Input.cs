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
    /// Base class for input type components.
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
        protected IDomEventListener DomEventListener { get; set; }

        /// <summary>
        /// The label text displayed before (on the left side of) the input field.
        /// </summary>
        [Parameter]
        public RenderFragment AddOnBefore { get; set; }

        /// <summary>
        /// The label text displayed after (on the right side of) the input field.
        /// </summary>
        [Parameter]
        public RenderFragment AddOnAfter { get; set; }

        /// <summary>
        /// Allow to remove input content with clear icon 
        /// </summary>
        [Parameter]
        public bool AllowClear { get; set; }

        /// <summary>
        /// Controls the autocomplete attribute of the input HTML element.
        /// Default = true
        /// </summary>
        [Parameter]
        public bool AutoComplete { get; set; } = true;

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

        /// <summary>
        /// Whether has border style
        /// </summary>
        [Parameter]
        public bool Bordered { get; set; } = true;

        /// <summary>
        /// Delays the processing of the KeyUp event until the user has stopped 
        /// typing for a predetermined amount of time
        /// </summary>
        [Parameter]
        public int DebounceMilliseconds { get; set; } = 250;

        /// <summary>
        /// The initial input content  
        /// </summary>
        [Parameter]
        public TValue DefaultValue { get; set; }

        /// <summary>
        /// Whether the input is disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Css class that will be  added to input element class
        /// as the last class entry.
        /// </summary>
        [Parameter]
        public string InputElementSuffixClass { get; set; }

        /// <summary>
        /// Max length
        /// </summary>
        [Parameter]
        public int MaxLength { get; set; } = -1;

        /// <summary>
        /// Callback when input looses focus
        /// </summary>
        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Callback when the content changes
        /// </summary>
        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        /// <summary>
        /// Callback when input receives focus
        /// </summary>
        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Callback when value is inputed
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Callback when a key is pressed
        /// </summary>
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnkeyDown { get; set; }

        /// <summary>
        /// Callback when a key is released
        /// </summary>
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnkeyUp { get; set; }

        /// <summary>
        /// Callback when a mouse button is released
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseUp { get; set; }

        /// <summary>
        /// The callback function that is triggered when Enter key is pressed
        /// </summary>
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnPressEnter { get; set; }

        /// <summary>
        /// Provide prompt information that describes the expected value of the input field
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }

        /// <summary>
        /// The prefix icon for the Input.
        /// </summary>
        [Parameter]
        public RenderFragment Prefix { get; set; }

        /// <summary>
        /// When present, it specifies that an input field is read-only.
        /// </summary>
        [Parameter]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Controls onclick and blur event propagation.
        /// </summary>
        [Parameter]
        public bool StopPropagation { get; set; }

        /// <summary>
        /// The suffix icon for the Input.
        /// </summary>
        [Parameter]
        public RenderFragment Suffix { get; set; }


        /// <summary>
        /// The type of input, see: MDN(use `Input.TextArea` instead of type=`textarea`)
        /// </summary>
        [Parameter]
        public string Type { get; set; } = "text";

        /// <summary>
        /// Set CSS style of wrapper. Is used when component has visible: Prefix/Suffix 
        /// or has paramter set <seealso cref="AllowClear"/> or for components: <see cref="InputPassword"/>
        /// and <see cref="Search"/>. In these cases, html span elements is used 
        /// to wrap the html input element. 
        /// <seealso cref="WrapperStyle"/> is used on the span element.
        /// </summary>
        [Parameter]
        public string WrapperStyle { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        public ForwardRef WrapperRefBack { get; set; }

        /// <summary>
        /// Focus behavior for input component with optional behaviors.
        /// </summary>
        /// <param name="behavior">enum: AntDesign.FocusBehavior</param>
        /// <param name="preventScroll">When true, element receiving focus will not be scrolled to.</param>
        public virtual async Task Focus(FocusBehavior behavior = default, bool preventScroll = false)
        {
            if (behavior == FocusBehavior.FocusAndClear)
            {
                await Clear();
                StateHasChanged();
            }
            else
            {
                await FocusAsync(Ref, behavior, preventScroll);
                IsFocused = true;
            }
        }

        /// <summary>
        /// Removes focus from input element.
        /// </summary>       
        public async Task Blur()
        {
            await BlurAsync(Ref);
        }

        private async Task Clear()
        {
            CurrentValue = default;
            IsFocused = true;
            await this.FocusAsync(Ref);
            if (OnChange.HasDelegate)
                await OnChange.InvokeAsync(Value);
            else
                //Without the delay, focus is not enforced.
                await Task.Delay(1);
        }

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
            AffixWrapperClass = $"{PrefixCls}-affix-wrapper {(IsFocused ? $"{PrefixCls}-affix-wrapper-focused" : "")} {(Bordered ? "" : $"{PrefixCls}-affix-wrapper-borderless")}";
            GroupWrapperClass = $"{PrefixCls}-group-wrapper";

            if (!string.IsNullOrWhiteSpace(Class))
            {
                AffixWrapperClass = string.Join(" ", Class, AffixWrapperClass);
                ClassMapper.OriginalClass = "";
            }

            ClassMapper.Clear()
                .Add($"{PrefixCls}")
                .If($"{PrefixCls}-borderless", () => !Bordered)
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small)
                .If($"{PrefixCls}-rtl", () => RTL)
                .If($"{InputElementSuffixClass}", () => !string.IsNullOrEmpty(InputElementSuffixClass))
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
                builder.AddAttribute(33, "Theme", "fill");
                builder.AddAttribute(34, "Class", GetClearIconCls());
                if (string.IsNullOrEmpty(_inputValue?.ToString() ?? ""))
                {
                    builder.AddAttribute(35, "Style", "visibility: hidden;");
                }
                else
                {
                    builder.AddAttribute(36, "Style", "visibility: visible;");
                }
                builder.AddAttribute(37, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, async (args) =>
                {
                    await Clear();
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

                if (_debounceTimer != null)
                {
                    await _debounceTimer.DisposeAsync();
                    
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
                DomEventListener.AddExclusive<JsonElement>(Ref, "compositionstart", OnCompositionStart);
                DomEventListener.AddExclusive<JsonElement>(Ref, "compositionend", OnCompositionEnd);
                if (this.AutoFocus)
                {
                    IsFocused = true;
                    await this.FocusAsync(Ref);
                }

                DomEventListener.AddExclusive<JsonElement>(Ref, "focus", OnFocusInternal);
            }
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.DisposeExclusive();
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
                builder.AddAttribute(43, "style", Style);

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
                builder.AddAttribute(63, "readonly", ReadOnly);

                if (!AutoComplete)
                {
                    builder.AddAttribute(64, "autocomplete", "off");
                }

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

                if (StopPropagation)
                {
                    builder.AddEventStopPropagationAttribute(78, "onchange", true);
                    builder.AddEventStopPropagationAttribute(79, "onblur", true);
                }

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
