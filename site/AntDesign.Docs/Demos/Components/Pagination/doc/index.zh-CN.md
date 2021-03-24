---
category: Components
subtitle: 分页
type: 导航
title: Pagination
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/1vqv2bj68/Pagination.svg
---

采用分页的形式分隔长列表，每次只加载一个页面。

## 何时使用

- 当加载/渲染所有数据将花费很多时间时；
- 可切换页码浏览数据。

## API


| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Current | 当前页数 | number | - |  |
| DefaultCurrent | 默认的当前页数 | number | 1 |  |
| DefaultPageSize | 默认的每页条数 | number | 10 |  |
| Disabled | 禁用分页 | boolean | - |  |
| HideOnSinglePage | 只有一页时是否隐藏分页器 | boolean | false |  |
| ItemRender | 用于自定义页码的结构，可用于优化 SEO | RenderFragment(PaginationItemRenderContext) | - |  |
| PageSize | 每页条数 | number | - |  |
| PageSizeOptions | 指定每页可以显示多少条 | string\[] | \['10', '20', '50', '100'] |  |
| ShowLessItems | 是否显示较少页面内容 | boolean | false |  |
| ShowQuickJumper | 是否可以快速跳转至某页 | boolean | false |  |
| GoButton | 快速跳转按钮,对应原版 ShowQuickJumper: { goButton: ReactNode } | RenderFragment? | null |  |
| ShowSizeChanger | 是否展示 `pageSize` 切换器，当 `total` 大于 `50` 时默认为 `true` | boolean | - |  |
| ShowTitle | 是否显示原生 tooltip 页码提示 | boolean | true |  |
| ShowTotal | 用于显示数据总量和当前数据顺序 | Func<PaginationTotalContext, string>, RenderFragment<PaginationTotalContext> | - |  |
| Simple | 当添加该属性时，显示为简单分页 | boolean | - |  |
| Size | 当为「small」时，是小尺寸分页 | "default" \| "small" | "" |  |
| Responsive | (未实现)当 size 未指定时，根据屏幕宽度自动调整尺寸 | boolean | - |  |
| Total | 数据总数 | number | 0 |  |
| OnChange | 页码改变的回调，参数是改变后的页码及每页条数 | Function(PaginationEventArgs) | null |  |
| OnShowSizeChange | pageSize 变化的回调 | Function(PaginationEventArgs) | null |  |
