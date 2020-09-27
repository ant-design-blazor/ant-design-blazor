---
order: 15
title:
  en-US: ColSpan and RowSpan
  zh-CN: 表格行/列合并
---

## zh-CN

表头只支持列合并，使用 `Column` 里的 `HeaderColSpan` 进行设置，设值为 0 时，设置的表格不会渲染。

表格支持行/列合并，使用 `Column` 里的单元格属性 `ColSpan` 或者 `RowSpan` 设值为 0 时，设置的表格不会渲染。

## en-US

Table column title supports `HeaderColSpan` that set in column. When each of them is set to 0, the cell will not be rendered.

Table cell supports `ColSpan` and `RowSpan` that set in `Column` return object. When each of them is set to 0, the cell will not be rendered.