// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.Core.Extensions;
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

        protected const string InputWrapperClass = $"{PrefixCls}-wrapper";
        protected const string SuggestionWrapperClass = $"{PrefixCls}-suggestion-wrapper";
        protected const string SuggestionClass = $"{PrefixCls}-suggestion";
        protected const string SuggestionHiddenClass = $"{PrefixCls}-suggestion-hidden";
        protected const string SuggestionTextClass = $"{PrefixCls}-suggestion-text";

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
        /// The callback function that is triggered when input changes to get suggestion text
        /// </summary>
        [Parameter]
        public EventCallback<string> OnSuggestion { get; set; }

        /// <summary>
        /// The suggestion text that will be shown in gray after the input
        /// </summary>
        [Parameter]
        public string SuggestionText { get; set; }

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

        private string _internalSuggestionText;
        private bool _enableSuggestion;
        protected bool ShowSuggestion => !string.IsNullOrEmpty(_internalSuggestionText);
        protected bool EnableSuggestion => _enableSuggestion;
        protected string InternalSuggestionText => _internalSuggestionText;

        private void UpdateInternalSuggestion()
        {
            if (string.IsNullOrEmpty(SuggestionText) || string.IsNullOrEmpty(_inputString))
            {
                _internalSuggestionText = null;
                return;
            }

            if (SuggestionText.StartsWith(_inputString, StringComparison.CurrentCultureIgnoreCase))
            {
                _internalSuggestionText = SuggestionText[_inputString.Length..];
            }
            else
            {
                _internalSuggestionText = null;
            }

            StateHasChanged();
        }

        private void ConfigureClassMapper()
        {
            ClassMapper.Clear()
                .Add($"{PrefixCls}")
                .If($"{PrefixCls}-borderless", () => !Bordered)
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small)
                .If($"{PrefixCls}-rtl", () => RTL)
                .GetIf(() => $"{PrefixCls}-status-{FormItem?.ValidateStatus.ToString().ToLowerInvariant()}", () => FormItem is { ValidateStatus: not FormValidateStatus.Default })
                .If($"{InputElementSuffixClass}", () => !string.IsNullOrEmpty(InputElementSuffixClass));
        }

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

            ConfigureClassMapper();

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

            if (FormItem is { ValidateStatus: not FormValidateStatus.Default })
            {
                AffixWrapperClass += $" ant-input-affix-wrapper-status-{FormItem.ValidateStatus.ToString().ToLower()}";
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

            UpdateInternalSuggestion();
            SetClasses();
            SetAttributes();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            _enableSuggestion = parameters.TryGetValue(nameof(SuggestionText), out string suggestionText);
            return base.SetParametersAsync(parameters);
        }

        protected virtual Task OnChangeAsync(ChangeEventArgs args)
        {
            ChangeValue(true);
            return Task.CompletedTask;
        }

        protected void OnKeyPressAsync(KeyboardEventArgs args)
        {
            if (EnableOnPressEnter && args?.Key == "Enter")
            {
                CallAfterValueChanged(async () =>
                {
                    await OnPressEnter.InvokeAsync(args);
                    await OnPressEnterAsync();
                });
            }
        }

        protected virtual Task OnPressEnterAsync() => Task.CompletedTask;

        protected void CallAfterValueChanged(Func<Task> workItem)
        {
            _afterValueChangedQueue.Enqueue(workItem);
            var original = BindOnInput;
            BindOnInput = true;
            // Ignoring textarea is to avoid ongoing line breaks being withdrawn due to bindings
            // However, if the user wishes to use the carriage return event to get the bound value, the value needs to be bound first
            ChangeValue(InputType != "textarea");
            BindOnInput = original;
        }

        protected async Task OnKeyUpAsync(KeyboardEventArgs args)
        {
            ChangeValue();

            if (OnKeyUp.HasDelegate) await OnKeyUp.InvokeAsync(args);
        }

        protected virtual async Task OnkeyDownAsync(KeyboardEventArgs args)
        {
            if (ShowSuggestion && (args.Key == "ArrowRight" || args.Key == "Tab"))
            {
                if (args.Key == "Tab")
                {
                    await FocusAsync(Ref, FocusBehavior.FocusAndClear, true);
                }
                CallAfterRender(() =>
                {
                    ChangeValue(true);
                    return Task.CompletedTask;
                });
                ChangeValue(true);
                _inputString = SuggestionText;
                _internalSuggestionText = null;
            }

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
            while (_afterValueChangedQueue.TryDequeue(out var task))
            {
                InvokeAsync(task.Invoke);
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

            if (OnSuggestion.HasDelegate)
            {
                await OnSuggestion.InvokeAsync(_inputString);
            }

            UpdateInternalSuggestion();

            if (BindOnInput)
            {
                CurrentValueAsString = _inputString;
            }

            StateHasChanged();
        }

        protected virtual void BuildSuggestion(RenderTreeBuilder builder, ref int sequence)
        {
            if (ShowSuggestion)
            {
                builder.OpenElement(sequence++, "div");
                builder.AddAttribute(sequence++, "class", SuggestionClass);

                // Hidden input text
                builder.OpenElement(sequence++, "span");
                builder.AddAttribute(sequence++, "class", SuggestionHiddenClass);
                builder.AddContent(sequence++, _inputString);
                builder.CloseElement();

                // Full suggestion text
                builder.OpenElement(sequence++, "span");
                builder.AddAttribute(sequence++, "class", SuggestionTextClass);
                builder.AddContent(sequence++, _inputString + _internalSuggestionText);
                builder.CloseElement();

                builder.CloseElement();
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = 0;
            bool hasGroup = AddOnBefore != null || AddOnAfter != null;
            bool needsWrapper = EnableSuggestion && !_hasAffixWrapper && !hasGroup;
            bool hasSuffix = Suffix != null || AllowClear || FormItem?.FeedbackIcon != null || ShowCount;

            // Build the core input element
            void BuildInput(RenderTreeBuilder b)
            {
                b.WrapElement2(true, "input", (input, _) =>
                {
                    input.AddAttribute(seq++, "class", ClassMapper.Class);
                    input.AddAttribute(seq++, "style", $"{WidthStyle} {Style}");

                    if (Attributes != null)
                    {
                        foreach (var attr in Attributes)
                        {
                            input.AddAttribute(seq++, attr.Key, attr.Value);
                        }
                    }

                    if (AdditionalAttributes != null)
                    {
                        foreach (var attr in AdditionalAttributes)
                        {
                            input.AddAttribute(seq++, attr.Key, attr.Value);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(NameAttributeValue))
                    {
                        input.AddAttribute(seq++, "name", NameAttributeValue);
                    }

                    input.AddAttribute(seq++, "id", Id);
                    input.AddAttribute(seq++, "type", Type);

                    if (!string.IsNullOrWhiteSpace(Placeholder))
                    {
                        input.AddAttribute(seq++, "placeholder", Placeholder);
                    }

                    input.AddAttribute(seq++, "value", CurrentValueAsString);
                    input.AddAttribute(seq++, "disabled", Disabled);
                    input.AddAttribute(seq++, "readonly", ReadOnly);

                    if (!AutoComplete)
                    {
                        input.AddAttribute(seq++, "autocomplete", "off");
                    }

                    if (FormItem?.IsRequired ?? false)
                    {
                        input.AddAttribute(seq++, "aria-required", true);
                    }

                    input.AddAttribute(seq++, "onchange", CallbackFactory.Create(this, OnChangeAsync));
                    input.AddAttribute(seq++, "onblur", CallbackFactory.Create(this, OnBlurAsync));
                    input.AddAttribute(seq++, "onkeypress", CallbackFactory.Create(this, OnKeyPressAsync));
                    input.AddAttribute(seq++, "onkeydown", CallbackFactory.Create(this, OnkeyDownAsync));
                    input.AddAttribute(seq++, "onkeyup", CallbackFactory.Create(this, OnKeyUpAsync));
                    input.AddAttribute(seq++, "oninput", CallbackFactory.Create(this, OnInputAsync));
                    input.AddAttribute(seq++, "onmouseup", CallbackFactory.Create(this, OnMouseUpAsync));

                    if (StopPropagation)
                    {
                        input.AddEventStopPropagationAttribute(seq++, "onchange", true);
                        input.AddEventStopPropagationAttribute(seq++, "onblur", true);
                    }

                    input.AddElementReferenceCapture(seq++, r => Ref = r);
                }, _ => { }, seq);

                BuildSuggestion(b, ref seq);
            }

            // Build with suggestion wrapper if needed
            void BuildWithSuggestion(RenderTreeBuilder b)
            {
                b.WrapElement2(needsWrapper, "span", (wrapper, child) =>
                {
                    wrapper.AddAttribute(seq++, "class", $"{SuggestionWrapperClass} {WrapperClass}");
                    wrapper.AddAttribute(seq++, "style", $"{WidthStyle} {WrapperStyle}");
                    wrapper.AddContent(seq++, child);
                }, BuildInput, seq);
            }

            // Build with affix wrapper if needed
            void BuildWithAffix(RenderTreeBuilder b)
            {
                b.WrapElement2(_hasAffixWrapper, "span", (affixWrapper, child) =>
                {
                    AffixWrapperClass = string.Join(" ", Class ?? "", AffixWrapperClass, ShowSuggestion ? SuggestionWrapperClass : "");
                    ClassMapper.OriginalClass = "";

                    affixWrapper.AddAttribute(seq++, "class", AffixWrapperClass);
                    affixWrapper.AddAttribute(seq++, "style", $"{WidthStyle} {WrapperStyle}");

                    // Add prefix
                    if (Prefix != null)
                    {
                        affixWrapper.OpenElement(seq++, "span");
                        affixWrapper.AddAttribute(seq++, "class", $"{PrefixCls}-prefix");
                        affixWrapper.AddContent(seq++, Prefix);
                        affixWrapper.CloseElement();
                    }

                    affixWrapper.AddContent(seq++, child);

                    // Add suffix
                    if (hasSuffix)
                    {
                        affixWrapper.OpenElement(seq++, "span");
                        affixWrapper.AddAttribute(seq++, "class", $"{PrefixCls}-suffix");

                        if (AllowClear)
                        {
                            affixWrapper.AddContent(seq++, ClearIcon);
                        }

                        if (ShowCount)
                        {
                            affixWrapper.AddContent(seq++, Counter);
                        }

                        if (Suffix != null)
                        {
                            affixWrapper.AddContent(seq++, Suffix);
                        }

                        if (FormItem?.FeedbackIcon != null)
                        {
                            affixWrapper.AddContent(seq++, FormItem.FeedbackIcon);
                        }

                        affixWrapper.CloseElement();
                    }
                }, BuildWithSuggestion, seq);
            }

            // Build with group wrapper if needed
            void BuildWithGroup(RenderTreeBuilder b)
            {
                b.WrapElement2(hasGroup, "span", (wrapper, child) =>
                {
                    wrapper.AddAttribute(seq++, "class", string.Join(" ", GroupWrapperClass, WrapperClass, ShowSuggestion ? SuggestionWrapperClass : ""));
                    wrapper.AddAttribute(seq++, "style", $"{WidthStyle} {WrapperStyle}");

                    wrapper.OpenElement(seq++, "span");
                    wrapper.AddAttribute(seq++, "class", $"{InputWrapperClass} {PrefixCls}-group");

                    // Add before addon
                    if (AddOnBefore != null)
                    {
                        wrapper.OpenElement(seq++, "span");
                        wrapper.AddAttribute(seq++, "class", $"{PrefixCls}-group-addon");
                        wrapper.AddContent(seq++, AddOnBefore);
                        wrapper.CloseElement();
                    }

                    wrapper.AddContent(seq++, child);

                    // Add after addon
                    if (AddOnAfter != null)
                    {
                        wrapper.OpenElement(seq++, "span");
                        wrapper.AddAttribute(seq++, "class", $"{PrefixCls}-group-addon");
                        wrapper.AddContent(seq++, AddOnAfter);
                        wrapper.CloseElement();
                    }

                    wrapper.CloseElement();
                }, BuildWithAffix, seq);
            }

            // Start building from the outermost wrapper
            BuildWithGroup(builder);
        }
    }
}
