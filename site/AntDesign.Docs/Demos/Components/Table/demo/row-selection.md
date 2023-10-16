---
order: 2
title:
  en-US: Selection
  zh-CN: 行选择
---

## zh-CN

第一列是联动的选择框。可以通过 `RowSelection.Type` 属性指定选择类型，默认为 `checkbox`。

> 可为 `SelectedRows` 设置默认值来默认选中，默认是用对象引用来跟列数据做比对的，所以 **要从 DataSource 中** 选取对象。也可以利用 Table 上的 `RowKey` 属性来自定义比对值。

## en-US

Rows can be selectable by making first column as a selectable column. You can use `rowSelection.type` to set selection type. Default is `checkbox`.

> Default values can be set for `SelectedRows` to set the default selection. The default is to compare the column data with the object reference, so **select the object from the DataSource**. You can also customize the compare key using the `RowKey` parameter on the Table.
