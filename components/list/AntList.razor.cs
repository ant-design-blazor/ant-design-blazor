// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>Simple List.</para>

    <h2>When To Use</h2>

    <para>A list can be used to display content related to a single subject. The content can consist of multiple elements of varying type and size.</para>
    </summary>
    <seealso cref="ListItem"/>
    <seealso cref="ListItemMeta"/>
    <seealso cref="PaginationOptions"/>
    <seealso cref="ListGridType"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/5FrZKStG_/List.svg", Columns = 1, Title = "List", SubTitle = "列表")]
    public partial class AntList<TItem> : AntDomComponentBase, IAntList
    {
        internal string PrefixName { get; set; } = "ant-list";

        /// <summary>
        /// List of items to show in list
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> DataSource { get; set; }

        /// <summary>
        /// Put a border on the list
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Bordered { get; set; } = false;

        /// <summary>
        /// Header content for the list
        /// </summary>
        [Parameter]
        public RenderFragment Header { get; set; }

        /// <summary>
        /// Footer content for the list
        /// </summary>
        [Parameter]
        public RenderFragment Footer { get; set; }

        /// <summary>
        /// Content for the end of list items for diplaying a load more
        /// </summary>
        [Parameter]
        public RenderFragment LoadMore { get; set; }

        /// <summary>
        /// The layout of list, default is horizontal, If a vertical list is desired, set the itemLayout property to vertical
        /// </summary>
        [Parameter]
        public ListItemLayout ItemLayout { get; set; }

        /// <summary>
        /// Show loading on the list
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Loading { get; set; } = false;

        /// <summary>
        /// Currently unused
        /// </summary>
        [Parameter]
        public string NoResult { get; set; }

        /// <summary>
        /// Size of the list
        /// </summary>
        /// <default value="ListSize.Default" />
        [Parameter]
        public ListSize Size { get; set; } = ListSize.Default;

        /// <summary>
        /// Toggles rendering of the split under the list item
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool Split { get; set; } = true;

        /// <summary>
        /// The grid type of list
        /// </summary>
        [Parameter]
        public ListGridType Grid { get; set; }

        /// <summary>
        /// Options for paginating the list
        /// </summary>
        [Parameter]
        public PaginationOptions Pagination { get; set; }

        /// <summary>
        /// Content for the list
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        private bool IsSomethingAfterLastItem
        {
            get
            {
                return LoadMore != null || Footer != null || Pagination != null;
            }
        }

        private string SizeCls => Size switch
        {
            ListSize.Large => "lg",
            ListSize.Small => "sm",
            _ => string.Empty
        };

        ListGridType IAntList.Grid => Grid;
        ListItemLayout IAntList.ItemLayout => ItemLayout;
        double IAntList.ColumnWidth => _columnWidth;

        private double _columnWidth;
        private int _columns = 0;
        private bool _isInitialized = false;
        private int _currentPage = 1;
        private int _pageSize = 10;

        private List<TItem> GetItems()
        {
            var items = DataSource?.ToList() ?? new List<TItem>();
            if (Pagination == null)
            {
                return items;
            }

            var pageSize = GetPageSize();
            var currentPage = GetCurrentPage(items.Count, pageSize);
            return items.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        private int GetCurrentPage(int total, int pageSize)
        {
            var current = Pagination is { Current: > 0 } pagination ? pagination.Current : _currentPage;
            var pageCount = Math.Max(1, (int)Math.Ceiling((double)total / Math.Max(pageSize, 1)));
            return Math.Clamp(current, 1, pageCount);
        }

        private int GetPageSize() => Pagination is { PageSize: > 0 } pagination ? pagination.PageSize : _pageSize;

        private bool ShowPaginationAtTop => Pagination != null && (Pagination.Position is "top" or "both");
        private bool ShowPaginationAtBottom => Pagination != null && LoadMore == null && (Pagination.Position is "bottom" or "both");

        private async Task OnPaginationChanged(PaginationEventArgs args)
        {
            if (Pagination == null) return;
            if (Pagination.Current <= 0) _currentPage = args.Page;
            if (Pagination.PageSize <= 0) _pageSize = args.PageSize;
            await Pagination.OnChange.InvokeAsync(args);
        }

        protected override void OnInitialized()
        {
            SetClassMap();
            _columns = Grid?.Column ?? 0;
            if (_columns > 0)
            {
                _columnWidth = 100d / Grid.Column;
            }
            base.OnInitialized();
            _isInitialized = true;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (_isInitialized && _columns != (Grid?.Column ?? 0))
            {
                _columnWidth = 100d / Grid.Column;
            }
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(PrefixName)
                .If($"{PrefixName}-split", () => Split)
                .If($"{PrefixName}-rtl", () => RTL)
                .If($"{PrefixName}-bordered", () => Bordered)
                .GetIf(() => $"{PrefixName}-{SizeCls}", () => !string.IsNullOrEmpty(SizeCls))
                .If($"{PrefixName}-vertical", () => ItemLayout == ListItemLayout.Vertical)
                .If($"{PrefixName}-loading", () => (Loading))
                .If($"{PrefixName}-grid", () => Grid != null)
                .If($"{PrefixName}-something-after-last-item", () => IsSomethingAfterLastItem);
        }

        private void OnBreakpoint(BreakpointType breakPoint)
        {
            var column = breakPoint switch
            {
                BreakpointType.Xs => Grid.Xs,
                BreakpointType.Sm => Grid.Sm,
                BreakpointType.Md => Grid.Md,
                BreakpointType.Lg => Grid.Lg,
                BreakpointType.Xl => Grid.Xl,
                BreakpointType.Xxl => Grid.Xxl,
                _ => 4,
            };

            if (column > 0)
            {
                _columnWidth = 100d / column;
            }
        }
    }
}
