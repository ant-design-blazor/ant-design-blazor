---
order: 0
title:
  zh-CN: Default - Basic Usage
  en-US: Default - Basic Usage
---

## zh-CN

The select-module supports a `two-way` binding. When the selection changes, the value for `@bind-value` is updated. If `@bind-value` is not specified, the parameters `TItemValue` and `TItem` must be set. The `IEnumerable<T>` DataSource should be used by default. In some use cases, a `IEnumerable<T>` DataSource produces a considerable source code overhead. For this reason, you can create SelectOptions directly, but the functions are limited (`TItem` must be `string`).

## en-US

The select-module supports a `two-way` binding. When the selection changes, the value for `@bind-value` is updated. If `@bind-value` is not specified, the parameters `TItemValue` and `TItem` must be set. The `IEnumerable<T>` DataSource should be used by default. In some use cases, a `IEnumerable<T>` DataSource produces a considerable source code overhead. For this reason, you can create SelectOptions directly, but the functions are limited (`TItem` must be `string`).