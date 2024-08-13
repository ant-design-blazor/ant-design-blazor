// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class PaginationOptions
    {
        internal static readonly int[] DefaultPageSizeOptions = { 10, 20, 50, 100 };

        /// <summary>
        /// If pagination is small or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool IsSmall { get; set; }

        /// <summary>
        /// Whether the pagination is disabled or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string RootPrefixCls { get; set; }

        /// <summary>
        /// Callback executed when the page size changes
        /// </summary>
        [Parameter]
        public EventCallback<int> ChangeSize { get; set; }

        /// <summary>
        /// Current page
        /// </summary>
        [Parameter]
        public int Current { get; set; }

        /// <summary>
        /// Current pag size
        /// </summary>
        [Parameter]
        public int PageSize { get; set; }

        /// <summary>
        /// Options for page size selection
        /// </summary>
        /// <default value="{ 10, 20, 50, 100 }"/>
        [Parameter]
        public int[] PageSizeOptions { get; set; }

        /// <summary>
        /// Callback executed when jumping to a specific page
        /// </summary>
        [Parameter]
        public EventCallback<int> QuickGo { get; set; }

        /// <summary>
        /// Quick jumper confirm button, this is for react version <c>ShowQuickJumper: { goButton: ReactNode }</c>
        /// </summary>
        [Parameter]
        public OneOf<bool, RenderFragment>? GoButton { get; set; }

        private string _goInputText = string.Empty;

        /// <summary>
        /// Locale used for localization of the component
        /// </summary>
        /// <default value="LocaleProvider.CurrentLocale.Pagination" />
        [Parameter]
        public PaginationLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Pagination;

        private int GetValidValue()
        {
            return string.IsNullOrWhiteSpace(_goInputText) ? 0 :
                   int.TryParse(_goInputText, out var value) ? value : 0;
        }

        private string BuildOptionText(int value) => $"{value} {Locale.ItemsPerPage}";

        private async Task ChangePaginationSize(int value)
        {
            if (PageSize == value)
            {
                return;
            }

            PageSize = value;

            if (ChangeSize.HasDelegate)
            {
                await ChangeSize.InvokeAsync(value);
            }
        }

        private void HandleChange(ChangeEventArgs e)
        {
            _goInputText = e.Value as string;
        }

        private void HandleBlur(FocusEventArgs e)
        {
            if (GoButton != null || string.IsNullOrEmpty(_goInputText))
            {
                return;
            }

            _goInputText = string.Empty;

            // relatedTarget not implemented
            // if (e.RelatedTarget && (e.relatedTarget.className.indexOf($"{rootPrefixCls}-item-link") >= 0 || e.relatedTarget.className.indexOf($"{rootPrefixCls}-item") >= 0))
            // {
            //     return;
            // }

            if (QuickGo.HasDelegate)
            {
                QuickGo.InvokeAsync(this.GetValidValue());
            }
        }

        private void Go(EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_goInputText))
            {
                return;
            }

            if (e is KeyboardEventArgs { Code: "Enter" } || e is MouseEventArgs { Type: "Click" })
            {
                if (QuickGo.HasDelegate)
                {
                    QuickGo.InvokeAsync(this.GetValidValue());
                }

                _goInputText = string.Empty;
            }
        }

        private int[] GetPageSizeOptions()
        {
            PageSizeOptions ??= DefaultPageSizeOptions;
            if (PageSizeOptions.Any(option => option == PageSize))
            {
                return PageSizeOptions;
            }

            return PageSizeOptions.Concat(new[] { PageSize }).OrderBy(e => e).ToArray();
        }
    }
}
