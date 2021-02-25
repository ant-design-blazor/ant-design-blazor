---
category: Components
cols: 1
type: 数据展示
title: Table
subtitle: 表格
cover: https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg
---

展示行列数据。

## 何时使用

- 当有大量结构化的数据需要展现时；
- 当需要对数据进行排序、搜索、分页、自定义操作等复杂行为时。

## 如何使用

指定表格的数据源 `dataSource` 为一个数组。

#### onRow 用法

适用于 `onRow` `onHeaderRow` `onCell` `onHeaderCell`。

```jsx
<Table OnRow="OnRow" />

@code {
    Dictionary<string, object> OnRow(RowData<Data> row)
        {
            Action<MouseEventArgs> OnClick = (args) =>
            {
                Console.WriteLine($"row {row.Data.Key} was clicked");
            };

            return new Dictionary<string, object>()
            {
                { "onclick", OnClick },
                { "id", row.Data.Key },
            };
        }
    }
```
