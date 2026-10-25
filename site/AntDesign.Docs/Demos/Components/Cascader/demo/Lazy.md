---
order: 14
title:
  zh-CN: 动态加载选项
  en-US: Load options lazily
---

## zh-CN

设置节点 `IsLeaf="false"` 并提供 `LoadData` 回调，可在展开节点时异步加载子项。回调接收从根节点到当前节点的路径，并将新节点添加到最后一个节点的 `Children`。

## en-US

Set `IsLeaf="false"` and provide `LoadData` to load children asynchronously when a node is expanded. The callback receives the path from the root to the selected node; append the new options to the last node's `Children`.
