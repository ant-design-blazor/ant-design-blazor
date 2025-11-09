---
order: 2
title:
  zh-CN: 虚拟化 + 关联列演示
  en-US: Virtualization + Relation Column Demo
additionalFiles:
  - Shared/UserNameRelation.razor
---

## zh-CN

演示如何在虚拟化表格中使用关联列，通过 `UserNameRelation` 组件实现用户信息的批量异步加载与缓存。

**核心特性:**
- 支持表格虚拟化，提升大数据量渲染性能
- 通过 `UserNameRelation` 组件实现关联字段的批量加载和缓存
- 仅对未缓存的 id 进行数据请求，避免重复加载
- 组件可复用性，支持泛型 `TItem`

## en-US

Demonstrates how to use relation columns in virtualized tables, implementing batch asynchronous loading and caching of user information through the `UserNameRelation` component.

**Key features:**
- Supports table virtualization to improve rendering performance for large datasets
- Implements batch loading and caching of related fields through `UserNameRelation` component
- Only requests data for uncached ids, avoiding redundant loads
- Component reusability with generic `TItem` support