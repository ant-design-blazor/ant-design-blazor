---
order: 33
title:
  en-US: Wide table performance
  zh-CN: 大型表格性能
---

## zh-CN

这个示例从 80 列和 20 行开始，包含固定列、选择列、操作列和纵向虚拟滚动。可以添加行或列来观察表格更新。

表格会为每列生成单元格内容，不再为每个单元格单独创建列组件实例，减少宽表格更新时的组件开销。这个示例显式启用 `Quick`，以使用快速渲染路径；1.x 中该选项默认关闭，以保留旧的 `ChildContent` 渲染行为。快速模式仍会调用一次 `ChildContent` 来初始化列定义；2.0 移除的应是表头和每行重复执行的旧渲染路径，而不是列定义入口本身。启用虚拟化后，纵向滚动区域只渲染可见行。

如果参数没有变化，也可以将 `RerenderStrategy` 设为 `RerenderStrategy.ParametersHashCodeChanged`，跳过本次渲染。对于原地修改的引用类型数据，请替换发生变化的数据，或确保其 `GetHashCode` 能反映相关字段，否则表格可能无法检测到更新。

## en-US

This example starts with 80 columns and 20 rows. It includes fixed columns, a selection column, an action column, and vertical virtualization. Add rows or columns to see the table update.

The table renders cell content through each column without creating a separate column component instance for every cell, reducing component overhead when wide tables update. This demo explicitly enables `Quick` to use the fast rendering path. It remains off by default in 1.x to preserve the legacy `ChildContent` rendering behavior. Quick mode still invokes `ChildContent` once to initialize column definitions; the 2.0 cleanup should remove repeated legacy rendering, while retaining the column-definition input. With virtualization enabled, only visible rows are rendered in the vertical scroll area.

To skip renders when parameter hashes are unchanged, set `RerenderStrategy` to `RerenderStrategy.ParametersHashCodeChanged`. For mutable reference-type data, replace changed values or ensure `GetHashCode` reflects the relevant fields so the table can detect updates.
