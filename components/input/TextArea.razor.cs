using System;
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
                    throw new ArgumentOutOfRangeException(nameof(MinRows), $"Please enter a value between {DEFAULT_MIN_ROWS} and {MaxRows}");
                }
            }
        }

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
                    throw new ArgumentOutOfRangeException($"Please enter a value between {MinRows} and {uint.MaxValue}");
                }
            }
        }

        [Parameter]
        public EventCallback<OnResizeEventArgs> OnResize { get; set; }

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            if (AutoSize)
            {
                DomEventService.AddEventListener("window", "beforeunload", Reloading, false);

                await CalculateRowHeightAsync();
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
            _offsetHeight = textAreaInfo.PaddingTop + textAreaInfo.PaddingBottom;

            uint rows = (uint)(textAreaInfo.ScrollHeight / _rowHeight);
            if (_hasMinOrMaxSet)
                rows = Math.Max((uint)MinRows, rows);

            int height = 0;
            if (rows > MaxRows)
            {
                rows = MaxRows;

                height = (int)(rows * _rowHeight + _offsetHeight);
                Style = $"height: {height}px;";
            }
            else
            {
                height = (int)(rows * _rowHeight + _offsetHeight);
                Style = $"height: {height}px;overflow-y: hidden;";
            }
        }

        protected override string GetClearIconCls()
        {
            return $"{PrefixCls}-textarea-clear-icon";
        }

        private class TextAreaInfo
        {
            public double ScrollHeight { get; set; }
            public double LineHeight { get; set; }
            public double PaddingTop { get; set; }
            public double PaddingBottom { get; set; }
        }
    }
}
