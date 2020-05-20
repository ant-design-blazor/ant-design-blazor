using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class Search : Input
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
                    builder.AddAttribute(38, "onclick", CallbackFactory.Create<MouseEventArgs>(this, HandleSearch));
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
                        var e = new EventCallbackFactory().Create(this, HandleSearch);
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

            if (Size == InputSize.Large)
            {
                GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-search-large");
            }
            else if (Size == InputSize.Small)
            {
                GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-search-small");
            }

            AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-search");
            GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-search");
            GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-search-enter-button");
        }

        private async void HandleSearch(MouseEventArgs args)
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
