---
order: 33
title:
  en-US: Quick table
  zh-CN: 快速表格
---

## zh-CN

`QuickTable` 启用快速渲染路径，通过复用列定义并直接渲染单元格片段，减少宽表格中的组件实例数量。

快速模式下，行相关的单元格模板应使用 `CellRender`，并通过 `cell.FieldValue` 或 `cell.RowData` 读取当前行。直接放在列 `ChildContent` 中、依赖外层 `@context` 的内容不会针对每一行重新执行。

快速渲染适用于普通字段列和操作列。使用行分组、树形数据、展开行、选择列、自定义列组件或 `RowTemplate` 时，表格会退回标准渲染路径，以保持原有行为；这些场景不会获得快速渲染的性能收益。`Table` 的 `Quick` 默认是 `false`，现有用法不需要调整。

## en-US

`QuickTable` enables the quick rendering path. It reuses column definitions and renders cell fragments directly to reduce component instances in wide tables.

Use `CellRender` for row-specific cell templates and read the current row through `cell.FieldValue` or `cell.RowData`. Content placed directly in a column's `ChildContent` that depends on the outer `@context` is not re-executed for each row.

Quick rendering applies to standard field and action columns. Tables with row grouping, tree data, expanded rows, a selection column, custom column components, or `RowTemplate` fall back to standard rendering to preserve existing behavior, so those scenarios receive no quick-rendering performance benefit. `Table.Quick` defaults to `false`; existing usage does not need to change.
