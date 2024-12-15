---
order: 17
title:
  en-US: Tree Data
  zh-CN: 树形数据展示
---

## zh-CN

表格支持树形数据的展示，只需设置 `TreeChildren` 属性指定子数据。利用 `OnExpand` 可按需加载子数据。

可以通过设置 `IndentSize` 以控制每一层的缩进宽度。

## en-US

Tables can nest rows into a tree structure when `TreeChildren` is set. The `OnExpand` callback can be used to load children data on demand.

You can control the width of the indent by setting `IndentSize`.