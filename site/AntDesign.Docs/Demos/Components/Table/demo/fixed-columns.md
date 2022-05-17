---
order: 19
title:
  en-US: Fixed Columns
  zh-CN: 固定列
---

## zh-CN

对于列数很多的数据，可以固定前后的列，横向滚动查看其它数据，需要和 `ScrollX` 配合使用。

> 若列头与内容不对齐或出现列重复，请指定**固定列**的宽度 `Width`。如果指定 `Width` 不生效或出现白色垂直空隙，请尝试建议留一列不设宽度以适应弹性布局，或者检查是否有[超长连续字段破坏布局](https://github.com/ant-design/ant-design/issues/13825#issuecomment-449889241)。
>
> 建议指定 `ScrollX` 为大于表格宽度的固定值或百分比。注意，且非固定列宽度之和不要超过 `ScrollX`。

## en-US

To fix some columns and scroll inside other columns, and you must set `ScrollX` meanwhile.

> Specify the width of columns if header and cell do not align properly. If specified width is not working or have gutter between columns, please try to leave one column at least without width to fit fluid layout, or make sure no [long word to break table layout](https://github.com/ant-design/ant-design/issues/13825#issuecomment-449889241).
>
> A fixed value which is greater than table width for `ScrollX` is recommended. The sum of unfixed columns should not greater than `ScrollX`.
