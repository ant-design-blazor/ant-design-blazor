---
order: 3
title:
  zh-CN: 多列共用数据
  en-US: Multi-Column Data Sharing
additionalFiles:
  - Shared/UserMultiFieldRelation.razor
---

## zh-CN

演示多个列如何共用一次加载的数据，并支持分页功能。

在这个示例中，我们展示了一次加载用户数据，然后在多个列中显示用户的不同字段（姓名、地址、邮箱）。这避免了重复加载相同的数据，提高了性能。表格支持分页，总共包含 50 条订单记录。

**关键特性：**

- 数据共享：多个列共用一次数据加载
- 性能优化：避免重复加载相同数据
- 灵活显示：根据列需求显示不同字段
- 分页支持：支持前端分页，每页显示 10 条记录
- 加载状态：切换页面时显示加载动画

## en-US

Demonstrates how multiple columns can share the same loaded data with pagination support.

In this example, we show loading user data once and then displaying different user fields (name, address, email) in multiple columns. This avoids duplicate data loading and improves performance. The table supports pagination with 50 total order records.

**Key Features:**

- Data Sharing: Multiple columns share a single data load
- Performance Optimization: Avoids duplicate loading of the same data
- Flexible Display: Shows different fields based on column requirements
- Pagination Support: Supports client-side pagination with 10 records per page
- Loading State: Shows loading animation when switching pages
- Performance Optimization: Avoids duplicate loading of the same data
- Flexible Display: Shows different fields based on column requirements
