---
order: 6.55
title:
  zh-CN: 
  en-US: Custom field filter
---

## zh-CN


## en-US

For types that are not supported by the built-in filters, or when you want different semantics like customizing the default operator, you can specify a custom `FieldFilterType` on the column.

Your custom filter types can also be applied for a whole table, by setting its `FieldFilterTypeResolver` property, or globally by registering a different `IFilterTypeResolver` service. (See `DefaultFieldFilterTypeResolver` as a reference/fallback implementation.)

The custom Filter Types should implement `IFieldFilterType`, but may use the provided `BaseFieldFilterType` or `DateFieldFilterType` as a base implementation.