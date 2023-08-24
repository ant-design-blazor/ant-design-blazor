---
order: 6
title:
  en-US: Filter and sorter
  zh-CN: 筛选和排序
---

## zh-CN

对某一列数据进行筛选，使用列的 `Filters` 属性来指定需要筛选菜单的列，`OnFilter` 用于筛选当前数据，`FilterMultiple` 用于指定多选和单选。

对某一列数据进行排序，通过指定列的 `Sortable` 属性即可启动排序按钮，也可以设置 `SorterCompare: (rowA, rowB) => int`， rowA、rowB 为比较的两个行数据。

`SortDirections: ['ascend' | 'descend']`改变每列可用的排序方式，切换排序时按数组内容依次切换，设置在 table props 上时对所有列生效。你可以通过设置 `['ascend', 'descend', 'ascend']` 禁止排序恢复到默认状态。

使用 `DefaultSortOrder` 属性，设置列的默认排序顺序。

还可以在 `OnChange` 事件绑定的方法中，自定义筛选逻辑，可用方法 `QueryModel.ExecuteQuery(data)` 构建查询表达式。

## en-US

Use `Filters` to generate filter menu in columns, `OnFilter` to determine filtered result, and `FilterMultiple` to indicate whether it's multiple or single selection.

Uses `defaultFilteredValue` to make a column filtered by default.

Use `Sortable` to make a column sortable. Or use `SorterCompare`, a function of the type `(a, b) => int` for sorting data locally.

`SortDirections: ['ascend' | 'descend']` defines available sort methods for each columns, effective for all columns when set on table props. You can set as `['ascend', 'descend', 'ascend']` to prevent sorter back to default status.

Uses `DefaultSortOrder` to make a column sorted by default.

If a `SortOrder` or `DefaultSortOrder` is specified with the value `ascend` or `descend`, you can access this value from within the function passed to the `sorter` as explained above. Such a function can take the form: `function(a, b, sortOrder) { ... }`.

You can also customize the filtering logic in the 'OnChange' event method, using the method 'QueryModel.ExecuteQuery(data)' to build query expressions.