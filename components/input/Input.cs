// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
    <para>A basic widget for getting the user input is a text field. Keyboard and mouse can be used for providing or changing data.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>A user input in a form field is needed.</item>
        <item>A search input is required.</item>
    </list>
    </summary>
    <seealso cref="TextArea"/>
    <seealso cref="Search"/>
    <seealso cref="InputGroup"/>
    <seealso cref="InputPassword"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/xS9YEJhfe/Input.svg", Title = "Input", SubTitle = "输入框")]
    public class Input<TValue> : AntInputComponentBase<TValue>
    {
        protected const string PrefixCls = "ant-input";

        protected string AffixWrapperClass { get; set; } = $"{PrefixCls}-affix-wrapper";
        private bool _hasAffixWrapper;
        protected string GroupWrapperClass { get; set; } = $"{PrefixCls}-group-wrapper";

        protected virtual string InputType => "input";

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
        /// Overrides whether the clear icon is shown. When <see langword="null"/>, it is shown if and only if the input string is not empty.
        /// </summary>
        /// <remarks>
        /// Requires <see cref="AllowClear"/> to be <see langword="true"/>, otherwise this has no effect.
        /// </remarks>
        [Parameter]
        public bool? ShowClear { get; set; }

        /// <summary>
        /// Callback when the content is cleared by clicking the "ClearIcon"
        /// </summary>
        [Parameter]
        public EventCallback OnClear { get; set; }

        /// <summary>
        /// Controls the autocomplete attribute of the input HTML element.
        /// Default = true
        /// </summary>
        [Parameter]
        public bool AutoComplete { get; set; } = true;

        /// <summary>
        /// Autofocus on the input or not
        /// </summary>
        /// <default value="false"/>
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
        /// Whether to change value on input
        /// </summary>
        [Parameter]
        public bool BindOnInput { get; set; }

        /// <summary>
        /// Delays the processing of the KeyUp event until the user has stopped
        /// typing for a predetermined amount of time. Default is 250 ms.
        /// </summary>
        [Parameter]
        public int DebounceMilliseconds
        {
            get => _debounceMilliseconds;
            set
            {
                _debounceMilliseconds = value;
                BindOnInput = value >= 0;
            }
        }

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
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        /// <summary>
        /// Callback when a key is released
        /// </summary>
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }

        /// <summary>
        /// Callback when a mouse button is released
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseUp { get; set; }

        /// <summary>
        /// The callback function that is triggered when Enter key is pressed
        /// </summary>
        [Parameter]
        public EventCallback<PressEnterEventArgs> OnPressEnter { get; set; }

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
        public InputType Type { get; set; } = AntDesign.InputType.Text;

        /// <summary>
        /// Set CSS style of wrapper. Is used when component has visible: Prefix/Suffix
        /// or has paramter set <seealso cref="AllowClear"/> or for components: <see cref="InputPassword"/>
        /// and <see cref="Search"/>. In these cases, html span elements is used
        /// to wrap the html input element.
        /// <seealso cref="WrapperStyle"/> is used on the span element.
        /// </summary>
        [Parameter]
        public string WrapperStyle { get; set; }

        /// <summary>
        /// Set Class of wrapper. Is used when component has visible: Prefix/Suffix
        /// or has paramter set <seealso cref="AllowClear"/> or for components: <see cref="InputPassword"/>
        /// and <see cref="Search"/>. In these cases, html span elements is used
        /// to wrap the html input element.
        /// <seealso cref="WrapperClass"/> is used on the span element.
        /// </summary>
        [Parameter]
        [PublicApi("1.2.0")]
        public string WrapperClass { get; set; }

        /// <summary>
        /// Show count of characters in the input
        /// </summary>
        [Parameter]
        public bool ShowCount { get; set; }

        /// <summary>
        /// The width of the input
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// When true, value will be set to empty string.
        /// When false, value will be set to <c>null</c> when content is empty or whitespace. 
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool DefaultToEmptyString { get; set; }

        protected Dictionary<string, object> Attributes { get; set; }

        protected ForwardRef WrapperRefBack { get; set; }

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
            if (Disabled)
                return;

            CurrentValue = default;
            IsFocused = true;
            _inputString = null;
            await this.FocusAsync(Ref);

            if (OnClear.HasDelegate)
                await OnClear.InvokeAsync(Value);
            else
                //Without the delay, focus is not enforced.
                await Task.Yield();
        }

        private string _inputString;

        private bool _compositionInputting;
        private Timer _debounceTimer;
        private bool _autoFocus;
        private bool _isInitialized;
        private int _debounceMilliseconds = 250;

        protected bool IsFocused { get; set; }

        private string CountString => $"{_inputString?.Length ?? 0}{(MaxLength > 0 ? $" / {MaxLength}" : "")}";

        private string WidthStyle => Width is { Length: > 0 } ? $"width:{(CssSizeLength)Width};" : "";

        private readonly Queue<Func<Task>> _afterValueChangedQueue = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrEmpty(DefaultValue?.ToString()) && string.IsNullOrEmpty(Value?.ToString()))
            {
                Value = DefaultValue;
            }

            _inputString = Value?.ToString();

            SetClasses();
            SetAttributes();
            _isInitialized = true;
        }

        protected void SetAttributes()
        {
            Attributes ??= [];

            if (MaxLength >= 0)
            {
                Attributes.TryAdd("maxlength", MaxLength);
            }
        }

        protected virtual void SetClasses()
        {
            AffixWrapperClass = $"{PrefixCls}-affix-wrapper {(IsFocused ? $"{PrefixCls}-affix-wrapper-focused" : "")} {(Bordered ? "" : $"{PrefixCls}-affix-wrapper-borderless")}";
            GroupWrapperClass = $"{PrefixCls}-group-wrapper";

            ClassMapper.Clear()
                .Add($"{PrefixCls}")
                .If($"{PrefixCls}-borderless", () => !Bordered)
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small)
                .If($"{PrefixCls}-rtl", () => RTL)
                .GetIf(() => $"{PrefixCls}-status-{FormItem?.ValidateStatus.ToString().ToLowerInvariant()}", () => FormItem is { ValidateStatus: not FormValidateStatus.Default })
                .If($"{InputElementSuffixClass}", () => !string.IsNullOrEmpty(InputElementSuffixClass))
                ;

            if (AllowClear)
            {
                AffixWrapperClass += $" {PrefixCls}-affix-wrapper-input-with-clear-btn";
            }

            if (Disabled)
            {
                AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-affix-wrapper-disabled");
                ClassMapper.Add($"{PrefixCls}-disabled");
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

            //if (ValidationMessages.Length > 0)
            //{
            //    AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-affix-wrapper-status-error");
            //}

            if (FormItem is { ValidateStatus: not FormValidateStatus.Default })
            {
                AffixWrapperClass += $" ant-input-affix-wrapper-status-{FormItem.ValidateStatus.ToString().ToLowerInvariant()}";
            }

            if (FormItem is { HasFeedback: true })
            {
                AffixWrapperClass += " ant-input-affix-wrapper-has-feedback";
            }
        }

        internal override void OnValidated(string[] validationMessages)
        {
            base.OnValidated(validationMessages);
            SetClasses();

            if (validationMessages.Length > 0 && !Attributes.ContainsKey("aria-invalid"))
            {
                Attributes.Add("aria-invalid", "true");
            }
            else
            {
                Attributes.Remove("aria-invalid");
            }
        }

        internal override void UpdateStyles()
        {
            base.UpdateStyles();
            SetClasses();
            InvokeAsync(StateHasChanged);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Form?.ValidateOnChange == true)
            {
                BindOnInput = true;
            }

            SetClasses();
            SetAttributes();
        }

        protected virtual Task OnChangeAsync(ChangeEventArgs args)
        {
            ChangeValue(true);
            return Task.CompletedTask;
        }

        protected void OnKeyPressAsync(KeyboardEventArgs args)
        {
            if (EnableOnPressEnter && (args?.Code == "Enter" || args?.Key == "Enter"))
            {
                CallAfterValueChanged(async () =>
                {
                    var enterArgs = new PressEnterEventArgs(args);
                    await OnPressEnterAsync(enterArgs);
                });
            }
        }

        protected virtual async Task OnPressEnterAsync(PressEnterEventArgs args)
        {
            await OnPressEnter.InvokeAsync(args);
        }

        protected void CallAfterValueChanged(Func<Task> workItem)
        {
            _afterValueChangedQueue.Enqueue(workItem);
            var original = BindOnInput;
            BindOnInput = true;
            // Ignoring textarea is to avoid ongoing line breaks being withdrawn due to bindings
            // However, if the user wishes to use the carriage return event to get the bound value, the value needs to be bound first
            ChangeValue(InputType != "textarea");
            BindOnInput = original;

            while (_afterValueChangedQueue.TryDequeue(out var task))
            {
                InvokeAsync(task.Invoke);
            }
        }

        protected async Task OnKeyUpAsync(KeyboardEventArgs args)
        {
            ChangeValue();

            if (OnKeyUp.HasDelegate) await OnKeyUp.InvokeAsync(args);
        }

        protected virtual async Task OnKeyDownAsync(KeyboardEventArgs args)
        {
            if (OnKeyDown.HasDelegate) await OnKeyDown.InvokeAsync(args);
        }

        protected async Task OnMouseUpAsync(MouseEventArgs args)
        {
            ChangeValue(true);

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

            ChangeValue(true);

            if (OnBlur.HasDelegate)
            {
                await OnBlur.InvokeAsync(e);
            }
        }

        private async Task OnFocusInternal(JsonElement e) => await OnFocusAsync(new());

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

        protected RenderFragment ClearIcon => builder =>
        {
            builder.OpenElement(31, "span");
            builder.AddAttribute(32, "class", $"{PrefixCls}-clear-icon " +
                (Suffix != null ? $"{PrefixCls}-clear-icon-has-suffix " : "") +
                (!ShowClear ?? string.IsNullOrEmpty(_inputString) || Disabled ? $"{PrefixCls}-clear-icon-hidden " : ""));

            builder.OpenComponent<Icon>(33);

            builder.AddAttribute(34, "Type", IconType.Fill.CloseCircle);
            builder.AddAttribute(35, "Theme", IconThemeType.Fill);

            builder.AddAttribute(36, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, async (args) =>
            {
                await Clear();
            }));

            builder.CloseComponent();
            builder.CloseElement();
        };

        protected RenderFragment Counter => builder =>
        {
            string counterCls = "ant-input-show-count-suffix";
            if (Suffix != null)
            {
                counterCls += " ant-input-show-count-has-suffix";
            }
            builder.AddMarkupContent(111, $@"<span class=""{counterCls}"">{CountString}</span>");
        };

        protected void DebounceChangeValue()
        {
            _debounceTimer?.Dispose();
            _debounceTimer = new Timer(DebounceTimerIntervalOnTick, null, DebounceMilliseconds, DebounceMilliseconds);
        }

        protected void DebounceTimerIntervalOnTick(object state)
        {
            InvokeAsync(() => ChangeValue(true));
        }

        protected void ChangeValue(bool force = false)
        {
            if (BindOnInput)
            {
                if (_debounceMilliseconds > 0 && !force)
                {
                    DebounceChangeValue();
                    return;
                }

                if (_debounceTimer != null)
                {
                    _debounceTimer.Dispose();
                    _debounceTimer = null;
                }
            }
            else if (!force)
            {
                return;
            }

            if (!_compositionInputting && CurrentValueAsString != _inputString)
            {
                CurrentValueAsString = _inputString;
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
            DomEventListener?.DisposeExclusive();
            _debounceTimer?.Dispose();

            base.Dispose(disposing);
        }

        protected override void OnValueChange(TValue value)
        {
            _inputString = CurrentValueAsString;
            base.OnValueChange(value);
        }

        protected override void OnCurrentValueChange(TValue value)
        {
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(Value);
            }

            base.OnCurrentValueChange(value);
        }

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(value) && DefaultToEmptyString && typeof(TValue) == typeof(string))
            {
                result = (TValue)(object)string.Empty;
                validationErrorMessage = null;
                return true;
            }

            return base.TryParseValueFromString(value, out result, out validationErrorMessage);
        }

        /// <summary>
        /// Invoked when user add/remove content
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual async Task OnInputAsync(ChangeEventArgs args)
        {
            _inputString = args?.Value.ToString();

            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args);
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            string container = "input";
            var hasSuffix = Suffix != null || AllowClear || FormItem?.FeedbackIcon != null || ShowCount;

            if (AddOnBefore != null || AddOnAfter != null)
            {
                container = "groupWrapper";
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", string.Join(" ", GroupWrapperClass, WrapperClass));
                builder.AddAttribute(3, "style", $"{WidthStyle} {WrapperStyle}");
                builder.OpenElement(4, "span");
                builder.AddAttribute(5, "class", $"{PrefixCls}-wrapper {PrefixCls}-group {(RTL ? $" {PrefixCls}-group-rtl" : "")}");
            }

            if (AddOnBefore != null)
            {
                // addOnBefore
                builder.OpenElement(11, "span");
                builder.AddAttribute(12, "class", $"{PrefixCls}-group-addon");
                builder.AddContent(13, AddOnBefore);
                builder.CloseElement();
            }

            if (Prefix != null || hasSuffix)
            {
                _hasAffixWrapper = true;
            }

            if (_hasAffixWrapper)
            {
                AffixWrapperClass = string.Join(" ", Class ?? "", AffixWrapperClass);
                ClassMapper.OriginalClass = "";

                builder.OpenElement(21, "span");
                builder.AddAttribute(22, "class", AffixWrapperClass);
                if (container == "input")
                {
                    container = "affixWrapper";
                    builder.AddAttribute(23, "style", $"{WidthStyle} {WrapperStyle}");
                }
                if (WrapperRefBack != null)
                {
                    builder.AddElementReferenceCapture(24, r => WrapperRefBack.Current = r);
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
            builder.AddAttribute(43, "style", $"{WidthStyle} {Style}");

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

            if (!string.IsNullOrWhiteSpace(NameAttributeValue))
            {
                builder.AddAttribute(46, "name", NameAttributeValue);
            }

            builder.AddAttribute(50, "id", Id);
            builder.AddAttribute(51, "type", Type);
            if (!string.IsNullOrWhiteSpace(Placeholder))
            {
                builder.AddAttribute(60, "placeholder", Placeholder);
            }
            builder.AddAttribute(61, "value", CurrentValueAsString);
            builder.AddAttribute(62, "disabled", needsDisabled);
            builder.AddAttribute(63, "readonly", ReadOnly);

            if (!AutoComplete)
            {
                builder.AddAttribute(64, "autocomplete", "off");
            }

            if (FormItem?.IsRequired ?? false)
            {
                builder.AddAttribute(65, "aria-required", true);
            }

            // onchange 和 onblur 事件会导致点击 OnSearch 按钮时不触发 Click 事件，暂时取消这两个事件
            //2022-8-3 去掉if后，search也能正常工作
            //if (!IgnoreOnChangeAndBlur)
            //{
            builder.AddAttribute(70, "onchange", CallbackFactory.Create(this, OnChangeAsync));
            builder.AddAttribute(71, "onblur", CallbackFactory.Create(this, OnBlurAsync));
            //}

            builder.AddAttribute(72, "onkeypress", CallbackFactory.Create(this, OnKeyPressAsync));
            builder.AddAttribute(73, "onkeydown", CallbackFactory.Create(this, OnKeyDownAsync));
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

            if (hasSuffix)
            {
                // suffix
                builder.OpenElement(91, "span");
                builder.AddAttribute(92, "class", $"{PrefixCls}-suffix");

                if (AllowClear)
                {
                    builder.AddContent(93, ClearIcon);
                }

                if (ShowCount)
                {
                    builder.AddContent(110, Counter);
                }

                if (Suffix != null)
                {
                    builder.AddContent(94, Suffix);
                }

                if (FormItem?.FeedbackIcon != null)
                {
                    builder.AddContent(95, FormItem.FeedbackIcon);
                }

                builder.CloseElement();
            }

            if (_hasAffixWrapper)
            {
                builder.CloseElement();
            }

            if (AddOnAfter != null)
            {
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
