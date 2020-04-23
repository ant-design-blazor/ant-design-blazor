using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntSearch : AntInput
    {
        private bool _isSearching;

        [Parameter]
        public EventCallback<EventArgs> OnSearch { get; set; }


        [Parameter]
        public OneOf<bool, string> EnterButton { get; set; }

        private int _sequence = 0;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (EnterButton.Value == null)
            {
                Suffix = new RenderFragment((builder) =>
                {
                    builder.OpenComponent<AntIcon>(35);
                    builder.AddAttribute(36, "class", $"{PrefixCls}-search-icon");
                    builder.AddAttribute(37, "type", "search");
                    builder.AddAttribute(38, "onclick", _callbackFactory.Create<MouseEventArgs>(this, Search));
                    builder.CloseComponent();
                });
            }
            else
            {
                AddOnAfter = new RenderFragment((builder) =>
                {
                    builder.OpenComponent<AntButton>(_sequence++);
                    builder.AddAttribute(_sequence++, "class", $"{PrefixCls}-search-button");
                    builder.AddAttribute(_sequence++, "type", "primary");
                    builder.AddAttribute(_sequence++, "size", Size);
                    if (_isSearching)
                    {
                        builder.AddAttribute(_sequence++, "loading", true);
                    }
                    else
                    {
                        EventCallback<MouseEventArgs> e = new EventCallbackFactory().Create(this, Search);
                        builder.AddAttribute(_sequence++, "onclick", e);
                    }


                    EnterButton.Switch(boolean =>
                    {
                        if (boolean)
                        {
                            builder.AddAttribute(_sequence++, "icon", "search");
                        }
                    }, str =>
                    {
                        builder.AddAttribute(_sequence++, "ChildContent", new RenderFragment((b) =>
                        {
                            b.AddContent(_sequence++, str);
                        }));
                    });

                    builder.CloseComponent();
                });
            }
        }

        protected override void SetClasses()
        {
            base.SetClasses();

            if (Size == AntInputSize.Large)
            {
                _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-search-large");
            }
            else if (Size == AntInputSize.Small)
            {
                _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-search-small");
            }

            _affixWrapperClass = string.Join(" ", _affixWrapperClass, $"{PrefixCls}-search");
            _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-search");
            _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-search-enter-button");
        }

        private async void Search(MouseEventArgs args)
        {
            _isSearching = true;
            StateHasChanged();
            if (OnSearch.HasDelegate)
            {
                await OnSearch.InvokeAsync(EventArgs.Empty);
            }
            await Task.Delay(TimeSpan.FromSeconds(10));
            _isSearching = false;
            StateHasChanged();
        }
    }
}
