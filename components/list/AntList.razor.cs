// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ListGridType
    {
        public int Gutter { get; set; }
        public int Column { get; set; }
        public int Xs { get; set; }
        public int Sm { get; set; }
        public int Md { get; set; }
        public int Lg { get; set; }
        public int Xl { get; set; }
        public int Xxl { get; set; }
    }

    public partial class AntList<TItem> : AntDomComponentBase, IAntList
    {
        public string PrefixName { get; set; } = "ant-list";

        [Parameter] public IEnumerable<TItem> DataSource { get; set; }

        [Parameter] public bool Bordered { get; set; } = false;

        [Parameter] public RenderFragment Header { get; set; }

        [Parameter] public RenderFragment Footer { get; set; }

        [Parameter] public RenderFragment LoadMore { get; set; }

        [Parameter] public ListItemLayout ItemLayout { get; set; }

        [Parameter] public bool Loading { get; set; } = false;

        [Parameter] public string NoResult { get; set; }

        [Parameter] public string Size { get; set; } = AntSizeLDSType.Default;

        [Parameter] public bool Split { get; set; } = true;

        [Parameter] public ListGridType Grid { get; set; }

        [Parameter] public PaginationOptions Pagination { get; set; }

        [Parameter] public RenderFragment<TItem> ChildContent { get; set; }

        private bool IsSomethingAfterLastItem
        {
            get
            {
                return LoadMore != null || Footer != null || Pagination != null;
            }
        }

        private string SizeCls => Size switch
        {
            "large" => "lg",
            "small" => "sm",
            _ => string.Empty
        };

        ListGridType IAntList.Grid => Grid;
        ListItemLayout IAntList.ItemLayout => ItemLayout;
        double IAntList.ColumnWidth => _columnWidth;

        private double _columnWidth;
        private int _columns = 0;
        private bool _isInitialized = false;

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
