// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class ActionColumn : ColumnBase
    {
        /// <summary>Whether to display the sortable header affordance.</summary>
        [Parameter]
        public bool Sortable { get; set; }

        private SortDirection _sortDirection;

        [CascadingParameter(Name = "AntDesign.Column.Blocked")]
        internal bool Blocked { get; set; }

        /// <summary>
        /// Column content for a row. Takes priority over <see cref="ColumnBase.ChildContent"/>
        /// </summary>
        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (IsHeader)
            {
                ClassMapper.If("ant-table-column-has-sorters", () => Sortable);
                Context.HeaderColumnInitialized(this);
            }
        }

        private SortDirection NextSortDirection()
        {
            var directions = Table.SortDirections;
            if (_sortDirection == SortDirection.None)
                return directions[0];

            var index = Array.IndexOf(directions, _sortDirection);
            return index < 0 || index >= directions.Length - 1 ? SortDirection.None : directions[index + 1];
        }

        private void HandleSort()
        {
            if (Sortable)
            {
                _sortDirection = NextSortDirection();
                StateHasChanged();
            }
        }

        private void HandleSortKeyDown(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
                HandleSort();
        }

        private string AriaSort => _sortDirection switch
        {
            SortDirection.Ascending => "ascending",
            SortDirection.Descending => "descending",
            _ => null
        };

        private RenderFragment SorterIcon() => builder =>
        {
            var sequence = 0;
            builder.OpenElement(sequence++, "span");
            builder.AddAttribute(sequence++, "class", "ant-table-column-sorter ant-table-column-sorter-full");
            builder.OpenElement(sequence++, "span");
            builder.AddAttribute(sequence++, "class", "ant-table-column-sorter-inner");
            builder.AddAttribute(sequence++, "aria-hidden", "true");
            builder.OpenComponent<Icon>(sequence++);
            builder.AddAttribute(sequence++, "Type", "caret-up");
            builder.AddAttribute(sequence++, "Class", $"ant-table-column-sorter-up{(_sortDirection == SortDirection.Ascending ? " active" : "")}");
            builder.CloseComponent();
            builder.OpenComponent<Icon>(sequence++);
            builder.AddAttribute(sequence++, "Type", "caret-down");
            builder.AddAttribute(sequence++, "Class", $"ant-table-column-sorter-down{(_sortDirection == SortDirection.Descending ? " active" : "")}");
            builder.CloseComponent();
            builder.CloseElement();
            builder.CloseElement();
        };

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
        }
    }
}
