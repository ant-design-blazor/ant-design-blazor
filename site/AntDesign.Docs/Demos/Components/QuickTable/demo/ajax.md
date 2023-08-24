---
order: 10
title:
  en-US: Ajax
  zh-CN: 远程加载数据
---

## zh-CN

这个例子通过简单的 `HttpClient` 读取方式，演示了如何从服务端读取并展现数据，具有筛选、排序等功能以及页面 loading 效果。开发者可以自行接入其他数据处理方式。

另外，本例也展示了筛选排序功能如何交给服务端实现，列不需要指定具体的 `OnFilter` 和 `Sorter` 函数，而是在把筛选和排序的参数通过 `OnChange` 事件获取，并请求服务端来处理。

注意，此示例使用 [模拟接口](https://randomuser.me/)，展示数据可能不准确，请打开网络面板查看请求。

从0.9.0版本开始，可以设置 `RemoteDataSource` 为 `true` 来避免在数据总条数小于等于 `PageSize` 时调用客户端筛选排序分页逻辑。

## en-US

This example shows how to fetch and present data from a remote server, and how to implement filtering and sorting in server side by sending related parameters to server.

Setting `rowSelection.preserveSelectedRowKeys` to keep the `key` when enable selection.

Note, this example use [Mock API](https://randomuser.me/) that you can look up in Network Console.

From version 0.9.0, you can set `RemoteDataSource` to `true` to forbid client side pagination behaviors(this is useful when your datasource records can not fulfill a whole page so that `Table` component doesn't know whether the data is from a local data storage or a remote one').