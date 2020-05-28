using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class PaginationOptions : AntDomComponentBase
    {
        /// <summary>
        /// 'default' | 'small' = 'default';
        /// </summary>
        [Parameter] public string Size { get; set; } = "default";

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public bool ShowSizeChanger { get; set; } = false;

        [Parameter] public bool ShowQuickJumper { get; set; } = false;

        [Parameter] public object Locale { get; set; }

        [Parameter] public int Total { get; set; } = 0;

        [Parameter] public int PageIndex { get; set; } = 1;

        [Parameter] public int PageSize { get; set; } = 10;

        [Parameter] public int[] PageSizeOptions { get; set; } = Array.Empty<int>();

        [Parameter] public EventCallback<int> PageIndexChange { get; set; }

        [Parameter] public EventCallback<int> PageSizeChange { get; set; }

        private string _inputValue;

        private (int value, string label)[] _listOfPageSizeOption = Array.Empty<(int, string)>();

        private void OnPageSizeChange(int size)
        {
            if (this.PageSize != size)
            {
                this.PageSizeChange.InvokeAsync(size);
            }
        }

        private void JumpToPageViaInput(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                var index = int.TryParse(_inputValue, out var value) && value > 0 ? value : this.PageIndex;
                PageIndexChange.InvokeAsync(index);
                _inputValue = "";
            }
        }
    }
}
