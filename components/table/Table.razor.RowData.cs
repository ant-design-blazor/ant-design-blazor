using System;
using AntDesign.TableModels;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private IDictionary<int, RowData<TItem>> _dataSourceCache;

        private void FlushCache()
        {
            if (_dataSourceCache == null)
            {
                _dataSourceCache = new Dictionary<int, RowData<TItem>>();
            }
            else
            {
                _dataSourceCache.Clear();
            }
        }

        int[] ITable.GetSelectedCacheKeys()
        {
            return _dataSourceCache.Where(x => x.Value.Selected).Select(x => x.Key).ToArray();
        }

        private void FinishLoadPage()
        {
            if (_selection == null)
                return;

            // Clear cached items that are not on current page
            var currentPageCacheKeys = _selection.RowSelections.Select(x => x.CacheKey);
            var deletedCaches =
                _dataSourceCache.Where(x => x.Value.PageIndex == PageIndex && !x.Key.IsIn(currentPageCacheKeys));
            var needInvokeChange = deletedCaches.Any(x => x.Value.Selected);
            deletedCaches.ForEach(x => _dataSourceCache.Remove(x));

            _selection?.ChangeOnPaging();

            if (needInvokeChange)
            {
                _selection.InvokeSelectedRowsChange();
            }
        }
    }
}
