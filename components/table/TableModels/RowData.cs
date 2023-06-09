using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    /// <inheritdoc />
    public class RowData<TItem> : RowData
    {
        internal TableDataItem<TItem> DataItem { get; }
        public override TableDataItem TableDataItem => DataItem;

        public TItem Data => DataItem.Data;
        public Table<TItem> Table => DataItem.Table;

        internal Dictionary<TItem, RowData<TItem>> Children { get; set; }

        public RowData(TableDataItem<TItem> dataItem)
        {
            DataItem = dataItem;
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

        public bool Selected { get => TableDataItem.Selected; set => TableDataItem.Selected = value; }
        public bool HasChildren { get => TableDataItem.HasChildren; set => TableDataItem.HasChildren = value; }

        public event Action<RowData, bool> ExpandedChanged;

        internal void SetExpanded(bool expanded)
        {
            _expanded = expanded;
        }
    }

    /// <inheritdoc />
    public class TableDataItem<TItem> : TableDataItem
    {
        public TItem Data { get; }

        public Table<TItem> Table { get; }

        public TableDataItem(TItem data, Table<TItem> table)
        {
            this.Data = data;
            Table = table;
        }

        protected override void OnSelectedChanged(bool value)
        {
            base.OnSelectedChanged(value);
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
            set
            {
                if (_selected != value)
                {
                    OnSelectedChanged(value);
                }
            }
        }

        public bool HasChildren { get; set; }

        public event Action<TableDataItem, bool> SelectedChanged;

        protected virtual void OnSelectedChanged(bool value)
        {
            SetSelected(value);
        }

        internal void SetSelected(bool selected, bool triggersSelectedChanged = true)
        {
            _selected = selected;
            if (triggersSelectedChanged)
                SelectedChanged?.Invoke(this, _selected);
        }
    }
}
