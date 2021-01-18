---
order: 2
title:
  en-US: Data Index
  zh-CN: 数据索引
---

## zh-CN

可以不使用 `@bind-Field`，而是通过设置数据类型 `TData` 和数据索引字符串 `DataIndex` 来指定要绑定的属性，可以绑定后代属性。

当绑定的属性是值类型并且是Nullable时, `TData` 应设为Nullable类型。如: `<Column TData="int?" DataIndex="prop1" />` 或 `<Column TData="Nullable<int>" DataIndex="prop1" />`。

## en-US

Instead of using `@bind-Field`, you can specify the property to be bound by setting the data type `TData` and the data index string `DataIndex`, which can bind descendant properties.

When the bound property is ValueType and is Nullable, `TData` should be set to Nullable type. For example: `<Column TData="int?" DataIndex="prop1" />` or `<Column TData="Nullable<int>" DataIndex="prop1" />`.
