using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntSearch : AntInputBase
    {
        private bool _isSearching;

        [Parameter]
        public EventCallback<EventArgs> onSearch { get; set; }

        protected override void SetClasses()
        {
            base.SetClasses();

            if (size == AntInputSize.Large)
            {
                _groupClass = string.Join(" ", _groupClass, $"{PrefixCls}-search-large");
            }
            else if (size == AntInputSize.Small)
            {
                _groupClass = string.Join(" ", _groupClass, $"{PrefixCls}-search-small");
            }
        }

        private async void Search()
        {
            _isSearching = true;
            StateHasChanged();
            if (onSearch.HasDelegate)
            {
                await onSearch.InvokeAsync(EventArgs.Empty);
            }
            await Task.Delay(TimeSpan.FromSeconds(10));
            _isSearching = false;
            StateHasChanged();
        }
    }
}