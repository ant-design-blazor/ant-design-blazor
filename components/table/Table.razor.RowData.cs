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
            _dataSourceCache ??= new Dictionary<int, RowData<TItem>>();
            _dataSourceCache.Clear();
        }

        int[] ITable.GetSelectedIndex()
        {
            return _dataSourceCache.Where(x => x.Value.Selected).Select(x => x.Key).ToArray();
        }

        private void FinishLoadPage()
        {
            _selection?.ChangeOnPaging();
        }
    }
}
