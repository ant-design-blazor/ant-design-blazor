using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private ISelectionColumn _headerSelection;

        ISelectionColumn ITable.HeaderSelection
        {
            get => _headerSelection;
            set => _headerSelection = value;
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
                this._headerSelection.RowSelections.ForEach(x => x.Check(false));
                this._headerSelection.Check(false);
            }
            else
            {
                this._headerSelection.RowSelections.Where(x => !x.RowIndex.IsIn(indexes)).ForEach(x => x.Check(false));
                this._headerSelection.RowSelections.Where(x => x.RowIndex.IsIn(indexes)).ForEach(x => x.Check(true));
                this._headerSelection.Check(true);
            }
        }

        public void SetSelection(string[] keys)
        {
            if (keys == null || !keys.Any())
            {
                this._headerSelection.RowSelections.ForEach(x => x.Check(false));
                this._headerSelection.Check(false);
            }
            else
            {
                this._headerSelection.RowSelections.Where(x => !x.Key.IsIn(keys)).ForEach(x => x.Check(false));
                this._headerSelection.RowSelections.Where(x => x.Key.IsIn(keys)).ForEach(x => x.Check(true));
                this._headerSelection.Check(keys.Any());
            }
        }
    }
}
