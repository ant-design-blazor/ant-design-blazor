// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class Search : Input<string>
    {
        /// <summary>
        /// Search input is rendered with suffix search icon, not as a button.
        /// Will be ignored when <see cref="EnterButton"/> != false
        /// </summary>
        [Parameter]
        public bool ClassicSearchIcon { get; set; }

        /// <summary>
        /// Whether to show an enter button after input. This property conflicts with the <see cref="Input{TValue}.AddOnAfter"/>
        /// </summary>
        [Parameter]
        public OneOf<bool, string> EnterButton { get; set; } = false;

        /// <summary>
        /// Search box with loading
        /// </summary>
        [Parameter]
        public bool Loading { get; set; }

        /// <summary>
        /// Callback executed when you click on the search-icon, the clear-icon or press the Enter key
        /// </summary>
        [Parameter]
        public EventCallback<string> OnSearch { get; set; }

        protected override bool IgnoreOnChangeAndBlur => OnSearch.HasDelegate;

        protected override bool EnableOnPressEnter => OnSearch.HasDelegate || OnPressEnter.HasDelegate;

        private static readonly Dictionary<InputSize, ButtonSize> _buttonSizeMap = new()
        {
            [InputSize.Large] = ButtonSize.Large,
            [InputSize.Default] = ButtonSize.Default,
            [InputSize.Small] = ButtonSize.Small,
        };

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
                            builder.AddAttribute(3, "Type", IconType.Outline.Loading);
                        }
                        else
                        {
                            builder.AddAttribute(4, "Type", IconType.Outline.Search);
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
                        builder.AddAttribute(8, "Type", ButtonType.Default);
                        builder.AddAttribute(9, "Size", _buttonSizeMap[Size]);
                        builder.AddAttribute(10, "Loading", Loading);
                        builder.AddAttribute(11, "Disabled", this.Disabled);
                        if (!Loading)
                        {
                            builder.AddAttribute(12, "OnClick", CallbackFactory.Create<MouseEventArgs>(this, HandleSearch));
                        }
                        builder.AddAttribute(13, "Icon", IconType.Outline.Search);

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
                    builder.AddAttribute(13, "Type", ButtonType.Primary);
                    builder.AddAttribute(14, "Size", _buttonSizeMap[Size]);
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
                                b.AddAttribute(21, "Type", IconType.Outline.Search);
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
                    builder.AddAttribute(20, "Disabled", this.Disabled);
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

        protected override async Task OnPressEnterAsync(PressEnterEventArgs args)
        {
            await base.OnPressEnterAsync(args);
            await SearchAsync();
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
