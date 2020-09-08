using System;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

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

        private string _hiddenWidth;

        private ElementReference _hiddenEle;

        [Parameter]
        public bool AutoSize { get; set; }

        private uint _minRows = DEFAULT_MIN_ROWS;

        [Parameter]
        public uint MinRows
        {
            get
            {
                return _minRows;
            }
            set
            {
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

        private uint _maxRows = uint.MaxValue;

        [Parameter]
        public uint MaxRows
        {
            get
            {
                return _maxRows;
            }
            set
            {
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
                await CalculateRowHeightAsync();
            }
        }

        protected override async void OnInputAsync(ChangeEventArgs args)
        {
            base.OnInputAsync(args);

            if (AutoSize)
            {
                await ChangeSizeAsync();
            }
        }

        private async Task ChangeSizeAsync()
        {
            // Ant-design use a hidden textarea to calculate row height, totalHeight = rows * rowHeight
            // TODO: compare with maxheight

            Element element = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _hiddenEle);
            System.Diagnostics.Debug.WriteLine($"hidden\t{element.scrollHeight}");

            // do not use %mod in case _rowheight is not an integer
            uint rows = (uint)(element.scrollHeight / _rowHeight);
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

            await OnResize.InvokeAsync(new OnResizeEventArgs { Width = element.scrollWidth, Height = height });
        }

        private async Task CalculateRowHeightAsync()
        {
            Element element = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, Ref);
            element.ToString();
            _hiddenWidth = $"width: {element.offsetWidth}px;";

            // save the content in the textarea
            string str = Value;

            // total height of 1 row
            Value = " ";
            StateHasChanged();
            element = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _hiddenEle);
            double rHeight = element.scrollHeight;

            // total height of 2 rows
            Value = " \r\n ";
            StateHasChanged();
            element = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _hiddenEle);
            double rrHeight = element.scrollHeight;

            _rowHeight = rrHeight - rHeight;
            _offsetHeight = rHeight - _rowHeight;

            // revert the value back to original content
            Value = str;
            StateHasChanged();
            await ChangeSizeAsync();
        }

        protected override string GetClearIconCls()
        {
            return $"{PrefixCls}-textarea-clear-icon";
        }
    }
}
