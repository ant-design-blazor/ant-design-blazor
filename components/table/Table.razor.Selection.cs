using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private ISelectionColumn _selection;

        ISelectionColumn ITable.Selection
        {
            get => _selection;
            set => _selection = value;
        }

        void ITable.SelectionChanged(int[] checkedIndex)
        {
            if (SelectedRowsChanged.HasDelegate)
            {
                var list = new List<TItem>();
                foreach (var index in checkedIndex)
                {
                    list.Add(_dataSource.ElementAt(index));
                }

                SelectedRowsChanged.InvokeAsync(list);
            }
        }

        private void ChangeSelection(int[] indexes)
        {
            if (indexes == null || !indexes.Any())
            {
                this._selection.RowSelections.ForEach(x => x.Check(false));
                this._selection.Check(false);
            }
            else
            {
                this._selection.RowSelections.Where(x => !x.RowIndex.IsIn(indexes)).ForEach(x => x.Check(false));
                this._selection.RowSelections.Where(x => x.RowIndex.IsIn(indexes)).ForEach(x => x.Check(true));
                this._selection.Check(true);
            }
        }

        public void SetSelection(string[] keys)
        {
            if (keys == null || !keys.Any())
            {
                this._selection.RowSelections.ForEach(x => x.Check(false));
                this._selection.Check(false);
            }
            else
            {
                this._selection.RowSelections.Where(x => !x.Key.IsIn(keys)).ForEach(x => x.Check(false));
                this._selection.RowSelections.Where(x => x.Key.IsIn(keys)).ForEach(x => x.Check(true));
                this._selection.Check(keys.Any());
            }
        }
    }
}
