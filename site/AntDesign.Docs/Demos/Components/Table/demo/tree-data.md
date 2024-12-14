---
order: 17
title:
  en-US: Tree Data
  zh-CN: 树形数据展示
---

## zh-CN

表格支持树形数据的展示，当数据中有 `children` 字段时会自动展示为树形表格，如果不需要或配置为其他字段可以用 `childrenColumnName` 进行配置。

可以通过设置 `indentSize` 以控制每一层的缩进宽度。

## en-US

Tables can nest rows into a tree structure when `TreeChildren` is set.

You can control the width of the indent by setting `IndentSize`.