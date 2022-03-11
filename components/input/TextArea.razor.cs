﻿using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
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
        /// Will adjust (grow or shrink) the `TextArea` according to content. 
        /// Can work in connection with `MaxRows` and `MinRows`.
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
        /// When `false`, value will be set to `null` when content is empty 
        /// or whitespace. When `true`, value will be set to empty string.        
        /// </summary>
        [Parameter]
        public bool DefaultToEmptyString { get; set; }

        /// <summary>
        /// `TextArea` will allow growing, but it will stop when visible 
        /// rows = MaxRows (will not grow further).
        /// Default value = uint.MaxValue
        /// </summary>
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
        /// `TextArea` will allow shrinking, but it will stop when visible 
        /// rows = MinRows (will not shrink further).
        /// Default value = DEFAULT_MIN_ROWS = 1
        /// </summary>
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
        /// Default value is 3.
        /// </summary>
        [Parameter]
        public uint Rows { get; set; } = 3;

        /// <summary>
        /// Callback when the size changes
        /// </summary>
        [Parameter]
        public EventCallback<OnResizeEventArgs> OnResize { get; set; }

        /// <summary>
        /// Show character counting.
        /// </summary>
        [Parameter]
        public bool ShowCount
        {
            get => _showCount && MaxLength >= 0;
            set => _showCount = value;
        }

        /// <inheritdoc/>
        [Parameter]
        public override string Value
        {
            get => base.Value;
            set
            {
                if (base.Value != value)
                {
                    _valueHasChanged = true;
                }
                base.Value = value;
            }
        }

        private uint InnerMinRows => _hasMinSet ? MinRows : Rows;

        private bool _showCount;

        private ClassMapper _warpperClassMapper = new ClassMapper();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _warpperClassMapper
                .Get(() => $"{PrefixCls}-affix-wrapper")
                .Get(() => $"{PrefixCls}-affix-wrapper-textarea-with-clear-btn")
                .GetIf(() => $"{PrefixCls}-affix-wrapper-rtl", () => RTL);
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

        protected async override Task OnFirstAfterRenderAsync()
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
            if (AutoSize && _valueHasChanged)
            {
                _valueHasChanged = false;
                if (_isInputing)
                {
                    _isInputing = false;
                }
                else
                {
                    await JsInvokeAsync(JSInteropConstants.InputComponentHelper.ResizeTextArea, Ref, InnerMinRows, MaxRows);
                }
            }
            if (_styleHasChanged)
            {
                _styleHasChanged = false;
                if (AutoSize && !string.IsNullOrWhiteSpace(Style))
                {
                    await JsInvokeAsync(JSInteropConstants.StyleHelper.SetStyle, Ref, Style);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnInputAsync(ChangeEventArgs args)
        {
            _isInputing = true;
            base.OnInputAsync(args);
        }

        protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
        {
            validationErrorMessage = null;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (DefaultToEmptyString)
                    result = string.Empty;
                else
                    result = default;
                return true;
            }
            result = value;
            return true;

        }

        protected override void Dispose(bool disposing)
        {
            if (AutoSize && !_isReloading)
            {
                _reference?.Dispose();
                DomEventListener.Dispose();

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

        internal class TextAreaInfo
        {
            public double ScrollHeight { get; set; }
            public double LineHeight { get; set; }
            public double PaddingTop { get; set; }
            public double PaddingBottom { get; set; }
            public double BorderTop { get; set; }
            public double BorderBottom { get; set; }
        }
    }
}
