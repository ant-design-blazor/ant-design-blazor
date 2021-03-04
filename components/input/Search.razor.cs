using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
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


        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (EnterButton.IsT0 && !EnterButton.AsT0)
            {
                Suffix = builder =>
                {
                    builder.OpenComponent<Icon>(1);
                    builder.AddAttribute(2, "Class", $"{PrefixCls}-search-icon");
                    if (Loading)
                    {
                        builder.AddAttribute(3, "Type", "loading");
                    }
                    else
                    {
                        builder.AddAttribute(4, "Type", "search");
                    }
                    builder.AddAttribute(5, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, HandleSearch));
                    builder.CloseComponent();
                };
            }
            else
            {
                AddOnAfter = builder =>
                {
                    builder.OpenComponent<Button>(6);
                    builder.AddAttribute(7, "Class", $"{PrefixCls}-search-button");
                    builder.AddAttribute(8, "Type", "primary");
                    builder.AddAttribute(9, "Size", Size);
                    builder.AddAttribute(10, "Loading", Loading);
                    if (!Loading)
                    {
                        builder.AddAttribute(11, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, HandleSearch));
                    }

                    EnterButton.Switch(boolean =>
                    {
                        if (boolean)
                        {
                            builder.AddAttribute(12, "Icon", "search");
                        }
                    }, str =>
                    {
                        builder.AddAttribute(13, "ChildContent", new RenderFragment((b) =>
                        {
                            b.AddContent(14, str);
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
