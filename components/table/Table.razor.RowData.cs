using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private Dictionary<int, TableDataItem<TItem>> _dataSourceCache;
        private Dictionary<int, RowData<TItem>> _rootRowDataCache;

        private void FlushCache()
        {
            _dataSourceCache.Clear();
            _rootRowDataCache.Clear();
        }

        private void FinishLoadPage()
        {
            if (_selection == null)
                return;

            _selection?.StateHasChanged();
        }
    }
}
