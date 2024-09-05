---
order: 17.2
title:
  en-US: Row Grouping
  zh-CN: 行分组
---

## zh-CN

设置 `Grouping` 指定列属性值进行分组，也可以设置 `GroupBy` 委托来指定分组的值。还可以设置 `GroupTitleTemplate` 自定义分组标题。

**注意：目前实现的是客户端分组，因此如果 `DataSource` 绑定的是 `IQueryable<T>`，请确保其已包含数据库分组的操作（分页、排序和筛选仍然生效）**

## en-US

Set `Grouping` to specify the column property values for grouping, 
and you can also set `GroupBy` delegation to specify the grouping values. 
You can also set `GroupTitleTemplate` to customize the group title.

**Note: Currently implementation is client-side grouping , 
so if `DataSource` is bound to `IQueryable<T>`, make sure it includes database grouping operations (paging, sorting, and filtering are still in effect)**