---
order: 6.55
title:
  zh-CN: 自定义筛选器
  en-US: Custom field filter
---

## zh-CN

当列类型不是内置筛选器支持的类型时，或者您想修改筛选器的比较操作，可以使用自定义筛选器 `FieldFilterType`。

在本示例中，Color 列就是用自定义筛选器，可以按"亮度"筛选 （或按HUE值排序）。

自定义筛选器需要实现 `IFieldFilterType`，或者继承 `BaseFieldFilterType` 或 `DateFieldFilterType` 来按需重写相关方法。

另外，还可以通过设置 `FieldFilterTypeResolver` 属性为整个表格组件配置自定义筛选器，或者可以通过在依赖注入服务中注册不同的 `IFilterTypeResolver` 实现来为整个应用程序中的表格组件设置自定义筛选器（`DefaultFieldFilterTypeResolver` 是默认实现。）。

## en-US

For types that are not supported by the built-in filters, or when you want different semantics like customizing the default operator, you can specify a custom `FieldFilterType` on the column.

In this example, the Color column can be filtered by the colors brightness (or sorted by its hue).

The custom Filter Types should implement `IFieldFilterType`, but may use the provided `BaseFieldFilterType` or `DateFieldFilterType` as a base implementation.

Your custom filter types can also be applied for a whole table, by setting its `FieldFilterTypeResolver` property, or globally by registering a different `IFilterTypeResolver` service. (See `DefaultFieldFilterTypeResolver` as a reference/fallback implementation.)
