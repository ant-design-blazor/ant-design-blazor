using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private Dictionary<TItem, RowData<TItem>> _dataSourceCache = new();

        private void FlushCache()
        {
            _dataSourceCache.Clear();
        }

        private void FinishLoadPage()
        {
            if (_selection == null)
                return;

            _selection?.StateHasChanged();
        }
    }
}
