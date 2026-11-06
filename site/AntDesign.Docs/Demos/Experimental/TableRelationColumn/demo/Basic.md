---
order: 1
title:
  zh-CN: 基本用法 - 共享关联组件
  en-US: Basic - Shared Relation Components
additionalFiles:
  - Shared/UserNameRelation.razor
---

## zh-CN

演示如何使用共享的 Razor 关联组件，这些组件支持泛型 `TItem`，可以在不同类型的表格中复用。

在这个例子中，我们展示了两个表格都使用同一个 `UserNameRelation` 组件，但 `TItem` 类型不同：
- 订单表使用 `UserNameRelation<Order>` 显示用户信息
- 员工表使用 `UserNameRelation<Employee>` 显示部门信息

**核心特性:**
- 组件可复用性：同一个组件可以在不同 `TItem` 类型中使用
- 批量加载避免 N+1 查询
- 自动去重，只加载需要的关联数据
- 零反射字段访问

## en-US

Demonstrates how to use shared Razor relation components that support generic `TItem` and can be reused across different table types.

In this example, we show two tables both using the same `UserNameRelation` component, but with different `TItem` types:
- Orders table uses `UserNameRelation<Order>` to display user information
- Employees table uses `UserNameRelation<Employee>` to display department information

**Key features:**
- Component reusability: The same component can be used with different `TItem` types
- Batch loading to avoid N+1 queries
- Automatic deduplication, only load required related data
- Zero-reflection field access
