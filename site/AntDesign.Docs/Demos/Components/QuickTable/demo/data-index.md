---
order: 0.2
title:
  en-US: Data Index
  zh-CN: 数据索引
---

## zh-CN

可以不使用 `@bind-Field`，而是通过设置数据类型 `TData` 和数据索引字符串 `DataIndex` 来指定要绑定的属性。列数据的类型与 `TData` 的定义一致。

当绑定的属性是值类型并且是Nullable时，`TData` 应设为Nullable类型。如：`<Column TData="int?" DataIndex="prop1" />` 或 `<Column TData="Nullable<int>" DataIndex="prop1" />`。

Column使用DataIndex时，Table的OnChange参数 `QuerModel<TItem>.SortModel[].FieldName` 等于 `DataIndex`。

DataIndex支持的访问操作类型，详见[成员路径助手](/zh-CN/docs/member-path-helper)

## en-US

Instead of using `@bind-Field`, you can specify the property to be bound by setting the data type `TData` and the data index string `DataIndex`, which can bind descendant properties. The type of the column data is the same as the definition of `TData`.

When the bound property is ValueType and is Nullable, `TData` should be set to Nullable type. For example: `<Column TData="int?" DataIndex="prop1" />` or `<Column TData="Nullable<int>" DataIndex="prop1" />`.

When Column uses DataIndex, Table's OnChange event parameter `QuerModel<TItem>.SortModel[].FieldName` is equal to `DataIndex`.

See [Member path helper](/en-US/docs/member-path-helper) for details of the access modes supported by DataIndex.
