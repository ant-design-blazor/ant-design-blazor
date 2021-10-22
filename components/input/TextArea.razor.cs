using System.Diagnostics;
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
                if (_autoSize)
                {
                    _resizeStyle = "resize: none";
                }
                else
                {
                    _resizeStyle = "";
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
                DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);
            }
            await CalculateRowHeightAsync();
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
        private string _resizeStyle = "";

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

            uint rows = Rows;
            if (_hasMinSet)
                rows = MinRows;

            TextAreaInfo textAreaInfo;
            if (AutoSize)
            {
                textAreaInfo = await JsInvokeAsync<TextAreaInfo>(
                    JSInteropConstants.InputComponentHelper.RegisterResizeTextArea, Ref, rows, MaxRows, _reference);
            }
            else
            {
                textAreaInfo = await JsInvokeAsync<TextAreaInfo>(
                    JSInteropConstants.InputComponentHelper.GetTextAreaInfo, Ref);
            }

            _rowHeight = textAreaInfo.LineHeight;
            _offsetHeight = textAreaInfo.PaddingTop + textAreaInfo.PaddingBottom
                + textAreaInfo.BorderTop + textAreaInfo.BorderBottom;

            if (rows > MaxRows)
            {
                Style = $"height: {MaxRows * _rowHeight + _offsetHeight}px;{_resizeStyle};overflow-x: hidden";
            }
            else
            {
                string overflow = _autoSize ? "hidden" : "visible";
                Style = $"height: {rows * _rowHeight + _offsetHeight}px;overflow-y: {overflow};{_resizeStyle};overflow-x: hidden";
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
