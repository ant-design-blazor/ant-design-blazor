using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System;
using System.Threading.Tasks;

namespace AntDesign
{
    public partial class Search : Input<string>
    {
        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public EventCallback<string> OnSearch { get; set; }

        [Parameter]
        public OneOf<bool, string> EnterButton { get; set; } = false;

        protected override bool IgnoreOnChangeAndBlur => OnSearch.HasDelegate;

        protected override bool EnableOnPressEnter => OnSearch.HasDelegate || OnPressEnter.HasDelegate;

        private int _sequence = 0;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (EnterButton.IsT0 && !EnterButton.AsT0)
            {
                Suffix = builder =>
                {
                    var i = 0;
                    builder.OpenComponent<Icon>(i++);
                    builder.AddAttribute(i++, "Class", $"{PrefixCls}-search-icon");
                    if (Loading)
                    {
                        builder.AddAttribute(i++, "Type", "loading");
                    }
                    else
                    {
                        builder.AddAttribute(i++, "Type", "search");
                    }
                    builder.AddAttribute(i++, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, HandleSearch));
                    builder.CloseComponent();
                };
            }
            else
            {
                AddOnAfter = builder =>
                {
                    builder.OpenComponent<Button>(_sequence++);
                    builder.AddAttribute(_sequence++, "Class", $"{PrefixCls}-search-button");
                    builder.AddAttribute(_sequence++, "Type", "primary");
                    builder.AddAttribute(_sequence++, "Size", Size);

                    if (Loading)
                    {
                        builder.AddAttribute(_sequence++, "Loading", true);
                    }
                    else
                    {
                        builder.AddAttribute(_sequence++, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, HandleSearch));
                    }

                    EnterButton.Switch(boolean =>
                    {
                        if (boolean)
                        {
                            builder.AddAttribute(_sequence++, "Icon", "search");
                        }
                    }, str =>
                    {
                        builder.AddAttribute(_sequence++, "ChildContent", new RenderFragment((b) =>
                        {
                            b.AddContent(_sequence++, str);
                        }));
                    });

                    builder.CloseComponent();
                };
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

        private async Task HandleSearch(MouseEventArgs args)
        {
            await SearchAsync();
        }

        protected override async Task OnPressEnterAsync()
        {
            await SearchAsync();
            await base.OnPressEnterAsync();
        }

        private async Task SearchAsync()
        {
            Loading = true;
            if (OnSearch.HasDelegate)
            {
                await OnSearch.InvokeAsync(CurrentValue);
            }
            Loading = false;
        }
    }
}
