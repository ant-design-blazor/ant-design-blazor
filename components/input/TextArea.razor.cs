// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class TextArea : Input<string>
    {
        private const uint DEFAULT_MIN_ROWS = 1;

        protected override string InputType => "textarea";

        private uint _minRows = DEFAULT_MIN_ROWS;
        private uint _maxRows = uint.MaxValue;
        private bool _hasMinOrMaxSet;
        private bool _hasMinSet;
        private DotNetObjectReference<TextArea> _reference;

        /// <summary>
        /// Will adjust (grow or shrink) the <c>TextArea</c> according to content.
        /// Can work in connection with <see cref="MaxRows"/> and <see cref="MinRows"/>.
        /// Sets resize attribute of the textarea HTML element to: none.
        /// </summary>
        [Parameter]
        public bool AutoSize
        {
            get => _autoSize;
            set
            {
                if (_hasMinOrMaxSet && !value)
                {
                    Debug.WriteLine("AntBlazor.TextArea: AutoSize cannot be set to false when either MinRows or MaxRows has been set.AutoSize has been switched to true.");
                    _autoSize = true;
                }
                else
                {
                    _autoSize = value;
                }
            }
        }

        /// <summary>
        /// Allow growing, but stop when visible rows = MaxRows (will not grow further).
        /// </summary>
        /// <default value="uint.MaxValue"/>
        [Parameter]
        public uint MaxRows
        {
            get
            {
                return _maxRows;
            }
            set
            {
                _hasMinOrMaxSet = true;
                if (value >= MinRows)
                {
                    _maxRows = value;
                    Debug.WriteLineIf(!AutoSize, "AntBlazor.TextArea: AutoSize cannot be set to false when either MinRows or MaxRows has been set.AutoSize has been switched to true.");
                    AutoSize = true;
                }
                else
                {
                    _maxRows = uint.MaxValue;
                    Debug.WriteLine($"AntBlazor.TextArea: Value of {nameof(MaxRows)}({MaxRows}) has to be between {nameof(MinRows)}({MinRows}) and {uint.MaxValue}");
                }
            }
        }

        /// <summary>
        /// Allow shrinking, but stop when visible rows = MinRows (will not shrink further).
        /// </summary>
        /// <default value="1"/>
        [Parameter]
        public uint MinRows
        {
            get
            {
                return _minRows;
            }
            set
            {
                _hasMinOrMaxSet = true;
                _hasMinSet = true;
                if (value >= DEFAULT_MIN_ROWS && value <= MaxRows)
                {
                    _minRows = value;
                    Debug.WriteLineIf(!AutoSize, "AntBlazor.TextArea: AutoSize cannot be set to false when either MinRows or MaxRows has been set.AutoSize has been switched to true.");
                    AutoSize = true;
                }
                else
                {
                    _minRows = DEFAULT_MIN_ROWS;
                    Debug.WriteLine($"AntBlazor.TextArea: Value of {nameof(MinRows)}({MinRows}) has to be between {DEFAULT_MIN_ROWS} and {nameof(MaxRows)}({MaxRows})");
                }
            }
        }

        /// <summary>
        /// Sets the height of the TextArea expressed in number of rows.
        /// </summary>
        /// <default value="3"/>
        [Parameter]
        public uint Rows { get; set; } = 2;

        /// <summary>
        /// Callback executed when the size changes
        /// </summary>
        [Parameter]
        public EventCallback<OnResizeEventArgs> OnResize { get; set; }

        /// <summary>
        /// Gets or sets the value of the TextArea.
        /// </summary>
        [Parameter]
        public override string Value
        {
            get => base.Value;
            set
            {
                if (base.Value != value)
                {
                    _valueHasChanged = true;
                    _inputString = value;
                }
                base.Value = value;
            }
        }

        private uint InnerMinRows => _hasMinSet ? MinRows : Rows;
        private string Count => $"{_inputString?.Length ?? 0}{(MaxLength > 0 ? $" / {MaxLength}" : "")}";

        private string _inputString;

        private ClassMapper _warpperClassMapper = new();
        private ClassMapper _textareaClassMapper = new();

        private bool _afterFirstRender = false;
        private PressEnterEventArgs _duringPressEnterArgs;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _warpperClassMapper
                .Add($"{PrefixCls}-affix-wrapper")
                .If($"{PrefixCls}-affix-wrapper-textarea-with-clear-btn", () => AllowClear)
                .If($"{PrefixCls}-affix-wrapper-has-feedback", () => FormItem?.HasFeedback == true)
                .GetIf(() => $"{PrefixCls}-affix-wrapper-status-{FormItem?.ValidateStatus.ToString().ToLowerInvariant()}", () => FormItem is { ValidateStatus: not FormValidateStatus.Default })
                .If($"{PrefixCls}-affix-wrapper-rtl", () => RTL)
                .GetIf(() => $"{PrefixCls}-affix-wrapper-disabled", () => Disabled)
                ;

            ClassMapper
                .Add("ant-input-textarea ")
                .If("ant-input-textarea-show-count", () => ShowCount)
                .If("ant-input-textarea-in-form-item", () => FormItem != null)
                .If("ant-input-textarea-has-feedback", () => FormItem?.HasFeedback == true)
                .GetIf(() => $"ant-input-textarea-status-{FormItem?.ValidateStatus.ToString().ToLowerInvariant()}", () => FormItem is { ValidateStatus: not FormValidateStatus.Default })
                ;

            _textareaClassMapper
                .Add("ant-input")
                .If("ant-input-borderless", () => !Bordered)
                .GetIf(() => $"ant-input-status-{FormItem?.ValidateStatus.ToString().ToLowerInvariant()}", () => FormItem is { ValidateStatus: not FormValidateStatus.Default })
                ;
        }

        protected override void SetClasses()
        {
            //  override the classmapper setting
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_oldStyle != Style)
            {
                _styleHasChanged = true;
                _oldStyle = Style;
            }
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            if (AutoSize)
            {
                DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);
            }
            await RegisterResizeEvents();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                _afterFirstRender = true;
            }

            if (_afterFirstRender)
            {
                if (AutoSize && _valueHasChanged)
                {
                    _valueHasChanged = false;
                    if (_isInputing)
                    {
                        _isInputing = false;
                    }
                    else if (_afterFirstRender)
                    {
                        await JsInvokeAsync(JSInteropConstants.InputComponentHelper.ResizeTextArea, Ref, InnerMinRows, MaxRows);
                    }
                }
                if (_styleHasChanged)
                {
                    _styleHasChanged = false;
                    if (AutoSize && !string.IsNullOrWhiteSpace(Style) && _afterFirstRender)
                    {
                        await JsInvokeAsync(JSInteropConstants.StyleHelper.SetStyle, Ref, Style);
                    }
                }
            }
        }

        /// <inheritdoc/>
        /// OnPress -> OnInput, so we remove the line break here after pressing enter.
        protected override async Task OnInputAsync(ChangeEventArgs args)
        {
            _isInputing = true;
            _inputString = args.Value.ToString();
            await base.OnInputAsync(args);

            if (_duringPressEnterArgs != null)
            {
                if (_duringPressEnterArgs.ShouldPreventLineBreak)
                {
                    if (_inputString?.EndsWith('\n') == true)
                    {
                        // Only remove the last line break
                        if (_inputString.EndsWith("\r\n"))
                        {
                            _inputString = _inputString[..^2];
                        }
                        else
                        {
                            _inputString = _inputString[..^1];
                        }
                        ForceUpdateValueString(_inputString);
                    }
                }

                _duringPressEnterArgs = null;
            }

            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args);
            }
        }

        protected override void OnCurrentValueChange(string value)
        {
            base.OnCurrentValueChange(value);
            _inputString = value;
        }

        protected override void Dispose(bool disposing)
        {
            if (AutoSize && !_isReloading)
            {
                _reference?.Dispose();
                DomEventListener?.Dispose();

                _ = InvokeAsync(async () =>
                {
                    await JsInvokeAsync(JSInteropConstants.DisposeResizeTextArea, Ref);
                });
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Indicates that a page is being refreshed
        /// </summary>
        private bool _isReloading;

        private bool _autoSize;
        private bool _valueHasChanged;
        private bool _isInputing;
        private string _oldStyle;
        private bool _styleHasChanged;
        private string _heightStyle;

        private void Reloading(JsonElement jsonElement) => _isReloading = true;

        [JSInvokable]
        public void ChangeSizeAsyncJs(float width, float height)
        {
            if (OnResize.HasDelegate)
                OnResize.InvokeAsync(new OnResizeEventArgs { Width = width, Height = height });
        }

        private async Task RegisterResizeEvents()
        {
            if (_reference == null)
            {
                _reference = DotNetObjectReference.Create<TextArea>(this);
            }

            if (AutoSize)
            {
                await JsInvokeAsync<TextAreaInfo>(
                    JSInteropConstants.InputComponentHelper.RegisterResizeTextArea, Ref, InnerMinRows, MaxRows, _reference);
            }
            else
            {
                var textAreaInfo = await JsInvokeAsync<TextAreaInfo>(
                    JSInteropConstants.InputComponentHelper.GetTextAreaInfo, Ref);

                var rowHeight = textAreaInfo.LineHeight;
                var offsetHeight = textAreaInfo.PaddingTop + textAreaInfo.PaddingBottom
                    + textAreaInfo.BorderTop + textAreaInfo.BorderBottom;

                _heightStyle = $"height: {Rows * rowHeight + offsetHeight}px;overflow-y: auto;overflow-x: hidden;";
                StateHasChanged();
            }
        }

        protected override async Task OnPressEnterAsync(PressEnterEventArgs args)
        {
            _duringPressEnterArgs = args;

            await base.OnPressEnterAsync(_duringPressEnterArgs);

            // add new line when pressing other key link ctrl
            if (!_duringPressEnterArgs.ShouldPreventLineBreak && args.Key != "Enter" && args.Key != "NumpadEnter")
            {
                _inputString = _inputString + '\n';
                ForceUpdateValueString(_inputString);
            }
        }
    }
}
