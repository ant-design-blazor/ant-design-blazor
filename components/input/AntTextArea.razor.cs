using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using System;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntTextArea : AntInput
    {
        /// <summary>
        /// scrollHeight of 1 row
        /// </summary>
        private double _rowHeight;

        /// <summary>
        /// total height = row * _rowHeight + _offsetHeight
        /// </summary>

        private double _offsetHeight;

        private string _hiddenWidth;

        private ElementReference _hiddenEle;

        [Parameter]
        public bool AutoSize { get; set; }

        private uint _minRows = 1;
        [Parameter]
        public uint MinRows
        {
            get
            {
                return _minRows;
            }
            set
            {
                if (value >= 1 && value <= MaxRows)
                {
                    _minRows = value;
                    AutoSize = true;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(MinRows));
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
                if (value <= uint.MaxValue && value >= MinRows)
                {
                    _maxRows = value;
                    AutoSize = true;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(MinRows));
                }
            }
        }

        [Parameter]
        public EventCallback<object> OnResize { get; set; }

        protected override async void OnInitialized()
        {
            base.OnInitialized();
        }

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
            // do not call base method to avoid lost focus
            //base.OnInputAsync(args);

            Value = args.Value.ToString();

            if (AutoSize)
            {
                await ChangeSizeAsync();
            }
        }

        private async Task ChangeSizeAsync()
        {
            // Ant-design use a hidden textarea to calculate row height, totalHeight = rows * rowHeight
            // TODO: compare with maxheight

            Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _hiddenEle);
            System.Diagnostics.Debug.WriteLine($"hidden\t{element.scrollHeight}");

            // do not use %mod in case _rowheight is not an integer
            uint rows = (uint)(element.scrollHeight / _rowHeight);
            rows = Math.Max((uint)MinRows, rows);
            if (rows > MaxRows)
            {
                rows = MaxRows;
                Style = $"height: {rows * _rowHeight + _offsetHeight}px;";
            }
            else
            {
                Style = $"height: {rows * _rowHeight + _offsetHeight}px;overflow-y: hidden;";
            }
            rows = Math.Min((uint)MaxRows, rows);
        }

        protected async Task OnResizeAsync()
        {
        }

        private async Task CalculateRowHeightAsync()
        {
            Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, inputEl);
            element.ToString();
            _hiddenWidth = $"width: {element.offsetWidth}px;";

            // save the content in the textarea
            string str = Value;

            // total height of 1 row
            Value = " ";
            StateHasChanged();
            element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _hiddenEle);
            double rHeight = element.scrollHeight;

            // total height of 2 rows
            Value = " \r\n ";
            StateHasChanged();
            element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _hiddenEle);
            double rrHeight = element.scrollHeight;

            _rowHeight = rrHeight - rHeight;
            _offsetHeight = rHeight - _rowHeight;

            // revert the value back to original content
            Value = str;
            StateHasChanged();
            await ChangeSizeAsync();
        }
    }
}