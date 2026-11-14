---
order: 33
title:
  en-US: Quick table
  zh-CN: 快速表格
---

## zh-CN

`QuickTable` 启用快速渲染路径，通过复用列定义并直接渲染单元格片段，减少宽表格中的组件实例数量。

快速模式下，行相关的单元格模板应使用 `CellRender`，并通过 `cell.FieldValue` 或 `cell.RowData` 读取当前行。直接放在列 `ChildContent` 中、依赖外层 `@context` 的内容不会针对每一行重新执行。分组行目前不支持 `GroupTitleTemplate`；需要分组时请使用默认 `Quick=false` 的 `Table`。

## en-US

`QuickTable` enables the quick rendering path. It reuses column definitions and renders cell fragments directly to reduce component instances in wide tables.

Use `CellRender` for row-specific cell templates and read the current row through `cell.FieldValue` or `cell.RowData`. Content placed directly in a column's `ChildContent` that depends on the outer `@context` is not re-executed for each row. Grouped rows with `GroupTitleTemplate` are not currently supported; use `Table` with its default `Quick=false` when grouping is needed.
