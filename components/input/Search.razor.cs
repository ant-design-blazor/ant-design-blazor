﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System;
using System.Threading.Tasks;

namespace AntDesign
{
    public partial class Search : Input<string>
    {
        /// <summary>
        /// Search input is rendered with suffix search icon, not as a button.
        /// Will be ignored when EnterButton != false
        /// </summary>
        [Parameter]
        public bool ClassicSearchIcon { get; set; }

        /// <summary>
        /// Whether to show an enter button after input. This property conflicts with the AddonAfter property
        /// </summary>
        [Parameter]
        public OneOf<bool, string> EnterButton { get; set; } = false;

        /// <summary>
        /// Search box with loading
        /// </summary>
        [Parameter]
        public bool Loading { get; set; }

        /// <summary>
        /// The callback function triggered when you click on the search-icon, the clear-icon or press the Enter key
        /// </summary>
        [Parameter]
        public EventCallback<string> OnSearch { get; set; }

        protected override bool IgnoreOnChangeAndBlur => OnSearch.HasDelegate;

        protected override bool EnableOnPressEnter => OnSearch.HasDelegate || OnPressEnter.HasDelegate;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (EnterButton.IsT0 && !EnterButton.AsT0)
            {
                if (ClassicSearchIcon)
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
                        builder.AddAttribute(8, "Type", "default");
                        builder.AddAttribute(9, "Size", Size);
                        builder.AddAttribute(10, "Loading", Loading);
                        if (!Loading)
                        {
                            builder.AddAttribute(12, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, HandleSearch));
                        }
                        builder.AddAttribute(13, "Icon", "search");

                        builder.CloseComponent();
                    };
                }
            }
            else
            {
                AddOnAfter = builder =>
                {
                    builder.OpenComponent<Button>(11);
                    builder.AddAttribute(12, "Class", $"{PrefixCls}-search-button");
                    builder.AddAttribute(13, "Type", "primary");
                    builder.AddAttribute(14, "Size", Size);
                    builder.AddAttribute(15, "Loading", Loading);
                    if (!Loading)
                    {
                        builder.AddAttribute(16, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, HandleSearch));
                    }

                    EnterButton.Switch(boolean =>
                    {
                        if (boolean)
                        {
                            builder.AddAttribute(17, "ChildContent", new RenderFragment((b) =>
                            {
                                b.OpenComponent<Icon>(20);
                                b.AddAttribute(21, "Type", "search");
                                b.CloseComponent();
                            }));
                        }
                    }, str =>
                    {
                        builder.AddAttribute(18, "ChildContent", new RenderFragment((b) =>
                        {
                            b.AddContent(19, str);
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
            GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-search ant-input-group-wrapper");
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
