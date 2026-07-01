---
order: 6.7
title:
  en-US: Restore Query State
  zh-CN: 筛选状态
---

## zh-CN

在调用 `ReloadData()` 时可以将包含 “分页”、“排序”、“过滤器” 等状态的 `QueryModel` 传递给 `Table` 恢复状态。
另外也可以调用 `ResetData()` 重置 `Table` 状态。

QueryModel 还提供了一些方便的使用方法：

- 提供了 `ExecuteQuery`、`ExecuteTableQuery`、`CurrentPagedRecords` 等一系列方法，结合 `IQueryable<T>` 实现对数据源的排序、筛选和分页
- 支持序列化，可通过 HttpPost 等方式传递 QueryModel 到分离的 WebApi 服务端，以使用以上方法实现数据的排序、筛选和分页。

## en-US

Every table configuration with `Filter`, `Sort`, `PageSize` and `PageIndex` can be saved and later re-applied by `ReloadData()`. 

Applied sorting and filters can also be reset by calling `ResetData()`.

QueryModel provides some convenient methods:
- It provides a series of methods like `ExecuteQuery`, `ExecuteTableQuery`, and `CurrentPagedRecords` to implement sorting, filtering, and paging on the data source in combination with `IQueryable<T>`.
- It supports serialization, allowing the `QueryModel` to be passed to a separate WebApi backend via methods like HttpPost, enabling sorting, filtering, and paging of data on the backend.