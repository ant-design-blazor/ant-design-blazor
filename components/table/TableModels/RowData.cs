using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    public class RowData<TItem> : RowData
    {
        public TItem Data { get; set; }

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

        public bool Expanded { get; set; }

        public int Level { get; set; }

        public int CacheKey { get; set; }

        public bool HasChildren { get; set; }

        public event Action<RowData, bool> SelectedChanged;

        internal void SetSelected(bool selected)
        {
            _selected = selected;
        }
    }
}
