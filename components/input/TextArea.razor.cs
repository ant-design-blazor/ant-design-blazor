using System;
using System.Diagnostics;
using System.Globalization;
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

        /// <summary>
        /// scrollHeight of 1 row
        /// </summary>
        private double _rowHeight;

        /// <summary>
        /// total height = row * <see cref="_rowHeight" /> + <see cref="_offsetHeight" />
        /// </summary>
        private double _offsetHeight;

        private uint _minRows = DEFAULT_MIN_ROWS;
        private uint _maxRows = uint.MaxValue;
        private bool _hasMinOrMaxSet;
        private DotNetObjectReference<TextArea> _reference;

        [Parameter]
        public bool AutoSize { get; set; }

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
                    AutoSize = true;
                }
                else
                {
                    _maxRows = uint.MaxValue;
                    Debug.WriteLine($"Value of {nameof(MaxRows)}({MaxRows}) has to be between {nameof(MinRows)}({MinRows}) and {uint.MaxValue}");
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
                if (value >= DEFAULT_MIN_ROWS && value <= MaxRows)
                {
                    _minRows = value;
                    AutoSize = true;
                }
                else
                {
                    _minRows = DEFAULT_MIN_ROWS;
                    Debug.WriteLine($"Value of {nameof(MinRows)}({MinRows}) has to be between {DEFAULT_MIN_ROWS} and {nameof(MaxRows)}({MaxRows})");
                }
            }
        }

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

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            if (AutoSize)
            {
                DomEventService.AddEventListener("window", "beforeunload", Reloading, false);

                await CalculateRowHeightAsync();
            }
        }

        protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (DefaultToEmptyString)
                    result = string.Empty;
                else
                    result = default;
                validationErrorMessage = null;
                return true;
            }

            var success = BindConverter.TryConvertTo<string>(
               value, CultureInfo.CurrentCulture, out var parsedValue);

            if (success)
            {
                result = parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else
            {
                result = default;
                validationErrorMessage = $"{FieldIdentifier.FieldName} field isn't valid.";

                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (AutoSize && !_isReloading)
            {
                _reference?.Dispose();
                DomEventService.RemoveEventListerner<JsonElement>("window", "beforeunload", Reloading);

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

        private void Reloading(JsonElement jsonElement) => _isReloading = true;

        [JSInvokable]
        public void ChangeSizeAsyncJs(float width, float height)
        {
            if (OnResize.HasDelegate)
                OnResize.InvokeAsync(new OnResizeEventArgs { Width = width, Height = height });
        }

        private async Task CalculateRowHeightAsync()
        {
            if (_reference == null)
            {
                _reference = DotNetObjectReference.Create<TextArea>(this);
            }
            var textAreaInfo = await JsInvokeAsync<TextAreaInfo>(JSInteropConstants.RegisterResizeTextArea, Ref, MinRows, MaxRows, _reference);

            //            var textAreaInfo = await JsInvokeAsync<TextAreaInfo>(JSInteropConstants.GetTextAreaInfo, Ref);
            _rowHeight = textAreaInfo.LineHeight;
            _offsetHeight = textAreaInfo.PaddingTop + textAreaInfo.PaddingBottom
                + textAreaInfo.BorderTop + textAreaInfo.BorderBottom;

            uint rows = (uint)(textAreaInfo.ScrollHeight / _rowHeight);
            if (_hasMinOrMaxSet)
                rows = Math.Max((uint)MinRows, rows);

            double height = 0;
            if (rows > MaxRows)
            {
                rows = MaxRows;

                height = rows * _rowHeight + _offsetHeight;
                Style = $"height: {height}px;";
            }
            else
            {
                height = rows * _rowHeight + _offsetHeight;
                Style = $"height: {height}px;overflow-y: hidden;";
            }
        }

        private class TextAreaInfo
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
