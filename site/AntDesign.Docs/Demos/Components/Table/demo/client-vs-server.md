---
order: 32
title:
  en-US: Client side pagination vs Server side pagination
  zh-CN: 客户端分页vs服务器分页
---

## zh-CN

有两种分页模式：
- 客户端分页：所有数据一次性加载到客户端，由 Table 组件实现排序、筛选、分页功能。
- 服务器分页：每次从服务器加载一页数据，排序、筛选、分页功能需要由服务器实现。

可使用 `PaginationMode` 属性控制 Table 组件的分页模式。可以为 `PaginationMode` 属性指定的值有：
- `TablePaginationMode.Auto`：默认值。如果 `Total` 属性的值大于 `DataSource` 集合中项的数量，则使用服务器分页模式；否则使用客户端分页模式。
- `TablePaginationMode.Server`：锁定使用服务器分页模式，客户端排序、筛选、分页相关逻辑永远不会执行，即使现有数据不能填充满一页也是如此。
- `TablePaginationMode.Client`：锁定使用客户端分页模式，`Total` 属性的值将被忽略。

## en-US

There are two pagination modes:
-Client side pagination: all data is loaded to the client at one time, and the table component implements the functions of sorting, filtering and paging.
-Server side pagination: load one page of data from the server each time. The functions of sorting, filtering and paging need to be implemented by the server.

You can use the `PaginationMode` attribute to control the pagination mode of the table component. The values that can be specified for the `PaginationMode` attribute are:
- `TablePaginationMode.Auto`: this is the default value. If the value of the 'Total' attribute is greater than the number of items in the 'DataSource' collection, server side pagination is used; Otherwise, use client side pagination.
- `TablePaginationMode.Server`: always use server side pagination, the client sorting, filtering and paging logic will never be executed, even if the existing data cannot fill a page.
- `TablePaginationMode.Client`: always use client side pagination, the value of `Total` attribute will be ignored.