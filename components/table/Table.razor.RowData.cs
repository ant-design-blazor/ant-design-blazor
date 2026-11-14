// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private readonly Dictionary<int, TableDataItem<TItem>> _dataSourceCache;
        private readonly Dictionary<int, RowData<TItem>> _rootRowDataCache;
        private readonly HashSet<TItem> _showItemHashs;

        private void FlushCache()
        {
            if (!_hasInitialized || !_dataSourceCache.Any())
            {
                return;
            }

            // Clears the cache of rowdata that is not in the current page.
            var showItemKeys = GetShowItemsIncludeChildren(_showItems).Select(GetHashCode);
            _showItemHashs.Clear();

            var removedKeys = _dataSourceCache.Keys.Except(showItemKeys);
            foreach (var key in removedKeys)
            {
                _rootRowDataCache.Remove(key);
                _dataSourceCache.Remove(key);
            }
        }

        private void FinishLoadPage()
        {
            if (_selection == null)
                return;

            _selection?.StateHasChanged();
        }

        public void ExpandAll()
        {
            _preventRender = true;

            foreach (var item in _rootRowDataCache)
            {
                item.Value.SetExpanded(true);
            }
        }

        public void CollapseAll()
        {
            _preventRender = true;

            foreach (var item in _rootRowDataCache)
            {
                item.Value.SetExpanded(false);
            }
        }

        private RowData<TItem> GetGroupRowData(GroupResult<TItem> grouping, int index, int level, Dictionary<int, RowData<TItem>> rowCache = null)
        {
            int rowIndex = index + 1;

            if (level == 0)
            {
                rowIndex += PageSize * (PageIndex - 1);
            }

            var hashCode = grouping.Key.GetHashCode() ^ rowIndex;
            rowCache ??= _rootRowDataCache;

            if (!rowCache.TryGetValue(hashCode, out var groupRowData) || groupRowData == null)
            {
                groupRowData = new RowData<TItem>()
                {
                    Key = grouping.Key.ToString(),
                    IsGrouping = true,
                    RowIndex = rowIndex,
                    Level = level,
                    GroupResult = grouping,
                    DataItem = new TableDataItem<TItem>
                    {
                        Table = this,
                    },
                };

                rowCache.Add(hashCode, groupRowData);
            }

            groupRowData.Children = grouping.Children.SelectMany(x =>
                    x.Key == null
                        ? x.Items.Select((data, index) => GetRowData(data, index + rowIndex, level + 1, rowCache))
                        : [GetGroupRowData(x, index + rowIndex, level + 1, rowCache)])
                .ToDictionary(x => x.Data != null ? GetHashCode(x.Data) : x.GroupResult.GetHashCode(), x => x);

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
            currentDataItem.Children = TreeChildren?.Invoke(data);
            // this row cache may be for children rows
            rowCache ??= _rootRowDataCache;

            if (!rowCache.TryGetValue(dataHashCode, out var currentRowData) || currentRowData == null)
            {
                currentRowData = new RowData<TItem>(currentDataItem)
                {
                    Key = dataHashCode.ToString(),
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

        // TODO: need to cache the children in showItems directly
        private IEnumerable<TItem> GetShowItemsIncludeChildren(IEnumerable<TItem> showItems)
        {
            foreach (var item in showItems)
            {
                if (_showItemHashs.Contains(item))
                    continue;

                _showItemHashs.Add(item);

                yield return item;

                var children = TreeChildren?.Invoke(item);

                if (children?.Any() == true)
                {
                    foreach (var child in GetShowItemsIncludeChildren(children))
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}
