---
order: 22.1
title:
  en-US: ColSpan and RowSpan
  zh-CN: 表格行/列合并
---

## zh-CN

表头只支持列合并，使用 `Column` 里的 `HeaderColSpan` 进行设置，设值为 0 时，设置的表格不会渲染。

表格支持固定的 `ColSpan`、`RowSpan` 参数，也支持通过 `OnCell` 为不同行设置合并属性。某行的任一合并值为 0 时，该单元格不会渲染。设置 `RowScope="row"` 可将该列单元格标记为行标题。

## en-US

Table column title supports `HeaderColSpan` that set in column. When each of them is set to 0, the cell will not be rendered.

Table cells support fixed `ColSpan` and `RowSpan` parameters, or row-specific span attributes returned by `OnCell`. When either span is 0 for a row, that cell is not rendered. Set `RowScope="row"` on a column to render row header cells.
