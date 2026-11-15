---
order: 33
title:
  en-US: Quick table
  zh-CN: 快速表格
---

## zh-CN

`QuickTable` 启用快速渲染路径，通过复用列定义并直接渲染单元格片段，减少宽表格中的组件实例数量。

快速模式同时支持 `CellRender` 和列 `ChildContent`。`CellRender` 会通过 `cell.FieldValue` 或 `cell.RowData` 读取当前行，并保留快速渲染性能。为兼容原有写法，设置了普通列 `ChildContent` 时，表格会按行执行列模板，让其中的 `@context` 对应当前行；即使内容本身不依赖行数据，也会走这条兼容路径，快速渲染的性能收益会降低。触发兼容路径时，表格会记录一次 warning 日志并建议改用 `CellRender`。建议在关注性能时使用 `CellRender`。

快速渲染支持行分组、树形数据、展开行、选择列，以及列的 `CellRender`、`ChildContent` 和 `TitleTemplate`。自定义列组件可以沿用 `Column<T>` 或 `ActionColumn` 的快速单元格渲染。设置 `RowTemplate` 时，表格会保留该模板的整行渲染行为；模板内部由调用方负责渲染，无法由表格替换成快速单元格路径。`Table` 的 `Quick` 默认是 `true`；设置 `Quick="false"` 可强制使用标准渲染路径。

启用 `Quick` 后，列配置在初始化时注册一次，因此表格运行期间应保持列结构不变；使用普通 `ChildContent` 时，每行仍会执行列模板以维持兼容。需要应用动态列结构时，请重新创建表格。

## en-US

`QuickTable` enables the quick rendering path. It reuses column definitions and renders cell fragments directly to reduce component instances in wide tables.

Quick mode supports both `CellRender` and column `ChildContent`. `CellRender` reads the current row through `cell.FieldValue` or `cell.RowData` and retains the quick-rendering performance. For compatibility, tables with regular column `ChildContent` re-execute the column template for each row, so `@context` refers to that row. This compatibility path is used even when the content does not depend on row data, and it reduces the performance benefit. The table logs one warning per instance recommending `CellRender` when this path is used. Prefer `CellRender` when performance matters.

Quick rendering supports row grouping, tree data, expanded rows, selection columns, `TitleTemplate`, and custom column components that use `Column<T>` or `ActionColumn`. When `RowTemplate` is set, the table preserves its whole-row rendering behavior; the template owns its rendering, so the table cannot replace its contents with quick cells. `Table.Quick` defaults to `true`; set `Quick="false"` to force the standard rendering path.

With `Quick` enabled, column configuration is registered once, so keep the column structure stable while the table is mounted. Regular column `ChildContent` is still executed per row for compatibility. Recreate the table to apply a dynamic column structure.
