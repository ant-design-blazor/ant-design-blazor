using System.Collections.Generic;
using AntDesign.TableModels;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private Dictionary<TItem, RowData<TItem>> _dataSourceCache = new();
        private Dictionary<TItem, List<RowData<TItem>>> _allRowDataCache = new();

        private void FlushCache()
        {
            _dataSourceCache.Clear();
            _allRowDataCache.Clear();
        }

        private void FinishLoadPage()
        {
            if (_selection == null)
                return;

            _selection?.StateHasChanged();
        }
    }
}
