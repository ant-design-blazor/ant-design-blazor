using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class PaginationSimple
    {
        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public object Locale { get; set; } = false;

        [Parameter]
        public int Total { get; set; } = 0;

        [Parameter]
        public int PageIndex { get; set; } = 1;

        [Parameter]
        public int PageSize { get; set; } = 10;

        [Parameter]
        public EventCallback<int> PageIndexChange { get; set; }

        [CascadingParameter]
        public Pagination Pagination { get; set; }

        private string _inputValue;

        private int _lastIndex = 0;
        private bool _isFirstIndex = false;
        private bool _isLastIndex = false;

        protected override void OnParametersSet()
        {
            this.UpdateBindingValue();
            base.OnParametersSet();
        }

        public void JumpToPageViaInput(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                var index = int.TryParse(_inputValue, out var value) && value > 0 ? value : this.PageIndex;
                PageIndexChange.InvokeAsync(index);
                _inputValue = $"{index}";
            }
        }

        private void PrePage()
        {
            this.OnPageIndexChange(this.PageIndex - 1);
        }

        private void NextPage()
        {
            this.OnPageIndexChange(this.PageIndex + 1);
        }

        private void OnPageIndexChange(int index)
        {
            this.PageIndexChange.InvokeAsync(index);
        }

        private void UpdateBindingValue()
        {
            this._lastIndex = (this.Total - 1) / this.PageSize + 1;
            this._inputValue = $"{this.PageIndex}";
            this._isFirstIndex = this.PageIndex == 1;
            this._isLastIndex = this.PageIndex == this._lastIndex;
        }
    }
}
