---
order: 17
title:
  en-US: Tree Data
  zh-CN: 树形数据展示
---

## zh-CN

表格支持树形数据的展示，只需设置 `TreeChildren` 属性指定子数据。

设置 `OnExpand` 后强制显示展开按钮，按需加载子数据，此时如需禁用某行展开则设置 `RowExpandable`。

可以通过设置 `IndentSize` 以控制每一层的缩进宽度。

## en-US

Tables can nest rows into a tree structure when `TreeChildren` is set. 

You can force the table to show the expand button by setting `OnExpand`, then disable the expansion of a row by setting `RowExpandable`.

You can control the width of the indent by setting `IndentSize`.