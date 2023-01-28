using System;
using System.Collections.Generic;

namespace AntDesign.TableModels
{
    public class RowData<TItem> : RowData
    {
        public TItem Data { get; set; }

        internal Dictionary<TItem, RowData<TItem>> Children { get; set; } = new();

        public RowData(int rowIndex, int pageIndex, TItem data)
        {
            this.RowIndex = rowIndex;
            this.PageIndex = pageIndex;
            this.Data = data;
        }
    }

    public class RowData
    {
        private bool _selected;

        private bool _expanded;

        public int RowIndex { get; set; }

        public int PageIndex { get; set; }

        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    SelectedChanged?.Invoke(this, _selected);
                }
            }
        }

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

        public int CacheKey { get; set; }

        public bool HasChildren { get; set; }

        public event Action<RowData, bool> SelectedChanged;

        public event Action<RowData, bool> ExpandedChanged;

        internal void SetSelected(bool selected)
        {
            _selected = selected;
        }

        internal void SetExpanded(bool expanded)
        {
            _expanded = expanded;
        }
    }
}
