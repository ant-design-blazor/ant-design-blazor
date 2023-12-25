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

        private RowData<TItem> GetGroupRowData(IGrouping<object, TItem> grouping, int index, int level, Dictionary<int, RowData<TItem>> rowCache = null)
        {
            int rowIndex = index + 1;

            if (level == 0)
            {
                rowIndex += PageSize * (PageIndex - 1);
            }

            var groupRowData = new RowData<TItem>()
            {
                Key = grouping.Key.ToString(),
                IsGrouping = true,
                RowIndex = rowIndex,
                DataItem = new TableDataItem<TItem>
                {
                    HasChildren = true,
                    Table = this,
                    Children = grouping
                },
                Children = grouping.Select((data, index) => GetRowData(data, index, level, rowCache)).ToDictionary(x => GetHashCode(x.Data), x => x)
            };

            return groupRowData;
        }

        private RowData<TItem> GetRowData(TItem data, int index, int level, Dictionary<int, RowData<TItem>> rowCache = null)
        {
            int rowIndex = index + 1;

            if (level == 0)
            {
                rowIndex += PageSize * (PageIndex - 1);
            }

            var dataHashCode = GetHashCode(data);

            if (!_dataSourceCache.TryGetValue(dataHashCode, out var currentDataItem) || currentDataItem == null)
            {
                currentDataItem = new TableDataItem<TItem>(data, this);
                currentDataItem.SetSelected(SelectedRows.Contains(data), triggersSelectedChanged: false);
                _dataSourceCache.Add(dataHashCode, currentDataItem);
            }

            currentDataItem.Data = data;

            // this row cache may be for children rows
            rowCache ??= _rootRowDataCache;

            if (!rowCache.TryGetValue(dataHashCode, out var currentRowData) || currentRowData == null)
            {
                currentRowData = new RowData<TItem>(currentDataItem)
                {
                    Expanded = DefaultExpandAllRows && level < DefaultExpandMaxLevel
                };
                rowCache.Add(dataHashCode, currentRowData);
            }

            currentRowData.Level = level;
            currentRowData.RowIndex = rowIndex;
            currentRowData.PageIndex = PageIndex;

            if (currentDataItem.HasChildren && (level < DefaultExpandMaxLevel || currentRowData.Expanded))
            {
                foreach (var (item, i) in currentDataItem.Children.Select((item, index) => (item, index)))
                {
                    currentRowData.Children ??= [];
                    if (currentRowData.Children.ContainsKey(GetHashCode(item)))
                        continue;

                    GetRowData(item, i, level + 1, currentRowData.Children);
                }
            }

            return currentRowData;
        }
    }
}
