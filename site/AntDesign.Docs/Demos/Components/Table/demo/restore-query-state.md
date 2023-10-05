---
order: 6.7
title:
  en-US: Restore query state
  zh-CN: 筛选状态
---

## zh-CN

在调用 `ReloadData()` 时可以将包含 “分页”、“排序”、“过滤器” 等状态的 `QueryModel` 传递给 `Table` 恢复状态。
另外也可以调用 `ResetData()` 重置 `Table` 状态。

## en-US

Every table configuration with 'Filter', 'Sort', 'PageSize' and 'PageIndex' can be saved and later re-applied by `ReloadData()`. 
Also applied sorting and filters can be reseted by `ResetData()`.