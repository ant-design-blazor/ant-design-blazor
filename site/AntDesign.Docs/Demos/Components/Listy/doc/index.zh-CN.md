---
category: Components
subtitle: 分组列表
type: 数据展示
title: Listy
---

Listy 用于渲染带分组的列表内容。

## API

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| DataSource | 列表数据 | IEnumerable&lt;TItem&gt; | - |
| GroupBy | 分组函数 | Func&lt;TItem, string&gt; | - |
| Height | 列表高度 | int | 0 |
| ItemHeight | 单项高度 | int | 32 |
