using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private Dictionary<TItem, TableDataItem<TItem>> _dataSourceCache = new();
        private Dictionary<TItem, RowData<TItem>> _rootRowDataCache = new();

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

        public void ExpandAll()
        {
            foreach (var item in _rootRowDataCache)
            {
                item.Value.SetExpanded(true);
            }
        }

        public void CollapseAll()
        {
            foreach (var item in _rootRowDataCache)
            {
                item.Value.SetExpanded(false);
            }
        }
    }
}
