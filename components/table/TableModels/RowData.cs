// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign.TableModels
{
    /// <inheritdoc />
    public class RowData<TItem> : RowData
    {
        public TableDataItem<TItem> DataItem { get; set; }

        public override TableDataItem TableDataItem => DataItem;

        public TItem Data => DataItem.Data;

        public Table<TItem> Table => DataItem.Table;

        /// <summary>
        /// hold the state of children rows
        /// </summary>
        public Dictionary<int, RowData<TItem>> Children { get; set; }

        public GroupResult<TItem> GroupResult { get; set; }

        public RowData()
        { }

        public RowData(TableDataItem<TItem> dataItem)
        {
            DataItem = dataItem;
        }

        protected override void CheckedChildren(bool isSelected, bool checkStrictly)
        {
            if (Children?.Any() != true)
                return;

            foreach (var item in Children)
            {
                item.Value.SetSelected(isSelected, checkStrictly);
            }
        }
    }

    /// <summary>
    /// Holds all data that is specific to a row, e.g. the row being expanded or not.
    /// See <see cref="TableDataItem"/> for all properties that are specific to an item instead of a row.
    /// </summary>
    public abstract class RowData
    {
        private bool _expanded;

        public int RowIndex { get; set; }

        public int PageIndex { get; set; }

        public bool IsGrouping { get; set; }

        public string Key { get; set; }

        internal ElementReference RowElementRef { get; set; }

        public bool Expanded
        {
            get => _expanded;
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    ExpandedChanged?.Invoke(this, _expanded);
                }
            }
        }

        public int Level { get; set; }

        public abstract TableDataItem TableDataItem { get; }

        public bool Selected
        {
            get => TableDataItem.Selected;
            set => TableDataItem.SetSelected(value);
        }

        internal event Action<RowData, bool> ExpandedChanged;

        internal void SetExpanded(bool expanded)
        {
            if (_expanded != expanded)
            {
                _expanded = expanded;
                ExpandedChanged?.Invoke(this, _expanded);
            }
        }

        protected abstract void CheckedChildren(bool isSelected, bool checkStrictly);

        internal void SetSelected(bool isSelected, bool checkStrictly)
        {
            TableDataItem.SetSelected(isSelected);

            if (!checkStrictly)
            {
                CheckedChildren(isSelected, checkStrictly);
            }
        }
    }

    /// <inheritdoc />
    public class TableDataItem<TItem> : TableDataItem
    {
        public TItem Data { get; set; }

        public Table<TItem> Table { get; set; }

        public IEnumerable<TItem> Children { get; set; }

        public override bool HasChildren => Children?.Any() ?? false;

        public TableDataItem()
        {
        }

        public TableDataItem(TItem data, Table<TItem> table)
        {
            this.Data = data;
            Table = table;
        }

        protected override void OnSelectedChanged(bool value)
        {
            Table.DataItemSelectedChanged(this, value);
        }
    }

    /// <summary>
    /// Holds the properties of an item within a table.
    /// Is unique for each item in a table (e.g. even if the item is displayed more than once,
    /// there will only be one <see cref="TableDataItem"/>).
    /// Therefore, all rows with the same item will be selected/deselected all at once.
    /// <br/>
    /// For row specific data, see <see cref="RowData"/>.
    /// </summary>
    public abstract class TableDataItem
    {
        private bool _selected;

        public bool Selected
        {
            get => _selected;
        }

        public bool Disabled { get; set; }

        public virtual bool HasChildren { get; }

        public event Action<TableDataItem, bool> SelectedChanged;

        protected abstract void OnSelectedChanged(bool value);

        internal void SetSelected(bool selected, bool triggersSelectedChanged = true)
        {
            if (_selected == selected)
            {
                return;
            }

            _selected = selected;

            if (triggersSelectedChanged)
            {
                OnSelectedChanged(_selected);
                SelectedChanged?.Invoke(this, _selected);
            }
        }
    }
}
